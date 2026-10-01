# 석류의 맛 한글패치

Super Famicom 《석류의 맛 / Zakuro no Aji》의 한글패치 **v2.0m 공개 후보판**입니다.

## 다운로드와 적용

[한글패치 ZIP 다운로드](https://github.com/ayaneros-eng/zakuro-no-aji-korean-patch/releases/download/v2.0m/Zakuro_v2.0m_PUBLIC_PATCH_BUNDLE.zip) · [배포 페이지](https://github.com/ayaneros-eng/zakuro-no-aji-korean-patch/releases/tag/v2.0m)

1. ZIP을 다운로드하고 한 폴더에 모두 압축을 풉니다.
2. Windows에서는 깨끗한 일본 원본 ROM을 **Apply_Korean.bat** 위로 끌어 놓습니다.
3. 원본과 같은 폴더에 `Zakuro_no_Aji_Korean_v2.0m_ReleaseCandidate.sfc`가 만들어집니다.

Windows에서는 Python이나 이전 한글패치가 필요 없습니다. 원본은 직접 준비해야 합니다. 게임 ROM은 제공하지 않습니다.

macOS/Linux에서는 Python 3.8 이상으로 압축을 푼 폴더에서 실행합니다.

```sh
python3 -X utf8 apply_korean.py "일본 원본.sfc"
```

## 적용할 원본

512바이트 추가 헤더가 없는 일본 원본, 크기 **2,097,152바이트**를 사용합니다.

| 파일 | SHA256 |
| --- | --- |
| 일본 원본 | `4e0a7f639c185b49abe70de16bf7ef062a316d986753c7aabd9b98e6b956d0b6` |
| 적용 결과 | `9a447a967ccfe2cecb0485f3def940d8067dcfc35133a5cf5d6436784030657f` |

동봉 실행기는 일본 원본 전용입니다. 정확한 v2.0j/k/l 실행본이 있는 경우 `extras`의 해당 업데이트 BPS를 BPS 호환 도구로 적용할 수 있습니다. 각 버전의 해시는 [적용 설명](patch/README_KO.txt)에 있습니다.

## 이번 버전

- 선택지 첫 인물 이름 누락 19곳을 복구했습니다.
- 띄어쓰기 11곳, 어법 3곳과 줄바꿈을 교정했습니다.
- 이전 버전의 지명, 저장 회차 단위, 저장 제목과 제작진 역할 표시 교정을 포함합니다.
- Windows 적용기는 원본과 기존 파일을 보존하며 원본·패치·결과 해시를 검증합니다.

## 검수 범위

선택지 70개 화면, 수정된 32개 선택 항목의 목표 분기, 정상 엔딩 5종 연속 회차와 일반 저장 호환성을 확인했습니다. 전체 대사와 모든 선택 조합, 자연 달성률 100%, 실제 기기 및 음성 출력의 전수 검수는 남아 있습니다. [검수 범위](patch/QA_SCOPE_KO.txt)를 확인해 주세요.

다른 ROM 버전의 세이브스테이트를 사용하지 마세요. 엔딩의 ‘끝’ 화면에 머무르는 경로는 에뮬레이터를 리셋한 뒤 메뉴에서 재시작합니다.

## 문제 제보

[Issues](https://github.com/ayaneros-eng/zakuro-no-aji-korean-patch/issues)에 패치 버전, 에뮬레이터 버전, 진입 순서와 선택지를 적어 주세요. 첨부 자료의 개인 이름·이메일·로컬 경로는 지워 주세요. 게임 ROM은 첨부하지 마세요.

추가 UI 글꼴의 고지와 라이선스는 [FONT_NOTICE.txt](patch/FONT_NOTICE.txt)와 [OFL-Noto.txt](patch/OFL-Noto.txt)에 있습니다. 원작 게임과 원본 데이터의 권리는 해당 권리자에게 있습니다.
