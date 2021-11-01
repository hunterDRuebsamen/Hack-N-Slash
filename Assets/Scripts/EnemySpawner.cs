using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField, Tooltip("The enemies that will be spawned")]
    public GameObject enemyPrefab;
    [SerializeField, Tooltip("Takes the x position of the player or GameManager as a whole to determine where the enemy is spawned")]
    public float spawnDist; //= transform.position.x
    [SerializeField, Tooltip("The maximum number of enemies that will appear")]
    public int maxEnemies;
    [SerializeField, Tooltip("How much time should occur between spawns")]
    public float spawnTime;

    private int numEnemies = 0;
    private bool _stopSpawn = false;
    private GameObject player;

    [SerializeField, Tooltip("List of Enemies we will spawn")]
    List<GameObject> enemyList;

    void Start() 
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        StartCoroutine(SpawnRoutine());
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
        while (_stopSpawn == false) 
        {
            yield return new WaitForSeconds(spawnTime);
            if (numEnemies < maxEnemies) 
            {
                float _xSpawnPos = player.transform.position.x + spawnDist + Mathf.Round(Random.Range(-4f,4f) * 10) / 10;
                float _ySpawnPos = Random.Range(0f,4f);

                int enemyIndex = Random.Range(0,enemyList.Capacity);
                // spawn a new enemy
                Instantiate(enemyList[enemyIndex], new Vector3(_xSpawnPos, _ySpawnPos, 0), Quaternion.identity);
                numEnemies++;
            }
        }
    }
}
