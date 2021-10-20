using UnityEngine;
using System.Collections;

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

    void Start() 
    {
        StartCoroutine(SpawnRoutine());
    }

    void OnEnable() {
        EnemyBase.onEnemyDeath += enemySubtractor;
        PlayerHealth.onPlayerDeath += playerDeath;
    }

    void OnDisable() {
        EnemyBase.onEnemyDeath -= enemySubtractor;
        PlayerHealth.onPlayerDeath += playerDeath;
    }

    private void enemySubtractor(GameObject gO) {
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
                float _xSpawnPos = spawnDist + Mathf.Round(Random.Range(-9f,9f) * 10) / 10;
                float _ySpawnPos = Random.Range(0f,4f);

                // spawn a new enemy
                Instantiate(enemyPrefab, new Vector3(_xSpawnPos, _ySpawnPos, 0), Quaternion.identity);
                numEnemies++;
            }
        }
    }
}
