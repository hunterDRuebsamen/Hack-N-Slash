using UnityEngine;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField, Tooltip("Takes the x position of the player or GameManager as a whole to determine where the enemy is spawned")]
    public float spawnDist; //= transform.position.x
    [SerializeField, Tooltip("The maximum number of enemies that will appear")]
    public int maxEnemies;
    [SerializeField, Tooltip("How much time should occur between spawns")]
    public float spawnTime;

    [SerializeField, Tooltip("The maximum distance away from the player an enemy can be before it is despawned.")]
    public float maxDist = 40f;


    private int numEnemies = 0;
    private bool _stopSpawn = false;
    private GameObject player;

    [SerializeField, Tooltip("List of Enemies we will randomly spawn")]
    List<GameObject> enemyList;

    void Start() 
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        StartCoroutine(SpawnRoutine());
        ClearEnemies(maxDist);
    }

    void OnEnable() {
        EnemyBase.onEnemyDeath += enemyDeath;
        PlayerHealth.onPlayerDeath += playerDeath;
    }

    void OnDisable() {
        EnemyBase.onEnemyDeath -= enemyDeath;
        PlayerHealth.onPlayerDeath += playerDeath;
    }

    private void enemyDeath(GameObject gO) {
        numEnemies--;
    }

    private void playerDeath() {
        // record when player dies, so we no longer spawn enemies
        _stopSpawn = true;
    }

    private IEnumerator SpawnRoutine()
    {
        // grab a reference to player movement so we can get Y-bounds
        PlayerMovement pm = player.GetComponent<PlayerMovement>();

        while (_stopSpawn == false) 
        {
            yield return new WaitForSeconds(spawnTime);
            if (numEnemies < maxEnemies) 
            {
                float _xSpawnPos = player.transform.position.x + spawnDist + Mathf.Round(UnityEngine.Random.Range(-4f,4f) * 10) / 10;
                float _ySpawnPos = UnityEngine.Random.Range(pm.minY+0.5f,pm.maxY-0.5f);

                int enemyIndex = UnityEngine.Random.Range(0,enemyList.Capacity);
                // spawn a new enemy
                Instantiate(enemyList[enemyIndex], new Vector3(_xSpawnPos, _ySpawnPos, 0), Quaternion.identity);
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
}
