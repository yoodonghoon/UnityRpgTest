using UnityEngine;

public abstract class AttackBehaviour : MonoBehaviour
{
    public int animationIndex;
    public int priority;
    public int damage;
    public float range = 3f;

    [SerializeField]
    private float coolTime;
    protected float calcCoolTime = 0.0f;

    public GameObject effectPrefab;

    [HideInInspector]
    public LayerMask targetMask;

    [SerializeField]
    public bool IsAvailable => calcCoolTime >= coolTime;

    protected virtual void Start()
    {
        calcCoolTime = coolTime;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (calcCoolTime < coolTime)
        {
            calcCoolTime += Time.deltaTime;
        }
    }

    public abstract void ExecuteAttack(GameObject target = null, Transform startPoint = null);
}