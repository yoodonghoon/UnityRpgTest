using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    private float moveSpeed = 7f;
    public TextMeshPro text;
    public int damage;
    public IObjectPool<DamageText> pool;

    public void Init(string damage)
    {
        text.text = damage;

        transform.DOLocalMoveY(3, 1).OnComplete(() =>
        {
            Destory();
        });
    }

    public void SetPool(IObjectPool<DamageText> _pool)
    {
        pool = _pool;
    }

    public void Destory()
    {
        pool.Release(this);
    }
}
