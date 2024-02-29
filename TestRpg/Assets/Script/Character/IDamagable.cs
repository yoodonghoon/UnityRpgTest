using UnityEngine;

public interface IDamagable
{
    bool IsAlive
    {
        get;
    }

    void TakeDamage(int damage, GameObject hitEffect);
}
