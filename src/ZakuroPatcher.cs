using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;

// Builds with the Windows .NET Framework C# compiler; no Python installation.
internal static class ZakuroPatcher {
    const string SourceHash = "4e0a7f639c185b49abe70de16bf7ef062a316d986753c7aabd9b98e6b956d0b6";
    const string TargetHash = "9a447a967ccfe2cecb0485f3def940d8067dcfc35133a5cf5d6436784030657f";
    const string PatchName = "Zakuro_original_to_v2.0m.bps";
    const string OutputName = "Zakuro_no_Aji_Korean_v2.0m_ReleaseCandidate.sfc";
    const int Limit = 32 * 1024 * 1024;
    sealed class PatchError : Exception { public readonly int Code; public PatchError(int c,string s):base(s){Code=c;} }
    static string Hash(byte[] b) { using(var s=SHA256.Create())return BitConverter.ToString(s.ComputeHash(b)).Replace("-","").ToLowerInvariant(); }
    static uint Crc(byte[] b,int count) { uint c=0xffffffff; for(int i=0;i<count;i++){c^=b[i];for(int j=0;j<8;j++)c=(c>>1)^((c&1)!=0?0xedb88320u:0);}return ~c; }
    static uint U32(byte[] b,int p){return (uint)b[p]|((uint)b[p+1]<<8)|((uint)b[p+2]<<16)|((uint)b[p+3]<<24);}
    static long Var(byte[] b,ref int p,int end) {
        long v=0,s=1;
        for(int n=0;n<9;n++){
            if(p>=end)throw new PatchError(4,"패치 데이터가 잘렸습니다.");
            byte x=b[p++]; checked {v+=(x&127)*s;}
            if((x&128)!=0)return v;
            checked {s<<=7;v+=s;}
        }
        throw new PatchError(4,"패치 숫자 형식이 잘못됐습니다.");
    }
    static byte[] Apply(byte[] source,byte[] patch) {
        if(patch.Length<19 || Encoding.ASCII.GetString(patch,0,4)!="BPS1")throw new PatchError(4,"BPS 패치 형식이 잘못됐습니다.");
        int end=patch.Length-12;
        if(Crc(patch,patch.Length-4)!=U32(patch,patch.Length-4))throw new PatchError(4,"패치 파일이 손상됐습니다. 다시 압축을 풀어 주세요.");
        if(Crc(source,source.Length)!=U32(patch,end))throw new PatchError(3,"패치와 원본이 일치하지 않습니다.");
        int p=4;long sl=Var(patch,ref p,end),tl=Var(patch,ref p,end),meta=Var(patch,ref p,end);
        if(sl!=source.Length || tl!=4194304 || meta<0 || meta>end-p)throw new PatchError(4,"패치 크기 정보가 잘못됐습니다.");
        p+=(int)meta;byte[] target=new byte[(int)tl];int pos=0;long sr=0,tr=0;
        while(pos<target.Length){
            long action=Var(patch,ref p,end),len=(action>>2)+1;
            if(len<=0 || len>target.Length-pos)throw new PatchError(4,"패치 쓰기 범위를 벗어났습니다.");
            int n=(int)len;
            switch((int)(action&3)){
                case 0:
                    if(n>source.Length-pos)throw new PatchError(4,"원본 읽기 범위를 벗어났습니다.");
                    Buffer.BlockCopy(source,pos,target,pos,n);break;
                case 1:
                    if(n>end-p)throw new PatchError(4,"패치 본문이 잘렸습니다.");
                    Buffer.BlockCopy(patch,p,target,pos,n);p+=n;break;
                case 2:
                    long a=Var(patch,ref p,end);checked{sr+=(a&1)!=0?-(a>>1):(a>>1);}
                    if(sr<0 || sr>source.Length-n)throw new PatchError(4,"원본 복사 범위를 벗어났습니다.");
                    Buffer.BlockCopy(source,(int)sr,target,pos,n);sr+=n;break;
                case 3:
                    long z=Var(patch,ref p,end);checked{tr+=(z&1)!=0?-(z>>1):(z>>1);}
                    if(tr<0 || tr>=pos)throw new PatchError(4,"결과 복사 범위를 벗어났습니다.");
                    for(int i=0;i<n;i++)target[pos+i]=target[(int)tr+i];tr+=n;break;
            }
            pos+=n;
        }
        if(p!=end || Crc(target,target.Length)!=U32(patch,end+4) || Hash(target)!=TargetHash)throw new PatchError(4,"패치 결과 검증에 실패했습니다. 파일을 만들지 않았습니다.");
        return target;
    }
    static byte[] Read(string path,int limit) {
        var f=new FileInfo(path);if(!f.Exists)throw new PatchError(6,"파일을 찾을 수 없습니다: "+path);
        if(f.Length>limit)throw new PatchError(3,"지원하는 파일 크기를 초과했습니다.");return File.ReadAllBytes(path);
    }
    static int Run(string[] args) {
        if(args.Length<1 || args.Length>2){Console.WriteLine("사용법: ZakuroPatcher.exe \"일본 원본.sfc\" [결과.sfc]");Console.WriteLine("Windows에서는 원본 파일을 Apply_Korean.bat 위로 끌어 놓으세요.");return 2;}
        string source=Path.GetFullPath(args[0]);byte[] data=Read(source,Limit);
        if(Hash(data)!=SourceHash)throw new PatchError(3,"깨끗한 일본 원본이 필요합니다 (2,097,152 bytes, 추가 헤더 없음).\n현재 SHA256: "+Hash(data)+"\n필요 SHA256: "+SourceHash);
        string output=Path.GetFullPath(args.Length==2?args[1]:Path.Combine(Path.GetDirectoryName(source),OutputName));
        if(String.Equals(source,output,StringComparison.OrdinalIgnoreCase))throw new PatchError(5,"원본 파일을 결과 경로로 지정할 수 없습니다.");
        if(File.Exists(output)){
            if(new FileInfo(output).Length==4194304 && Hash(File.ReadAllBytes(output))==TargetHash){Console.WriteLine("이미 검증된 패치 결과가 있습니다: "+output);return 0;}
            throw new PatchError(5,"같은 이름의 다른 파일이 있습니다. 기존 파일을 보존했습니다.\n다른 결과 경로를 지정해 주세요: "+output);
        }
        string folder=Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        byte[] target=Apply(data,Read(Path.Combine(folder,PatchName),Limit));
        string temp=Path.Combine(Path.GetDirectoryName(output),".zakuro-"+Guid.NewGuid().ToString("N")+".tmp");
        try{
            using(var f=new FileStream(temp,FileMode.CreateNew,FileAccess.Write,FileShare.None)){f.Write(target,0,target.Length);f.Flush(true);}
            File.Move(temp,output); // Fails if a competing writer created output; never overwrites.
        }finally{if(File.Exists(temp))File.Delete(temp);}
        Console.WriteLine("패치 완료: "+output);Console.WriteLine("SHA256: "+TargetHash);return 0;
    }
    public static int Main(string[] args) {
        Console.OutputEncoding=new UTF8Encoding(false);
        try{return Run(args);}catch(PatchError e){Console.Error.WriteLine(e.Message);return e.Code;}
        catch(OverflowException){Console.Error.WriteLine("패치 숫자 형식이 잘못됐습니다.");return 4;}
        catch(Exception e){Console.Error.WriteLine("파일 처리 실패: "+e.Message);return 6;}
    }
}
