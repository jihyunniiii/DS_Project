# 🐘 DS_Project
동국대학교 기업사회캡스톤디자인1 "동심" 프로젝트

### 📌소개
동국대학교 메타버스 '동심' <br/>
<img width="40%" src="https://user-images.githubusercontent.com/103172971/224857253-8825967c-6c1d-4e55-ae24-6da49b811088.png">

### 👨🏻‍🤝‍👨🏻 Team
| **이름** | **역할** |
| ----- | ------------ |
| [김민석](https://github.com/Kim-minseok123) | 팀장, 게임 맵 구현, 서버, 채팅 기능 구현 |
| [강채은](https://github.com/Chaeniiiii) | 메인 맵 구현 |
| [김다예](https://github.com/yeahh315) | 게임 맵 구현 |
| [배지현](https://github.com/jihyunniiii) | 메인 맵 구현 |
| [추호진](https://github.com/whochoo) | 타이틀 화면 구현 및 모델링 작업 |

### ⏰ 개발 기간
- 2022.03.02 ~ 2022.06.16

### ⚙ 기술 스택
<img src="https://img.shields.io/badge/Unity-000000?style=for-the-badge&logo=Unity&logoColor=white"> <img src="https://img.shields.io/badge/Blender-F5792A?style=for-the-badge&logo=Blender&logoColor=white"> <img src="https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white"/>

### 🔍 화면 소개
| <img src="https://user-images.githubusercontent.com/103172971/224860471-cfd6bd99-660d-4c57-a9d7-5b00bbbe891a.png"> | <img src="https://user-images.githubusercontent.com/103172971/224860884-da05db97-0804-4e10-a671-f1d21753af1f.png"> | <img src="https://user-images.githubusercontent.com/103172971/224860954-c439ce36-058d-4372-972d-a42e5aaebc4e.png"> |
| :-----: | :-----: | :-----: |
| 로그인 화면 | 회원가입 화면 | 접속 화면 |
| <img src="https://user-images.githubusercontent.com/103172971/224861460-02f49917-dbf5-476f-a39f-e1326c898c43.png"> | <img src="https://user-images.githubusercontent.com/103172971/224861727-da3f6b86-c18e-412a-a94a-c16cb8f1a2e6.png"> | <img src="https://user-images.githubusercontent.com/103172971/224861949-aeaeb0c5-17c4-4e7f-9227-cacdca35bad4.png"> |
| 메인 맵 구성 - 로비 | 메인 맵 구성 - 언덕 | 게임 맵 구성 |

### 💡 기능
1. 게임
- 명진관 앞에 있는 입간판 보드 근처 바닥과 충돌 시 게임룸 입장 가능
- 제일 낮은 층 타일에서 떨어질 시 게임 종료
- 게임 종료 시 결과에 따라 코인 지급 + 자동으로 메인 맵으로 돌아감

| <img src="https://user-images.githubusercontent.com/103172971/224865995-20da1bc7-c1e3-4cfc-9aab-b1e978dce647.png"> | <img src="https://user-images.githubusercontent.com/103172971/224866208-34823d38-463d-470b-acad-47058d7c27dc.png"> | <img src="https://user-images.githubusercontent.com/103172971/224866414-cb64587b-5c05-4fb3-bf82-ce619fe4114f.png"> |
| :-----: | :-----: | :-----: |
| 게임룸 입장 | 게임 화면 | 게임 진행 |

2. 채팅
- 'F2' 버튼 클릭 시 채팅 기능 사용 가능
- 접속자 목록 확인 가능
- 귓속말 채팅 가능

| <img src="https://user-images.githubusercontent.com/103172971/224862359-d34de5d8-ce46-445a-91cc-e05954a336b3.png"> | <img src="https://user-images.githubusercontent.com/103172971/224862403-d9152e03-76fb-4e85-88ff-0b46edf875b7.png"> | <img src="https://user-images.githubusercontent.com/103172971/224862597-6af851e8-a5dd-4151-a6e3-86465d85be6f.png"> |
| :-----: | :-----: | :-----: |
| 채팅 열기 | 채팅 그룹 선택 | 채팅 시작 |

3. 시스템 설정 (UI)
- 'ESC' 버튼 클릭 시 각종 UI 등장
- 랭킹, 스크린샷 촬영, 설정 기능

| <img src="https://user-images.githubusercontent.com/103172971/224862901-9b185e80-ebb0-46a1-8d22-34b1a8926cb5.png"> | <img src="https://user-images.githubusercontent.com/103172971/224863921-ab7e0ec6-ae43-489a-9430-ee723bd101a0.png"> | <img src="https://user-images.githubusercontent.com/103172971/224863335-6bb343a0-5efc-4071-ae03-fc92f6b49d39.png"> |
| :-----: | :-----: | :-----: |
| 시스템 설정 열기 | 랭킹 | 스크린샷 촬영 - 저장 |
| <img src="https://user-images.githubusercontent.com/103172971/224863418-683b934f-dd50-492a-95e8-72f383d4a31c.png"> | <img src="https://user-images.githubusercontent.com/103172971/224863560-2e7e0dfb-cd9e-4a55-962a-bee15b1e46d9.png"> | <img src="https://user-images.githubusercontent.com/103172971/224863646-e7b0be20-256e-4abe-b78d-ff1a6dd9f9e5.png"> |
| 스크린샷 촬영 - 촬영 사진 | 시스템 설정 - 사운드 | 시스템 설정 - 그래픽 |

4. 세부 기능 (이벤트, 상호작용) - 상점
- 중앙도서관 앞에 있는 입간판 보드 클릭 시 상점 입장 가능

| <img src="https://user-images.githubusercontent.com/103172971/224864149-03aa2d6a-b3dc-4132-a8c2-c6fc840d1dfa.png"> | <img src="https://user-images.githubusercontent.com/103172971/224864263-88f7504e-f5d3-49ee-a029-6b7874bfd808.png"> | <img src="https://user-images.githubusercontent.com/103172971/224864427-cc50a2ec-4109-400a-a284-d71de4c3efd1.png"> |
| :-----: | :-----: | :-----: |
| 상점 입장 | 연등 구매 | 소원 입력 |

5. 세부 기능 (이벤트, 상호작용) - 연등 설치
- 'R' 버튼 클릭 시 연등 설치 가능
- 상점에서 연등 구매 후 설치 가능
- 연등을 발판 삼아 이동 가능

| <img src="https://user-images.githubusercontent.com/103172971/224865386-9d8a3dcd-904e-4bea-9f90-8db4901a6b41.png"> |
| :-----: |
| 연등 설치 |

6. 세부 기능 (이벤트, 상호작용) - 랜덤 연꽃 이벤트
- 랜덤으로 생기는 연꽃을 5의 배수마큼 획득하면 불교 명언의 한 어절 획득할 수 있음
- 불교 명언의 어절을 모아 문장을 만들 수 있음

| <img src="https://user-images.githubusercontent.com/103172971/224864819-7cd70e8e-546d-480e-acae-e58faadce04d.png"> | <img src="https://user-images.githubusercontent.com/103172971/224864899-c739fd2d-97fe-4c63-8a0b-a46b2fef52d6.png"> |
| :-----: | :-----: |
| 연꽃 획득 | 어절 획득 |

7. 세부 기능 (이벤트, 상호작용) - 쿠폰 획득 이벤트
- 공중에 있는 쿠폰존에 도달할 경우 쿠폰 획득

| <img src="https://user-images.githubusercontent.com/103172971/224865591-9811cdda-e02a-4582-bfd2-e354093901e9.png"> | <img src="https://user-images.githubusercontent.com/103172971/224865640-1c93c33e-fd98-47cc-ba2a-a2a9573fcdff.png"> |
| :-----: | :-----: |
| 쿠폰존 | 쿠폰 획득 화면 |

### 📝 
[metaverse_dongsim_최종보고서](https://github.com/jihyunniiii/DS_Project/files/10963250/metaverse_dongsim_.pdf)
[metaverse_dongsim_최종발표자료](https://github.com/jihyunniiii/DS_Project/files/10963252/metaverse_dongsim_.pdf)
[metaverse_dongsim_시연영상](https://www.youtube.com/watch?v=sEu9GWfyJcc)
