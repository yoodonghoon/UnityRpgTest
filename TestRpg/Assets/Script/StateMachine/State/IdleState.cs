using UnityEngine;

public class IdleState : State<EnemyController>
{
    private Animator animator;
    private CharacterController controller;

    protected int hashMove = Animator.StringToHash("Move");
    protected int hashMoveSpeed = Animator.StringToHash("MoveSpeed");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        controller = context.GetComponent<CharacterController>();
    }

    public override void OnEnter()
    {
        animator?.SetBool(hashMove, false);
        animator.SetFloat(hashMoveSpeed, 0);
        controller?.Move(Vector3.zero);
    }

    public override void Update(float deltaTime)
    {
        Transform enemy = context.SearchEnemy();
        if (enemy)
        {
            if (context.IsAvailableAttack)
            {
                stateMachine.ChangeState<AttackState>();
            }
            else
            {
                stateMachine.ChangeState<MoveState>();
            }
        }
    }
}
