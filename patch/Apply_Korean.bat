@echo off
setlocal DisableDelayedExpansion
chcp 65001 >nul
if "%~1"=="" (
  echo 일본 원본 ROM 파일을 이 파일 위로 끌어 놓으세요.
  echo 사용법: Apply_Korean.bat "일본 원본.sfc" [결과.sfc]
  set "zakuro_result=2"
  goto finish
)
if "%~2"=="" (
  "%~dp0ZakuroPatcher.exe" "%~f1"
) else (
  "%~dp0ZakuroPatcher.exe" "%~f1" "%~f2"
)
set "zakuro_result=%errorlevel%"
:finish
if not defined ZAKURO_PATCH_NO_PAUSE pause
exit /b %zakuro_result%
