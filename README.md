# README - BRICK BREAKER 👊🏻
- Unity를 활용한 벽돌 깨기 게임입니다.🤗
- 각 스테이지 마다 다양한 벽돌들로 구성되어 있습니다.
- 스테이지를 클리어하면 다음 스테이지가 열리고, 중간중간 보스 스테이지도 포함되어 있어 더욱 흥미롭게 즐길 수 있는 게임입니다.💪🏻

## 📆 개발 기간
24.10.15(화) ~ 24.10.21(월)

## 🤼‍♂️ 팀원 구성
🧑🏻‍💻 성요셉(팀장) - 블록, 아이템, 게임매니저 / <https://github.com/jsj2518><br>
🧑🏻‍💻 김지훈(팀원) - 플레이어, 패들 / <https://github.com/kkamsogod><br>
👩🏻‍💻 김유민(팀원) - UI 캔버스 / <https://github.com/yumin43><br>
🧑🏻‍💻 최한결(팀원) - 볼컨트롤러, 물리효과 / <https://github.com/HanGyeolChoi><br>

## 🖥️ 기술 스택
- [Language] - `C#`
- [Engine] : `Unity 2022.3.17f1`
- [IDE] : `Visual Studio 2022`, `Visual Studio Code` 
- [Framework] : `.NET 8.0`

## 👀 와이어 프레임
<img width="632" alt="스크린샷 2024-10-22 오전 3 47 30" src="https://github.com/user-attachments/assets/0e1255d4-8a67-42b5-9744-1ac769e2a1a6">

## 🔎 주요 기능
### ◽️ 시작 화면
<img width="321" alt="스크린샷 2024-10-22 오전 9 37 04" src="https://github.com/user-attachments/assets/093114fa-b3d3-4243-8a07-b98613928239"><br>
- 게임 모드(SOLO, MULTI) 선택
- 스테이지 선택

### ◽️ 스테이지
- 이전 스테이지 클리어 시, 다음 스테이지 자동 UnLock
- 스테이지 별 다양한 벽돌 배치 구현

### ◽️ 게임 화면
<img width="321" alt="스크린샷 2024-10-22 오전 9 37 34" src="https://github.com/user-attachments/assets/d3205655-9a54-4dce-8fec-03201153a01a">

- 벽돌, 패들, 공으로 구성된 화면
- ESC 입력 시 메뉴 Panel 활성화됨

### ◽️ 충돌 감지
- 공과 벽돌, 공과 패들의 충돌을 감지
- 충돌 시 벽돌 파괴, 공 반사

### ◽️ 아이템 시스템
<img width="216" alt="스크린샷 2024-10-22 오전 9 49 42" src="https://github.com/user-attachments/assets/7e2bd57b-3dd8-4e62-b776-c9a9c7a205cf"><br>
- 벽돌 파괴 시 랜덤으로 아이템 생성
- 스테이지 별 부스팅된 아이템 존재

### ◽️ 보스 스테이지
<img width="246" alt="스크린샷 2024-10-22 오전 9 55 42" src="https://github.com/user-attachments/assets/24215a4d-ad16-471e-9c60-fee1b0e166c3"><br>
- 보스 캐릭터의 체력을 전부 소진시켰을 때 스테이지 Clear

## 🚨 Trouble Shooting
### ◽️ 블록이 다 깨지지 않았는데 게임이 Clear되는 문제(성요셉)
1. 사실 수집
- 디버깅을 통해 블록 카운트를 확인한 결과, 블록이 모두 깨지기 전에 카운트가 0 이하로 내려가는 것을 발견.
- 특히, 공의 개수가 많아질수록 문제가 자주 발생.
2. 원인 추론
- 기존 코드는 블록의 체력이 0 이하가 되면 파괴되도록 구현하였음
    ```C#
    private void SetHealth(int value)
    {
        value = Mathf.Clamp(value, 0, maxHealth);
        health = value;

        if (health > 0)
        {
            image.color = healthColor[health - 1];
            healthTxt.text = health.ToString();
        }
        else
        {
            canvas.gameObject.SetActive(false);
            GetComponent<Animator>().SetTrigger("blockBreak");
            OnBreak?.Invoke(transform.position.x, transform.position.y, dropItem);
            Invoke("DestroyObject", 1f);
        }
    }
    ```
- 문제는 블록 파괴 애니매이션을 1초 지연시키는 동안, 다른 공과 블록이 다시 충돌하면서 SetHealth 함수가 여러 번 호출됨
- 그 결과 하나의 블록에서 여러 번 OnBreak 이벤트가 발생하여 카운트가 잘못 처리됨
3. 조치 방안
- 변수를 추가하여 블록이 이미 파괴된 상태에서는 이벤트가 다시 발생하지 않도록 구현
    ```C#
    else
    {
        if (alreadyDestroyed == false)
        {
            alreadyDestroyed = true;

            canvas.gameObject.SetActive(false);
            if (TryGetComponent<PolygonCollider2D>(out PolygonCollider2D boxCollider)) boxCollider.enabled = false;
            if (TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider)) circleCollider.enabled = false;
            GetComponent<Animator>().SetTrigger("blockBreak");
            OnBreak?.Invoke(transform.position.x, transform.position.y, dropItem);
            Invoke("DestroyObject", 1f);
        }
    }
    ```
- 또한, 파괴된 블록이 이후 게임에 방해되지 않도록 블록의 모든 콜라이더를 비활성화하고 애니메이션만 남겨둠

### ◽️ 공 발사와 화살표 회전(김지훈)
1. 화살표와 공의 위치를 따로 처리
- 기존에는 화살표의 위치와 공의 위치를 따로따로 업데이트하려고 함.
- 그러나, 화살표와 공이 패들 위치와 따로 동작하게 되어 공이 발사되기 전에 화살표와 공의 위치가 일치하지 않는 문제 발생
- 그로 인해 공이 의도하지 않은 위치에서 발사되거나, 발사 각도가 이상해지는 현상 발생
2. 회전 각도 계산을 단순화
- 함수에서 회전 각도를 보다 단순하게 계산해보려고 시도함
- 하지만, 그 결과 화살표가 지나치게 수평으로 회전하거나 각도 제한을 벗어니 공이 의도하지 않은 방향으로 발사되는 문제 발생
- 이는 값이 0~360도 사이로 제한되기 때문에, 음수 각도 처리를 고려하지 않은 단순한 계산이 문제의 원인이었음을 인지함
3. 최종 해결 방법
- 화살표와 공의 동기화 문제<br>
  : 공과 화살표를 따로 관리하지 않고, 동시에 패들 위치에 맞춰 수정한 후 문제가 해결됨. 공이 패들에 정확히 위치하고, 발사 직전까지 화살표의 위치가 일치하게 됨
- 화살표 회전 각도 처리 문제<br>
  : 화살표의 회전 각도 처리 시 음수 각도를 고려하지 않고 단순화했던 게산 방식을 원래 방식으로 되돌림. 이로 인해 화살표가 일정한 각도로 부드럽게 회전하며, 공이 발사될 때 정확한 각도로 발사됨.

### ◽️ Scene창에서는 오브젝트가 보이지만 Game창에서는 보이지 않는 문제(김유민)
1. 문제 인식
- 부스팅된 아이템 목록을 표시하는 기능을 구현하는 과정에서 게임 실행 시 Scene 창에서는 오브젝트가 보이지만 Game 화면에서는 보이지 않는 문제 발생

<img width="900" alt="스크린샷 2024-10-21 오후 3 33 10" src="https://github.com/user-attachments/assets/9dc11988-fe35-4d7d-99a8-ff0541b698b5">

2. 원인 추론
- 기존에는 Inspector창의 Canvas 하위에 빈 오브젝트를 생성 후 SpriteRenderer 컴포넌트를 추가하여 렌더링하려고 함.
- 하지만 Canvas에 UI가 아닌 오브젝트는 위치 조정에 문제가 발생할 수 있음. 
3. 해결 방안
- SpriteRenderer 대신 UI -> Image 오브젝트를 생성함.
- 스크립트에서 해당 컴포넌트를 불러와 sprite 속성을 통해 이미지를 할당하는 방식으로 수정하여 의도한 위치에 랜더링됨.

### ◽️ 공의 충돌 이후 방향 결정 방법(최한결)
1. Physics Material 2D를 이용한 방법
- 공에 마찰력 0, 탄성 계수 1로 설정된 Physics Material 2D를 적용
- 대부분 공이 제대로 튕겼지만, 가끔씩 벽에 부딪힐 때 이상한 각도로 튕기거나, 공의 각도가 0에 가까워져 벽에 붙는 현상 발생
- 해당 문제를 해결하기 위해서는 공의 방향 벡터를 직접 참조해야 함.
2. 충돌 후 Vector2.Reflect 함수를 사용하는 방법
- BallController의 OnCollision2D에 아래 코드를 추가하여 충돌 후 공의 방향을 계산
    ```C#
    Vector2 normal = collision.contacts[0].normal;
    Vector2 newDirection = Vector2.Reflect(rb.velocity, normal);
    rb.velocity = newDirection;
    ```
- 해당 방법은 공이 튕기는 방향이 일정하지 않고, 각도가 계속 변동하는 문제가 발생하여 폐기함.
3. 입사각과 반사각을 계산한 방법
- 입사각과 반사각을 계산하여 공의 방향 벡터를 회전시키는 방법 사용
- 2번째 방법과 거의 동일한 움직임을 보였지만, 원형 장애물과 충돌 시 입사각이 90도를 넘는 경우 공이 장애물의 외곽을 따라 움직이는 문제 발생
4. 최종 해결 방법
- 1번과 3번을 결합하여 사용(Physics Material 2D를 이용한 방법 + 입사각과 반사각을 계산한 방법)
- 기본적으로 특성을 따르되, 공이 벽에 부딪힐 때 각도가 수평에 너무 가까울 경우 방향 벡터를 회전시켜 공이 너무 느려지지 않도록 조정함.

## 📄 팀 Notion
[📌 팀 Notion 페이지](https://www.notion.so/11891d7022398079923edb24979c70fe?pvs=4)

## 🎥 Demo 영상
[📌 Demo 영상 링크](https://www.notion.so/11891d7022398079923edb24979c70fe?pvs=4)

## 📜 Asset Reference
<U>[Dynamic Space BackGround Lite](https://assetstore.unity.com/packages/2d/textures-materials/dynamic-space-background-lite-104606)<br>
[Dotted Arrow](https://assetstore.unity.com/packages/tools/gui/dotted-arrow-213121)<br>
[Puzzle Pack](https://kenney.nl/assets/puzzle-pack)<br></U>