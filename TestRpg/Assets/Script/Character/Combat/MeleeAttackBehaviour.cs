using UnityEngine;

public class MeleeAttackBehaviour : AttackBehaviour
{
    public AttackCollision attackCollision;

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Collider[] colliders = attackCollision?.CheckOverlapBox(targetMask);

        foreach (Collider col in colliders)
        {
            col.gameObject.GetComponent<IDamagable>()?.TakeDamage(damage, effectPrefab);
        }

        calcCoolTime = 0.0f;
    }
}
