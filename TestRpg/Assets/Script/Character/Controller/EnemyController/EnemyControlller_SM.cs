using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyControlller_SM : EnemyController, IAttackable, IDamagable
{
    protected StateMachine<EnemyController> stateMachine;

    public LayerMask targetMask;
    public Collider weaponCollider;

    public GameObject hitEffect;
    public GameObject damageEffectPrefab;
    public Transform hitPoint;
    public int helth = 3;
    public bool IsAlive => helth > 0;

    public int isHitHash = Animator.StringToHash("IsHit");

    public IObjectPool<EnemyControlller_SM> pool;
    public CharacterController CharacterControllerCompoenet;

    protected override void Start()
    {
        base.Start();

        stateMachine = new StateMachine<EnemyController>(this, new IdleState());
        stateMachine.AddState(new MoveState());
        stateMachine.AddState(new AttackState());
        stateMachine.AddState(new DeadState());

        InitAttackBehaviour();
        CheckAttackBehaviour().Forget();
    }

    void Update()
    {
        stateMachine.Update(Time.deltaTime);

        if (!(stateMachine.currentState is MoveState))
        {
            FaceTarget();
        }
    }

    public void Init()
    {
        CharacterControllerCompoenet.enabled = true;
        helth = 3;
        stateMachine?.ChangeState<IdleState>();
    }

    public R ChangeState<R>() where R : State<EnemyController>
    {
        return stateMachine.ChangeState<R>();
    }

    public override bool IsAvailableAttack
    {
        get
        {
            if (!Target)
                return false;

            float distance = Vector3.Distance(transform.position, Target.position);
            return (distance <= CalcAttackRange);
        }
    }

    override public Transform SearchEnemy()
    {
        if (target == null)
        {
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
            if (targetsInViewRadius.Length > 0)
            {
                target = targetsInViewRadius[0].transform;
            }
        }

        return target;
    }

    public void FaceTarget()
    {
        if (!Target || IsAlive == false)
            return;

        Vector3 direction = (Target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void InitAttackBehaviour()
    {
        foreach (var attack in attackBehaviourList)
        {
            if(CurrentAttackBehaviour == null)
            {
                CurrentAttackBehaviour = attack;
                CurrentAttackBehaviour.targetMask = targetMask;
                break;
            }
        }
    }

    public async UniTask CheckAttackBehaviour()
    {
        if (CurrentAttackBehaviour == null || CurrentAttackBehaviour.IsAvailable == false)
        {
            CurrentAttackBehaviour = null;

            foreach (AttackBehaviour behaviour in attackBehaviourList)
            {
                if (behaviour.IsAvailable)
                {
                    if ((CurrentAttackBehaviour == null) || (CurrentAttackBehaviour.priority < behaviour.priority))
                    {
                        CurrentAttackBehaviour = behaviour;
                    }
                }
            }
        }

        await UniTask.WaitForSeconds(0.1f);
    }

    private void OnAnimatorMove()
    {
        Vector3 position = agent.nextPosition;
        animator.rootPosition = agent.nextPosition;
        transform.position = position;
    }

    public void TakeDamage(int damage, GameObject hitEffectPrefab)
    {
        if (IsAlive == false)
            return;

        helth -= damage;
        if (damageEffectPrefab)
            Instantiate<GameObject>(damageEffectPrefab, hitPoint);

        if (IsAlive)
            animator.SetTrigger(isHitHash);
        else
        {
            stateMachine.ChangeState<DeadState>();
            CharacterControllerCompoenet.enabled = false;
            
            InventoryManger.Instance.AddItem(Random.Range(0,10));
            Invoke("Destory", 5);
        }
    }

    public void OnExecuteAttack(int attackIndex)
    {
        CurrentAttackBehaviour?.ExecuteAttack(target.gameObject);
    }

    public List<AttackBehaviour> attackBehaviourList = new();
    public AttackBehaviour CurrentAttackBehaviour
    {
        get;
        private set;
    }

    public void SetPool(IObjectPool<EnemyControlller_SM> _pool)
    {
        pool = _pool;
    }

    public void Destory()
    {
        pool.Release(this);
    }
}
