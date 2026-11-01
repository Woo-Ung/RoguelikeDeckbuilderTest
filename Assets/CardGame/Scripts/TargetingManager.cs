
using UnityEngine;

// 대상 지정이 필요한 카드의 적 선택 상태를 관리
public class TargetingManager : MonoBehaviour
{
    [SerializeField] private BattleManager _battleManager;

    // 적 선택을 기다리는 카드
    private CardView _pendingCard;

    private void OnEnable()
    {
        foreach (Enemy enemy in _battleManager.Enemies)
        {
            enemy.OnClicked += HandleEnemyClicked;
        }
    }

    private void OnDisable()
    {
        foreach (Enemy enemy in _battleManager.Enemies)
        {
            enemy.OnClicked -= HandleEnemyClicked;
        }
    }

    // 같은 카드를 다시 누르면 취소, 다른 카드를 누르면 대상 지정 카드 전환
    public void ToggleTargeting(CardView card)
    {
        if (_pendingCard == card)
        {
            CancelTargeting();
            return;
        }

        if (_pendingCard != null)
            _pendingCard.SetTargeting(false);

        _pendingCard = card;
        _pendingCard.SetTargeting(true);
    }

    // 파괴되는 카드가 대기 중이었다면 표시 변경 없이 대기 상태만 해제
    public void ReleaseCard(CardView card)
    {
        if (_pendingCard == card)
            _pendingCard = null;
    }

    private void CancelTargeting()
    {
        if (_pendingCard == null)
            return;

        _pendingCard.SetTargeting(false);
        _pendingCard = null;
    }

    private void HandleEnemyClicked(Enemy enemy)
    {
        if (_pendingCard == null)
            return;

        CardView card = _pendingCard;

        CancelTargeting();

        card.PlayOn(enemy);
    }
}
