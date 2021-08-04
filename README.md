# 실행 화면





# WEEK4_Unity

## Ch.5 Prefab & Conflict Judgment

### CatEscape

#### 5.1 게임 설계하기

##### 5.1.1 게임 기획

- 화살 피하기 게임



##### 5.1.2 게임 리소스 생각하기

1. 화면에 놓일 오브젝트
   - 플레이어, 화살, 배경, 이동버튼, HP
2. 컨트롤러 스크립트
   - 움직이는 플레이어
   -  떨어지는 화살
3. 제너레이터 스크립트
   - 플레이할 때 생성되는 화살
4. 감독 스크립트
   - HP 게이지
5. 스크립트 흐름
   - 컨트롤러 - 제너레이터 - 감독





#### 5.2 프로젝트&씬 만들기

##### 5.2.1 프로젝트 만들기

- File -> New Project -> (CatEscape) -> 2D -> 생성
- 리소스 추가
- 화면 표시 설정 -> Game 탭 -> VSync 체크



##### 5.2.2 스마트폰용

- File -> Build Settings -> Platform -> Switch Platform
- 화면 크기 설정 -> Game 탭의 화면 크기에 맞는 크기 선택



##### 5.2.3 씬 저장하기

- File -> Save As -> (GameScene)





#### 5.3 씬에 오브젝트 배치하기

##### 5.3.1 플레이어 배치

- Position (0, -3.6, 0)



##### 5.3.2 배경 이미지 넣기

- Position (0, 0, 0)
- Scale (4.5, 2, 1)
- 레이어 설정 ... 플레이어가 배경 이미지 뒤에 놓이면 보이지 않는다
  - 각 오브젝트는 레이어 번호를 가짐 - 클수록 앞에 표시
  - Player의 Order in Layer를 1로 바꾸기





#### 5.4 키를 조작해 플레이어 움직이기

##### 5.4.1 플레이어 스크립트 작성

- 화면에 있는 버튼을 누르면 플레이어가 움직이도록
- Create -> C# Script -> (PlayerController)

```c#
// (PlayerController)
public class PlayerController : MonoBehaviour
{
	void Start()
	{
        
    }
    
	void Update()
	{
 		if (Input.GetKeyDown(KeyCode.LeftArrow))  // 왼쪽 화살표 누를 때
 		{
            transform.Translate(-3, 0, 0);  // 왼족으로 3 만큼 움직이기
        }
 		if (Input.GetKeyDown(KeyCode.RightArrow))  // 오른쪽 화살표 누를 때
 		{
            transform.Translate(3, 0, 0);  // 오른쪽으로 3 만큼 움직이기
        }
	}
}
```

> - 키가 눌렸는지 검출 -> Input.GetKeyDown 매서드
>   - 누른 순간 - Input.GetKeyDown
>   - 누르고 있는 동안 - Input.GetKey
>   - 뗀 순간 - Input.GetKeyUp



##### 5.4.2 플레이어에 스크립트 적용

- (PlayerController)를 player 오브젝트로 드래그&드롭





#### 5.5 Physics를 사용하지 않고 화살 떨어뜨리기

##### 5.5.1 화살 떨어뜨리기

- Physics를 사용하면 중력을 알아서 계산해주지만 직접적인 움직임 처리 어렵다



##### 5.5.2 화살 배치

- Position (0, 3.2, 0)
- Order in Layer (1)



##### 5.3.3 화살 스크립트 작성

```c#
// (ArrowController)
public class ArrowController : MonoBehaviour
{
	void Start()
    {
        
    }

    void Update ()
	{
 		// 프레임마다 등속으로 낙하
 		transform.Translate(0, -0.1f, 0);
 		// 화면 밖으로 나오면 오브젝트 소멸
 		if (transform.position.y < -5.0f)
 		{
            Destroy(gameObject);
        }
	}
}
```

> - 화면 밖으로 나온 화살 소멸시키기
>   - 화살이 보이지 않아도 컴퓨터는 계속 계산해야하므로 메모리 낭비
>   - Destroy 매서드로 오브젝트를 소멸 ... (gameObject)는 자기 자신



##### 5.5.4 화살에 스크립트 적용

- (ArrowController)를 (arrow) 오브젝트로 드래그&드롭
  - Hierarchy 창에서 arrow가 사라지는지 확인





#### 5.6 충돌 판정하기

##### 5.6.1 충돌 판정

- 오브젝트끼리 충돌하는지 감시하고(충돌 판정), 충돌하면 특정 처리(충돌 반응)



##### 5.6.2 간단한 충돌 판정

- 오브젝트의 형상을 단순히 원형이라고 가정
  - 화살을 둘러싼 원의 반지름 r1, 중심 좌표 p1
  - 고양이를 둘러싼 원의 반지름 r2, 중심 좌표 p2
    - 거리 d = sqrt((p1.x-p2.x)^2+(p1.y-p2.y)^2)
      - d <= r1 + r2 일 때 충돌



##### 5.6.3 충돌 판정 스크립트 작성

```c#
// (ArrowController)
public class ArrowController : MonoBehaviour
{
	GameObject player;
	void Start()
	{
        this.player = GameObject.Find("player");
    }
    
	void Update()
	{
 		...
 		// 충돌 판정
 		Vector2 p1 = transform.position;  // 화살의 중심 좌표
 		Vector2 p2 = this.transform.position;  // 플레이어의 중심 좌표
 		Vector2 dir = p1 - p2;
 		
        float d = dir.magnitude;
 		float r1 = 0.5f;  // 화살의 반경
 		float r2 = 1.0f;  // 플레이어의 반경
 		
        if (d < r1 + r2)
 		{
  			// 충돌한 경우는 화살을 지운다
  			Destroy(gameObject);
 		}
	}
}
```





#### 5.7 프리팹과 공장 만들기

##### 5.7.1 공장의 구성

- 화살 오브젝트를 1초에 한 개씩 만드는 공장(화살 제너레이터) 만들기
  - 양산 기계(제너레이터 스크립트)가 설계도(프리팹)을 통해 제품(인스턴스)를 생산하는 구조
    

##### 5.7.2 프리팹

- 같은 오브젝트를 많이 만들고 싶을 때 주로 프리팹을 사용



##### 5.7.3 프리팹 장점

- 프리팹을 쓰면 변경 사항이 있을 때 프리팹 파일만 수정하면 되므로 편하게 수정



##### 5.7.4 프리팹 만들기

- 설계도(프리팹)로 쓰고 싶은 오브젝트를 Project창으로 드래그&드롭

- Hierarchy창의 (arrow)를 Project창으로 -> (arrowPrefab)

  - Prefab이 있으면 씬에 배치한 오브젝트는 필요없으니 Delete

    

##### 5.7.5 제너레이터 스크립트

```c#
// (ArrowGenerator)
public class ArrowGenerator : MonoBehaviour
{
	public GameObject arrowPrefab;  // 화살 설계도를 넣는 변수 선언
	float span = 1.0f;  // 화살이 만들어질 시간 변수 1로 설정
	float delta = 0;  // 프레임과 프레임 사이의 시간 차이 변수

    void Update()
    {
 		this.delta += Time.deltaTime;  // 프레임이 지날때마다 프레임 사이 시간 추가
 		if (this.delta > this.span)  // 1초가 지나면
 		{
  			this.delta = 0;  // 다시 1초 세도록
  			// 화살 인스턴트 생성
  			GameObject go = Instantiate(arrowPrefab) as GameObject;
  			int px = Random.Range(-6, 7);  // (-6 <= x < 7) 사이의 랜덤으로 설정
  			go.transform.position = new Vector3(px, 7, 0);  // (px, 7, 0)에 생성
		}
    }
}

// - Instantiate 매서드 : 매개변수로 프리팹을 전달하면 반환값으로 인스턴트 돌려줌
// - Random.Range 매서드 : (-x, y) 사이 범위에서 무작위 수를 정수로 반환
```



##### 5.7.6 빈 오브젝트에 제너레이터 스크립트 적용하기

- 빈 오브젝트에 제너레이터 스크립트를 적용해야 공장 오브젝트가 됨.
- Hierarchy 창 -> '+' -> Create Empty -> (ArrowGenerator)
- (ArrowGenerator 스크립트) -> (ArrowGenerator 오브젝트)로 드래그&드롭



##### 5.7.7 제너레이터 스트립트에 프리팹 전달하기

- 스크립트 내의 변수에 오브젝트 실체를 대입
  - 아웃렛 접속
    - 스크립트 쪽에 플러그를 꽂을 수 있는 콘센트 구멍, Inspector 창에서 해당하는 플러그를 만들고 스크립트의 콘센트 구멍에 끼워 오브젝트를 대입
      1. 스크립트 쪽에 콘센트 구멍 -> public 접근 수식자를 붙힌다
      2. public 접근 수식자를 분인 변수가 Inspector 창에 보인다
      3. Inspector 창의 콘센트 구멍에 대입할 오브젝트를 끼운다 (드래그&드롭)

- 콘센트 구멍 만들기
  - public GameObject arrowPrefab;

- Inspector 창에 오브젝트 끼우기
  - Hierarchy창에서 (ArrowGenerator) -> arrowPrefab 변수 찾기 (콘센트 구멍)
    - Project 창의 (arrowPrefab)을 (Arrow Prefab) 항목으로 드래그&드롭 -> 프리팹 설정 -> (arrowPrefab) 변수에 프리팹 실체가 대입됨





#### 5.8 UI 표시하기

##### 5.8.1 UI를 표시하고 갱신하는 감독 만들기

1. UI 부품을 Scene 뷰에 배치한다.
2. UI를 갱신하는 감독 스크립트 작성.
3. 빈 오브젝트를 만들고 작성한 스크립트 적용.



##### 5.8.2 HP 게이지 배치

- (Image)를 사용한 HP 게이지 만들기
- Hierarchy 창에서 + -> UI -> Image -> (Image)를 (hpGauge)로 바꾸기
- (hp_gauge)를 Source Image로 드래그&드롭
  

- 앵커 포인터 설정
- 화면 크기가 바뀌어도 HP 게이지가 항상 표시되도록 앵커 포인터 변경
  - 화면 크기가 바뀔 때 어디를 원점으로 UI 부품 좌표를 다시 계산하는가?
  - (hpGauge)의 Anchor Presets에서 오른쪽 위에 고정 -> Pos XYZ (-120, -120, 0) -> Width, Height (200)



- HP 게이지 줄여 나가기
  - Image에서 제공하는 Fill 기능 -> Fill Amount를 가변
  - (hpGauge)에서 Image Type을 Filled, Filled Method를 (Radial 360)으로 설정
  - Fill Origin을 (Top), Fill Amount 초기값 (1)





#### 5.9 UI를 갱신하는 감독 만들기

##### 5.9.1 UI를 갱신하는 흐름 생각하기

- 감독 스크립트는 플레이어가 화살에 맞으면 이를 감지해 HP 게이지의 표시를 갱신
  1. 화살 컨트롤러는 감독에게 HP 감소되었다고 알린다.
  2. 감독은 HP 게이지의 UI를 갱신



#### 5.9.2 UI를 갱신하는 감독 만들기

- 감독 스크립트 작성 -> 빈 오브젝트 만들기 -> 빈 오브젝트에 감독 스크립트 적용

- 감독 스크립트 작성

  ```c#
  // Create -> C# Script -> (GameDirector)
  using UnityEngine.UI;  // UI 사용시 무조건 필요
  public class GameDirector : MonoBehaviour
  {
  	GameObejct hpGauge;
  	void Start()
  	{
          this.hpGauge = GameObject.Find("hpGauge");
      }
  	public void DecreaseHP()  // 화살컨트롤러에서 HP 줄이는 처리를 호출할 것
  	{
          this.hpGauge.GetComponent<Image>().fillAmount -= 0.1f;
      }
  }
  ```

  

- 빈 오브젝트 만들기
  - Hierarchy -> + -> Create Empty -> (GameDirector)



- 빈 오브젝트에 감독 스크립트 적용
  - (GameDirector) 스크립트를 (GameDirector) 오브젝트로 드래그&드롭



##### 5.9.3 HP 줄었다고 감독에게 알리기

- 화살에 맞으면 화살 컨트롤러에서 감독 스크립트의 DecreaseHp 매서드를 호출

  ```c#
  // (ArrowController)에 추가
  if (d < r1 + r2)
  {
   	// 감독 스크립트에 화살에 충돌했다고 전달
   	GameObject director = GameObject.Find("GameDirector");// 오브젝트 찾기
   	director.GetComponent<GameDirector>().DecreaseHp();
  }  // GameDirector 오브젝트의 GameDirector 스크립트의 DecreaHp 호출
  // -> 자신 이외의 오브젝트 컴포넌트에 접근하려면 Find와 GetComponent를 조합하여 사용해야한다.
  ```

  

  1. Find 매서드로 오브젝트 찾기
  2. GetComponent 매서드로 오브젝트의 컴포넌트를 구한다.
  3. 컴포넌트를 가진 데이터에 접근





#### 5.10 스마트폰에서 움직여보기

##### 5.10.1 컴퓨터와 스마트폰 차이

- 스마트폰에서 조작할 수 있도록 화면에 좌우 버튼을 배치
  1. UI를 사용해 오른쪽 버튼 배치 & 설정
  2. 오른쪽 버튼을 복제하여 왼쪽 버튼 만들기
  3. 버튼에 반응해 플레이어가 움직이도록 스크립트 수정



##### 5.10.2 오른쪽 버튼 만들기

- Hierarchy -> + -> UI -> Button -> (RButton)
- 앵커포인터 (우측하단), POS(-200, 200, 0), Width,Height(250)
- (RButton) 이미지를 Source Image로 드래그&드롭



- 버튼 레이블
  - 버튼 이미지 위에는 레이블이 Button으로 표시, Text라는 자식요소 제거



##### 5.10.3 오른쪽 버튼 복제해 왼쪽 버튼 만들기

- (RButton)을 Duplicate -> (LButton)
- 앵커포인터(좌측하단), POS(200, 200, 0), Source Image 업데이트



##### 5.10.4 버튼을 눌렀을 때 플레이어 이동시키기

1. 플레이어 이동시키는 매서드 만들기
2. 각 버튼에 해당하는 매서드 만들기



- 플레이어를 좌우로 이동시키는 매서드 작성

  ```c#
  // (PlayerController)
  public class PlayerController : MonoBehaviour
  {
  	public void LButtonDown()
  	{
          transform.Translate(-3, 0, 0);
      }
  	public void RButtonDown()
  	{
          transform.Translate(3, 0, 0);
      }
  }
  ```

  

- 버튼을 눌렀을 때의 매서드 지정
  - Hierarchy -> RButton -> OnClick()의 + -> Hierarchy의 player를 None(Object)로 드래그&드롭



> - Debug.Log
>   - 해당 변수를 Debug.Log로 출력하여 확인
>     - ex) Debug.Log(transform.position) 

