using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;

public class BossEnemySpawner : EnemySpawner
{

    // Start is called before the first frame update
    //PlayerMovement pm;
    public GameObject boss = null;
    BossBehavior bb;

    [SerializeField]
    BoxCollider2D bossTrigger;

    [SerializeField] List<GameObject> bossWalls;

    bool isMinionSpawned = false;
    void Start()
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        

        //pm = player.gameObject.GetComponent<PlayerMovement>();
    }

    protected override void OnEnable() {
        BossBehavior.onSpawnMinion += spawnMinion;
        base.OnEnable();
    }
    protected override void OnDisable() {
        BossBehavior.onSpawnMinion -= spawnMinion;
        base.OnDisable();
    }

    public override void chunkReached(float currentX, int chunkNumber) {
        inChunk = true;
        bossTrigger.enabled = false;
        // 1. pause camera movement
        var composer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        composer.m_DeadZoneWidth = 2f;
        // 2. move walls (currently in bgObjects)
        bossWalls[0].transform.position = new Vector3(vcam.transform.position.x-12.5f,0,0);
        bossWalls[1].transform.position = new Vector3(vcam.transform.position.x+12.5f,0,0);

        
        // 3. Spawn Boss 
        boss = Instantiate(boss, new Vector3(player.transform.position.x+15f,0,0), Quaternion.identity);
        bb = boss.GetComponent<BossBehavior>();

        // 4. When enemies are killed go back to normal movement!
    } 

    private void spawnMinion() {
        if (!isMinionSpawned) {
            enemyChunkSpawner(1000, 1, 1, 1);
            CheckChunkCleared(2000);
        }
        isMinionSpawned = true;
    }

    protected override async void CheckChunkCleared(int delay_ms)
    {
        bool done = false;

        while(!done && !_stopSpawn)
        {
            await Task.Delay(delay_ms);
            //EnemyBase[] enemies = GameObject.FindObjectsOfType<EnemyBase>();
            //Transform[] enemies = enemyContainer.GetComponentsInChildren<Transform>();
            if (enemyContainer.transform.childCount == 0 && bb.escaped) {
                Debug.Log("enemies cleared!!!");
                done = true;
                inChunk = false;
                boss.GetComponent<Animator>().SetTrigger("reEngage");

            }
        }
    }

    protected override async void enemyChunkSpawner(int delay_ms, int chunkNumber, int difficulty, float currentX) {
        // grab a reference to player movement so we can get Y-bounds
        int enemyIndex = UnityEngine.Random.Range(0,enemyList.Capacity);
        int enemySpawnNumber = 0;
        
        PlayerMovement pm = player.GetComponent<PlayerMovement>();

        //Gets a random enemy type from the enemy list
        
        //Spawn positions of the Enemy
        //float _xSpawnPos = currentX + Mathf.Round(UnityEngine.Random.Range(-25f,-20f));
        float _ySpawnPos = UnityEngine.Random.Range(pm.minY+0.5f,pm.maxY-0.5f);

        enemySpawnNumber = UnityEngine.Random.Range(5, 8);

        for(int i = 0; i < enemySpawnNumber; i++) {
            if (_stopSpawn)
                break;
            float _xSpawnPos;
            enemyIndex = UnityEngine.Random.Range(0,enemyList.Capacity);

            // Randomly spawn enemies to the left or right of the player
            if (UnityEngine.Random.value < 0.5f) {
                _xSpawnPos = player.transform.position.x + Mathf.Round(UnityEngine.Random.Range(-25f,-20f));
            } else {
                _xSpawnPos = player.transform.position.x + Mathf.Round(UnityEngine.Random.Range(20f,25f));
            }

            GameObject enemy = Instantiate(enemyList[enemyIndex], new Vector3((_xSpawnPos), _ySpawnPos, 0), Quaternion.identity);
            enemy.transform.parent = enemyContainer.transform;
            numEnemies++;
            await Task.Delay(delay_ms);
        }
    }

}
