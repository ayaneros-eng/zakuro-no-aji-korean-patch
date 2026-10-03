# 석류의 맛 한글패치 · v2.0m-E

Super Famicom 《석류의 맛 / Zakuro no Aji》의 **E형 폰트 가독성 개선판**입니다.
기존 v2.0m 한국어 패치를 바탕으로, 글자 구별을 우선한 정자형 픽셀 한글을 적용했습니다.

[**v2.0m-E 패치 ZIP 다운로드**](https://github.com/ayaneros-eng/zakuro-no-aji-korean-patch/raw/refs/heads/main/releases/v2.0m-E/Zakuro_v2.0m-E_PUBLIC_PATCH.zip) · [v2.0m-E 배포 안내](releases/v2.0m-E/README.md) · [오류 제보](https://github.com/ayaneros-eng/zakuro-no-aji-korean-patch/issues)

## 이번 변경

본문·추가 UI용 한글 1,128칸(서로 다른 979종)을 E형으로 교체했습니다. 초성·모음·받침을 픽셀 격자에 맞춰 구성하여 ‘도/드’, ‘온/은’처럼 혼동하기 쉬운 글자의 구별을 우선했습니다.

기존 v2.0m 대비 변경은 한글 글리프와 ROM 체크섬에 한정됩니다. 번역 문장·분기·엔진 코드·본문 포인터 1,342개는 그대로이며, 별도로 그려진 작은 메뉴 그래픽과 일부 영문/기호는 기존 것을 유지합니다. 이 버전은 새 번역본이나 모든 문제를 해결한 완전 무결판을 의미하지 않습니다.

### 기존 v2.0m과 E형 비교

![기존 v2.0m과 v2.0m-E E형 비교](docs/compare_public/01_opening_before_after.png)

[기존판과 E형 비교 화면 보기](docs/SCREENSHOTS.md)

## 적용 방법

1. 위의 **v2.0m-E 패치 ZIP 다운로드**에서 `Zakuro_v2.0m-E_PUBLIC_PATCH.zip`을 받아 모두 압축 해제합니다.
2. 직접 준비한 일본 원본에 `Zakuro_Japanese_to_v2.0m-E.bps`를 BPS 호환 패처로 적용합니다.
3. 기존 한글 v2.0m에서 갱신할 경우에는 `updates/Zakuro_v2.0m_to_v2.0m-E.bps`를 사용합니다.

Python 3.8 이상 환경에서는 동봉된 `apply_patch.py`도 사용할 수 있습니다. **두 BPS를 연속으로 적용하지 마세요.** 구버전 j/k/l 또는 A/B/C/D/F/G 시험판 위에 덧붙이지 마세요. 원본 게임 ROM은 배포하지 않습니다.

## 입력·출력 확인

| 파일 | 크기 | SHA-256 |
|---|---:|---|
| 추가 헤더 없는 일본 원본 | 2,097,152 bytes | `4e0a7f639c185b49abe70de16bf7ef062a316d986753c7aabd9b98e6b956d0b6` |
| 기존 한글 v2.0m | 4,194,304 bytes | `9a447a967ccfe2cecb0485f3def940d8067dcfc35133a5cf5d6436784030657f` |
| 최종 E형 | 4,194,304 bytes | `f142e809f7466a1f5543c7d99678c9a7a76ccc394e352d7660df493e0dfebbc3` |

출력 파일명은 `Zakuro_v2.0m-E_Korean.sfc`입니다. 비교 당시의 `Zakuro_v2.0m_E_Clear14.sfc`와 내용은 같습니다.

## 저장·검수 안내

다른 버전의 세이브스테이트는 사용하지 마세요. 일반 게임 저장은 원본을 보관한 복사본으로 확인하세요. Snes9x에서 일반 SRAM 한 건의 E형 이어하기를 확인했지만 모든 저장 상태의 호환을 보증하지는 않습니다.

Snes9x 1.63과 bsnes 115에서 E형의 본문·선택지 등을 확인했습니다. **E형 전체 루트·엔딩을 새로 완주한 것은 아닙니다.** 이전 v2.0m의 엔딩 검수와 이번 폰트 검수는 구분합니다. 자세한 범위는 [검수 안내](QA_SCOPE_KO.md)를 확인해 주세요.

## 피드백

[Issues](https://github.com/ayaneros-eng/zakuro-no-aji-korean-patch/issues)에 버전, 실행 환경, 문제 문장, 원래 의도된 글자와 실제로 보이는 글자, 선택 순서, 스크린샷을 남겨 주세요. 게임 ROM이나 개인 정보는 첨부하지 마세요.

[변경 기록](CHANGELOG.md) · [기여·기존 고지](notices/CREDITS_AND_NOTICES.md)
