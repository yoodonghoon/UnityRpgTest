using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;

public class GameManager : SingletonCommon<GameManager>
{
    public PlayerController Player { get; set; }
    public IObjectPool<EnemyControlller_SM> EnemyPools { get; set; }
    public GameObject MonsterPrefab;


    public void Start()
    {
        TableLoad();
        EnemyPools = new ObjectPool<EnemyControlller_SM>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize:10);

        SpawnMonster().Forget();
    }

    private void TableLoad()
    {
        ItemTable.Instance.LoadCsv();
    }

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
}