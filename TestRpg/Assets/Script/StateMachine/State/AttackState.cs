using UnityEngine;

public class AttackState : State<EnemyController>
{
    private Animator animator;
    private AttackStateController attackStateController;
    private IAttackable attackable;

    protected int hashAttack = Animator.StringToHash("Attack");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        attackStateController = context.GetComponent<AttackStateController>();
        attackable = context.GetComponent<IAttackable>();
    }

    public override void OnEnter()
    {
        if (attackable == null || attackable.CurrentAttackBehaviour == null)
        {
            stateMachine.ChangeState<IdleState>();
            return;
        }


        attackStateController.enterAttackHandler += OnEnterAttackState;
        attackStateController.exitAttackHandler += OnExitAttackState;

        animator?.SetTrigger(hashAttack);
    }

    public override void Update(float deltaTime)
    {
    }

    public override void OnExit()
    {
        attackStateController.enterAttackHandler -= OnEnterAttackState;
        attackStateController.exitAttackHandler -= OnExitAttackState;
    }

    public void OnEnterAttackState()
    {
        Debug.Log("OnEnterAttackState");
    }

    public void OnExitAttackState()
    {
        Debug.Log("OnExitAttackState");
        stateMachine.ChangeState<IdleState>();
    }
}
