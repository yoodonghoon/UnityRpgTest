using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;

public class GameManager : SingletonCommon<GameManager>
{
    public PlayerController Player { get; set; }
    public IObjectPool<EnemyControlller_SM> EnemyPools { get; set; }
    public IObjectPool<DamageText> DamageTextPools { get; set; }
    public GameObject MonsterPrefab;
    public GameObject DamageTextPrefab;


    public void Start()
    {
        TableLoad();
        EnemyPools = new ObjectPool<EnemyControlller_SM>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize:10);
        DamageTextPools = new ObjectPool<DamageText>(CreateDamageText, OnGetDamageText, OnReleaseDamageText, OnDestroyDamageText, maxSize: 10);

        SpawnMonster().Forget();
    }

    private void TableLoad()
    {
        ItemTable.Instance.LoadCsv();
    }

#region Monster
    private async UniTask SpawnMonster()
    {
        while(true)
        {
            var enemy = EnemyPools.Get();
            enemy.transform.position = new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10));
            await UniTask.WaitForSeconds(5);
        }
    }

    private EnemyControlller_SM CreateMonster()
    {
        EnemyControlller_SM monster = Instantiate(MonsterPrefab).GetComponent<EnemyControlller_SM>();
        monster.SetPool(EnemyPools);
        return monster;
    }

    private void OnGetMonster(EnemyControlller_SM monster)
    {
        monster.Init();
        monster.gameObject.SetActive(true);
    }

    private void OnReleaseMonster(EnemyControlller_SM monster)
    {
        monster.gameObject.SetActive(false);
    }

    private void OnDestroyMonster(EnemyControlller_SM monster)
    {
        Destroy(monster.gameObject);
    }

    #endregion Monster

#region DamageText
    public void SpawnDamageText(Transform pos, string damage)
    {
        var damageText = DamageTextPools.Get();
        damageText.transform.position = pos.position;
        damageText.Init(damage);
    }

    private DamageText CreateDamageText()
    {
        DamageText text = Instantiate(DamageTextPrefab).GetComponent<DamageText>();
        text.SetPool(DamageTextPools);
        return text;
    }

    private void OnGetDamageText(DamageText damageText)
    {
        damageText.gameObject.SetActive(true);
    }

    private void OnReleaseDamageText(DamageText damageText)
    {
        damageText.gameObject.SetActive(false);
    }

    private void OnDestroyDamageText(DamageText damageText)
    {
        Destroy(damageText.gameObject);
    }

#endregion DamageText
}