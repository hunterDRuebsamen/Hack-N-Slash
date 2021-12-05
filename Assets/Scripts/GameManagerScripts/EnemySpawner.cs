using UnityEngine;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cinemachine;

public class EnemySpawner : EnemySpawnerBase
{
    [SerializeField, Tooltip("Takes the x position of the player or GameManager as a whole to determine where the enemy is spawned")]
    public float spawnDist; //= transform.position.x
    [SerializeField, Tooltip("The maximum number of enemies that will appear")]
    public int maxEnemies;
    [SerializeField, Tooltip("How much time should occur between spawns")]
    public float spawnTime;

    [SerializeField, Tooltip("The maximum distance away from the player an enemy can be before it is despawned.")]
    public float maxDist = 40f;

    [SerializeField] protected GameObject enemyContainer;
    [SerializeField] protected GameObject tempWall;

    [SerializeField] protected CinemachineVirtualCamera vcam;

    [SerializeField] protected int enemyRespawnTimer = 0;

    protected int numEnemies = 0;
    private bool _stopSpawn = false;
    protected GameObject player;

    private float originalDeadzoneWidth;

    [SerializeField, Tooltip("List of Enemies we will randomly spawn")]
    protected List<GameObject> enemyList;

    protected GameObject[] walls = {null, null};

    protected bool inChunk = false;

    public int difficulty = 0;

    void Start() 
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        StartCoroutine(SpawnRoutine());
        ClearEnemies(maxDist);

        var composer = vcam.GetCinemachineComponent<CinemachineComposer>();
        originalDeadzoneWidth = composer.m_DeadZoneWidth;
    }

    void OnEnable() {
        EnemyBase.onEnemyDeath += enemyDeath;
        PlayerHealth.onPlayerDeath += playerDeath;
        PlayerMovement.onChunk += chunkReached;
    }

    void OnDisable() {
        EnemyBase.onEnemyDeath -= enemyDeath;
        PlayerHealth.onPlayerDeath += playerDeath;
        PlayerMovement.onChunk += chunkReached;
    }

    private void enemyDeath(GameObject gO) {
        numEnemies--;
    }

    private void playerDeath() {
        // record when player dies, so we no longer spawn enemies
        _stopSpawn = true;
    }


    protected override void chunkReached(float currentX, int chunkNumber) {
        inChunk = true;
        // 1. pause camera movement
        var composer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        composer.m_DeadZoneWidth = 2f;
        // 2. limit player movement to screen (-11.5, 12.9) - by spawing walls
        
        walls[0] = Instantiate(tempWall, new Vector3(vcam.transform.position.x-12.5f,0,0), Quaternion.identity);
        walls[1] = Instantiate(tempWall, new Vector3(vcam.transform.position.x+12.5f,0,0), Quaternion.identity);
        walls[0].transform.parent = enemyContainer.transform;
        walls[0].transform.localScale = new Vector3(-1f, 1f, 1f);;
        walls[1].transform.parent = enemyContainer.transform;
        // 3. Spawn Enemies (TODO)
        enemyChunkSpawner(enemyRespawnTimer, chunkNumber, difficulty, currentX);
        
        // 4. When enemies are killed go back to normal movement!
        CheckChunkCleared(2000);
    } 

    private IEnumerator SpawnRoutine()
    {
        // grab a reference to player movement so we can get Y-bounds
        PlayerMovement pm = player.GetComponent<PlayerMovement>();

        while (_stopSpawn == false) 
        {
            yield return new WaitForSeconds(spawnTime);
            if ((numEnemies < maxEnemies) && !inChunk) 
            {
                float _xSpawnPos = player.transform.position.x + spawnDist + Mathf.Round(UnityEngine.Random.Range(-4f,4f) * 10) / 10;
                float _ySpawnPos = UnityEngine.Random.Range(pm.minY+0.5f,pm.maxY-0.5f);

                int enemyIndex = UnityEngine.Random.Range(0,enemyList.Capacity);
                // spawn a new enemy
                GameObject enemy = Instantiate(enemyList[enemyIndex], new Vector3(_xSpawnPos, _ySpawnPos, 0), Quaternion.identity);
                enemy.transform.parent = enemyContainer.transform;
                numEnemies++;
            }
        }
    }

    // this function will periodically destroy the enemies who are TOO far away from the player
    private async void ClearEnemies(float maxDist)
    {

        while(_stopSpawn == false)
        {
            await Task.Delay(1500);
            EnemyBase[] enemies = GameObject.FindObjectsOfType<EnemyBase>();
            foreach (EnemyBase enemy in enemies) {
                if (Math.Abs(player.transform.position.x - enemy.transform.position.x) > maxDist) {
                    Destroy(enemy.gameObject);
                    numEnemies--;
                }
            }
        }
    }


    protected async void CheckChunkCleared(int delay_ms)
    {
        bool done = false;

        while(!done)
        {
            await Task.Delay(delay_ms);
            EnemyBase[] enemies = GameObject.FindObjectsOfType<EnemyBase>();
            if (enemies.Length == 0) {
                Debug.Log("enemies cleared!!!");
                done = true;
                inChunk = false;

                // clean up everything here
                // 1. destroy walls
                if (walls[0] != null) {
                    Destroy(walls[0]);
                    walls[0] = null;
                }
                if (walls[1] != null) {
                    Destroy(walls[1]);
                    walls[1] = null;
                }
                // 2. re-enable camera movement
                var composer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
                composer.m_DeadZoneWidth = originalDeadzoneWidth;
            }
        }
    }

    protected override async void enemyChunkSpawner(int delay_ms, int chunkNumber, int difficulty, float currentX) {
        // grab a reference to player movement so we can get Y-bounds
        PlayerMovement pm = player.GetComponent<PlayerMovement>();

        //Gets a random enemy type from the enemy list
        int enemyIndex = UnityEngine.Random.Range(0,enemyList.Capacity);
        int enemySpawnNumber = 0;
        
        //Spawn positions of the Enemy
        //float _xSpawnPos = currentX + Mathf.Round(UnityEngine.Random.Range(-25f,-20f));
        float _ySpawnPos = UnityEngine.Random.Range(pm.minY+0.5f,pm.maxY-0.5f);

        //If statements determine the number of enemies that should spawn
        if(chunkNumber <= 3) {
            enemySpawnNumber = UnityEngine.Random.Range(6, 8);
        }
        else if (chunkNumber > 3 && chunkNumber <= 8) {
            enemySpawnNumber = UnityEngine.Random.Range(8, 13);
        }

        for(int i = 0; i < enemySpawnNumber; i++) {
            float _xSpawnPos;
            enemyIndex = UnityEngine.Random.Range(0,enemyList.Capacity);

            // Randomly spawn enemies to the left or right of the player
            if (UnityEngine.Random.value < 0.5f) {
                _xSpawnPos = currentX + Mathf.Round(UnityEngine.Random.Range(-25f,-20f));
            } else {
                _xSpawnPos = currentX + Mathf.Round(UnityEngine.Random.Range(20f,25f));
            }

            GameObject enemy = Instantiate(enemyList[enemyIndex], new Vector3((_xSpawnPos), _ySpawnPos, 0), Quaternion.identity);
            enemy.transform.parent = enemyContainer.transform;
            numEnemies++;
            await Task.Delay(delay_ms);
        }
    }
}
