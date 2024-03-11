using UnityEngine;

public class DeadState : State<EnemyController>
{
    int isDeath = Animator.StringToHash("Death");

    public override void OnEnter()
    {
        context.GetComponent<Animator>()?.SetTrigger(isDeath);
    }

    public override void Update(float deltaTime)
    {
    }
}