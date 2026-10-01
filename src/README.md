# Windows 적용기 소스

`ZakuroPatcher.cs`는 배포된 Windows 적용기의 C# 소스입니다. .NET Framework의 C# 컴파일러로 빌드하며 디버그 정보는 포함하지 않습니다.

```powershell
& "$env:WINDIR\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /optimize+ /target:exe /out:ZakuroPatcher.exe ZakuroPatcher.cs
```

실행 파일과 원본용 BPS를 같은 폴더에 두세요. Python 적용기 소스는 `patch/apply_korean.py`에 있습니다.
