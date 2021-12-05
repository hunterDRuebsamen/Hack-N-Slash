using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;

public class BossEnemySpawner : EnemySpawner
{

    // Start is called before the first frame update
    //PlayerMovement pm;
    public GameObject boss;
    BossBehavior bb;
    void Start()
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        bb = boss.GetComponent<BossBehavior>();
        //pm = player.gameObject.GetComponent<PlayerMovement>();
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
        // 3. Spawn Enemies 
        enemyChunkSpawner(enemyRespawnTimer, chunkNumber, difficulty, currentX);
        
        // 4. When enemies are killed go back to normal movement!
        CheckChunkCleared(2000);
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

        //If statements determine the number of enemies that should spawn
        if(chunkNumber <= 1) {
            Instantiate(boss, new Vector3(player.transform.position.x+15f,0,0), Quaternion.identity);
        }
        if (bb.escaped == true) {
            enemySpawnNumber = UnityEngine.Random.Range(5, 8);
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
