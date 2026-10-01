#!/usr/bin/env python3
"""Verified one-step BPS installer. Python 3.8+; no third-party modules."""
from pathlib import Path
import hashlib,sys,os,tempfile,zlib
SOURCE_SHA='4e0a7f639c185b49abe70de16bf7ef062a316d986753c7aabd9b98e6b956d0b6'
TARGET_SHA='9a447a967ccfe2cecb0485f3def940d8067dcfc35133a5cf5d6436784030657f'
PATCH='Zakuro_original_to_v2.0m.bps'
OUTPUT='Zakuro_no_Aji_Korean_v2.0m_ReleaseCandidate.sfc'
def sha(b):return hashlib.sha256(b).hexdigest()
def apply(source,patch):
 if len(patch)<19 or patch[:4]!=b'BPS1':raise ValueError('BPS 헤더가 잘못됐습니다.')
 end=len(patch)-12
 if zlib.crc32(patch[:-4])!=int.from_bytes(patch[-4:],'little'):raise ValueError('패치 파일이 손상됐습니다.')
 if zlib.crc32(source)!=int.from_bytes(patch[end:end+4],'little'):raise ValueError('원본 CRC가 일치하지 않습니다.')
 p=4
 def var():
  nonlocal p
  value=0;shift=1
  for _ in range(9):
   if p>=end:raise ValueError('패치 데이터가 잘렸습니다.')
   x=patch[p];p+=1;value+=(x&127)*shift
   if x&128:return value
   shift<<=7;value+=shift
  raise ValueError('패치 숫자 범위를 벗어났습니다.')
 sl,tl,ml=var(),var(),var()
 if sl!=len(source) or tl!=4194304 or ml>end-p:raise ValueError('패치 크기 정보가 잘못됐습니다.')
 p+=ml;out=bytearray();sr=tr=0
 while len(out)<tl:
  cmd=var();mode=cmd&3;n=(cmd>>2)+1;pos=len(out)
  if n>tl-pos:raise ValueError('쓰기 범위를 벗어났습니다.')
  if mode==0:
   if pos+n>len(source):raise ValueError('원본 읽기 범위를 벗어났습니다.')
   out.extend(source[pos:pos+n])
  elif mode==1:
   if p+n>end:raise ValueError('패치 본문이 잘렸습니다.')
   out.extend(patch[p:p+n]);p+=n
  elif mode==2:
   d=var();sr+=-(d>>1) if d&1 else d>>1
   if sr<0 or sr+n>len(source):raise ValueError('원본 복사 범위를 벗어났습니다.')
   out.extend(source[sr:sr+n]);sr+=n
  else:
   d=var();tr+=-(d>>1) if d&1 else d>>1
   if tr<0 or tr>=pos:raise ValueError('결과 복사 범위를 벗어났습니다.')
   for i in range(n):out.append(out[tr+i])
   tr+=n
 if p!=end or zlib.crc32(out)!=int.from_bytes(patch[end+4:end+8],'little') or sha(out)!=TARGET_SHA:raise ValueError('패치 결과 검증에 실패했습니다.')
 return bytes(out)
def main():
 if len(sys.argv) not in [2,3]:print('사용법: python3 apply_korean.py "일본 원본.sfc" [결과.sfc]');return 2
 source=Path(sys.argv[1]).resolve();dest=Path(sys.argv[2]).resolve() if len(sys.argv)==3 else source.with_name(OUTPUT)
 try:
  if source.stat().st_size!=2097152:print('추가 헤더 없는 깨끗한 일본 원본 2,097,152 bytes가 필요합니다.');return 3
  data=source.read_bytes()
  if sha(data)!=SOURCE_SHA:print('원본 SHA256이 일치하지 않습니다.\n현재: '+sha(data)+'\n필요: '+SOURCE_SHA);return 3
  if source==dest:print('원본 파일을 결과 경로로 지정할 수 없습니다.');return 5
  if dest.exists():
   if dest.stat().st_size==4194304 and sha(dest.read_bytes())==TARGET_SHA:print('이미 검증된 패치 결과가 있습니다: '+str(dest));return 0
   print('같은 이름의 다른 파일이 있습니다. 기존 파일을 보존했습니다: '+str(dest));return 5
  patch=Path(__file__).with_name(PATCH)
  if patch.stat().st_size>32*1024*1024:raise ValueError('패치가 너무 큽니다.')
  result=apply(data,patch.read_bytes());temp=None
  try:
   with tempfile.NamedTemporaryFile(prefix='.zakuro-',suffix='.tmp',dir=str(dest.parent),delete=False) as f:
    temp=Path(f.name);f.write(result);f.flush();os.fsync(f.fileno())
   os.link(temp,dest) # Atomic create; refuses to overwrite an existing file.
  finally:
   if temp is not None:temp.unlink(missing_ok=True)
  print('패치 완료: '+str(dest)+'\nSHA256: '+TARGET_SHA);return 0
 except ValueError as e:print('패치 검증 실패: '+str(e));return 4
 except OSError as e:print('파일 처리 실패: '+str(e));return 6
if __name__=='__main__':raise SystemExit(main())
