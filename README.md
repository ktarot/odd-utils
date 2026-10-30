# ODD Utils

Windows용 포터블 광학디스크 유틸리티입니다. **ODD 복사 기능이 기본**이며 음악 재생은 같은 창에서 열 수 있는 보조 서랍으로 제공합니다.

## 기존 기능 유지

- 광학 드라이브 검색, CD/DVD/BD 계열 표시(장치 모델로 식별 가능한 경우), 준비 상태 표시
- 트레이 열기/닫기, 저장 폴더 선택 및 생성
- 디스크 파일 복사, 동일 이름 충돌 방지, 진행률, 복사 중지 및 완료 안내
- LED Dot Matrix 진행 표시, Light/Dark 테마

## 추가 기능

- 기본 창 360×640, 창 크기 및 DPI에 대응하는 반응형 레이아웃
- 같은 화면에서 펼치는 음악 재생 서랍과 내부 스크롤
- MP3, M4A, WAV, WMA, FLAC, AAC, AIFF 등 파일 재생(Windows 코덱에 따라 달라짐)
- 음악 CD(CD-DA) 재생(레거시 MCI 방식으로 일부 드라이브 미지원)
- 이전/다음 트랙, 재생/일시정지/정지, 트랙 목록, 기존 LED 재생률 연동
- 같은 이름의 LRC 또는 TXT 파일에서 가사 표시

## EXE 실행

1. [Actions](https://github.com/ktarot/odd-utils/actions)에서 최신 성공한 Build ODD Utils를 엽니다.
2. Artifacts에서 ODD-Utils-Windows-x64를 다운로드합니다.
3. ZIP 압축을 해제한 뒤 ODD_Utils.exe를 실행합니다.

## 구성 파일

- ODD_Utils.csproj: .NET 10 Windows Forms 프로젝트
- MainForm.cs: 기존 복사 기능 + 서랍형 반응형 UI + LED
- AudioPlayback.cs: 오디오 파일/음악 CD 재생 및 LRC/TXT 가사
- Program.cs: 실행 진입점
- Directory.Build.props: NAudio 의존성
- .github/workflows/build.yml: Windows 클라우드 빌드
- [음악 재생 사용설명서](./AUDIO_PLAYER_GUIDE.md)

## 제한사항

- 데이터 디스크 복사에는 저작권 보호 우회 기능이 없습니다.
- 음악 CD의 CDA 항목은 독립 음원 파일이 아니며 데이터 복사만으로 음악을 추출할 수 없습니다.
- 음악 CD 재생은 Windows 레거시 MCI/ODD 호환성에 따라 실패할 수 있습니다.
- 온라인 가사 검색 또는 내장 가사 태그 추출은 아직 구현하지 않았습니다.
- 빌드 성공은 Windows 하드웨어/코덱별 재생 성공을 보장하지 않습니다.

기존 저장소 ktarot/dvd-copier는 별도로 유지합니다.
