using UnityEngine;

public class AttackStateController : MonoBehaviour
{
    public delegate void OnEnterAttackState();
    public OnEnterAttackState enterAttackHandler;

    public delegate void OnExitAttackState();
    public OnExitAttackState exitAttackHandler;

    public bool IsInAttack { get; private set; }
    private IAttackable Attackable;

    public void Start()
    {
        Attackable = GetComponent<IAttackable>();
    }

    public void OnStartOfAttackState()
    {
        IsInAttack = true;
        if(enterAttackHandler != null)
            enterAttackHandler();
    }
    public void OnEndOfAttackState()
    {
        IsInAttack = false;
        if (exitAttackHandler != null)
            exitAttackHandler();
    }

    public void OnCheckAttackCollider(int attackIndex)
    {
        Attackable?.OnExecuteAttack(attackIndex);
    }
}
