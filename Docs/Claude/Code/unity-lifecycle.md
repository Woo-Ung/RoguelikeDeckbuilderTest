# Lifecycle & Initialization

## Lifecycle Methods
- Unity 생명주기 메서드의 간결성 유지
- 컴포넌트 참조, 초기화, 이벤트 등록 및 해제 로직은 역할별 메서드로 분리
- 생명주기 메서드에서는 역할별 메서드를 호출하는 구조 우선
- 단일 메서드 호출 시 표현식 본문(`=>`) 사용 권장
- 불필요한 생명주기 메서드 및 역할별 메서드 생성 지양

## Method Responsibilities
- `Awake()` → `CacheComponents()`: 컴포넌트 참조 획득 및 캐싱
- `OnEnable()` → `BindEvents()`: 활성화 상태에 종속된 이벤트 등록
- `Start()` → `Init()`: 초기 변수 및 상태 설정
- `OnDisable()` → `UnbindEvents()`: 활성화 상태에 종속된 이벤트 해제
- 초기화 및 이벤트 관리 시점은 컴포넌트의 의존성과 생명주기에 따라 조정

## Implementation Rules
- `OnEnable()`에서 다른 객체의 `Start()` 초기화 완료를 전제로 하는 로직 지양
- 이벤트 등록 전 필요한 참조의 유효성 확인
- 컴포넌트 활성화 상태에 종속된 이벤트는 `OnEnable()` 등록 / `OnDisable()` 해제를 기본으로 사용
- 객체 생존 기간 동안 유지해야 하는 이벤트는 적절한 등록 및 해제 시점 별도 설정
- 이벤트 등록과 해제의 생명주기 일치 및 중복 등록 방지
- Input System의 입력 액션 연결 시 `BindInputActions()` / `UnbindInputActions()` 사용
- 기존 프로젝트의 별도 초기화 구조가 있는 경우 해당 구조 우선