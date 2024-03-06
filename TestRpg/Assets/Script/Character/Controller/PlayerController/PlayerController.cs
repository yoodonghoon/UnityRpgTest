using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController)), RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour, IAttackable, IDamagable
{
    private CharacterController controller;
    [SerializeField]
    private LayerMask targetMask;

    //private NavMeshAgent agent;
    private Camera camera;

    [SerializeField]
    private Animator animator;

    readonly int moveHash = Animator.StringToHash("Move");
    readonly int attackTriggerHash = Animator.StringToHash("Attack");
    readonly int attackIndexHash = Animator.StringToHash("AttackIndex");
    
    private bool isAttackState => GetComponent<AttackStateController>()?.IsInAttack ?? false;

    public int helth = 100;
    public bool IsAlive => helth > 0;
    public Transform target;
    public List<AttackBehaviour> attackBehaviourList = new();
    public GameObject damageEffectPrefab;
    public Transform hitPoint;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        camera = Camera.main;

        InitAttackBehaviour();
        CheckAttackBehaviour().Forget();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            //// Check hit from ray
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit, 100))
            //{
            //    Debug.Log("We hit " + hit.collider.name + " " + hit.point);

            //    IDamagable damagable = hit.collider.GetComponent<IDamagable>();
            //    if (damagable != null && damagable.IsAlive)
            //    {
            //        target = hit.collider.transform;
            //    }
            //}
            AttackTarget();
        }
        else if(isAttackState == false)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 moveDir = new Vector3(h, 0, v);

            if (moveDir == Vector3.zero)
            {
                controller.Move(Vector3.zero);
                animator.SetBool(moveHash, false);
                return;
            }

            moveDir = camera.transform.TransformDirection(moveDir);

            Vector3 playerLook = new Vector3(moveDir.x, 0, moveDir.z);
            transform.forward = playerLook;

            moveDir *= 5;
            moveDir.y = 0;

            controller.Move(moveDir * Time.deltaTime);
            animator.SetBool(moveHash, true);
        }
    }

    void AttackTarget()
    {
        if (CurrentAttackBehaviour == null)
        {
            return;
        }

        if (!isAttackState && CurrentAttackBehaviour.IsAvailable)
        {
            animator.SetInteger(attackIndexHash, Random.Range(0,2));
            animator.SetTrigger(attackTriggerHash);
        }
    }

    private void InitAttackBehaviour()
    {
        foreach (AttackBehaviour behaviour in attackBehaviourList)
        {
            behaviour.targetMask = targetMask;
        }
    }

    async UniTask CheckAttackBehaviour()
    {
        if (CurrentAttackBehaviour == null || !CurrentAttackBehaviour.IsAvailable)
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

    public void TakeDamage(int damage, GameObject hitEffectPrefab)
    {
        if (IsAlive == false)
            return;

        helth -= damage;

        if (damageEffectPrefab)
            Instantiate<GameObject>(damageEffectPrefab, hitPoint);
    }

    public void OnExecuteAttack(int attackIndex)
    {
        //if(target)
            CurrentAttackBehaviour?.ExecuteAttack();
    }

    public AttackBehaviour CurrentAttackBehaviour
    {
        get;
        private set;
    }
}