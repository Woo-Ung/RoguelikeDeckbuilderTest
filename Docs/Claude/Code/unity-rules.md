# Unity Development Rules

## General Rules
- Inspector 연결이 필요한 private 필드는 `[SerializeField]` 사용
- 불필요한 public 필드 노출 지양 (필요 시 `get; private set;` 프로퍼티 활용)
- Unity 생명주기 메서드의 실행 순서 및 의존성 고려
- `Update()` / `FixedUpdate()` / `LateUpdate()` 내부 제약:
  - 반복적인 컴포넌트 검색(`GetComponent`) 지양 및 참조 캐싱 우선
  - 반복 호출되는 문자열 기반 API는 해시값 캐싱 우선 (예: `Animator.StringToHash()`, `Shader.PropertyToID()`)
  - 매 프레임 반복되는 불필요한 관리 힙 할당 및 오브젝트 생성 지양
  - 빈번한 생성 및 제거가 필요한 게임 오브젝트는 오브젝트 풀링 검토
- 씬과 프리팹의 기존 참조 관계 유지
- 에셋 및 `.meta` 파일의 임의 삭제 또는 변경 금지