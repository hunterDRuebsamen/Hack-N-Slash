using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField, Tooltip("The enemies that will be spawned")]
    public GameObject enemyPrefab;
    [SerializeField, Tooltip("Takes the x position of the player or GameManager as a whole to determine where the enemy is spawned")]
    public float spawnDist; //= transform.position.x
    [SerializeField, Tooltip("The maximum number of enemies that will appear")]
    public int maxEnemies;
    [Tooltip("The current amount of enemies on screen")]
    private int numEnemies = 0;
    [SerializeField, Tooltip("How much time should occur between spawns")]
    public float spawnTime;
    [SerializeField, Tooltip("Store the time taken inbetween spawns and tells the system when to spawn an enemy")]
    private float btwSpawns;

    void OnEnable() {
        EnemyBase.onEnemyDeath += enemySubtractor;
    }

    void OnDisable() {
        EnemyBase.onEnemyDeath -= enemySubtractor;
    }

    private void Update()
    {
        if (btwSpawns <= 0 && numEnemies < maxEnemies)
        {
            for(int enemyCount = 0; enemyCount < maxEnemies; enemyCount++)
            {
                Instantiate(enemyPrefab, new Vector3(spawnDist + 10, Random.Range(0,4), 0), Quaternion.identity);

            }
            btwSpawns = spawnTime;
            numEnemies += maxEnemies;
        }
        else
        {
            btwSpawns -= Time.deltaTime;
        }
    }

    private void enemySubtractor(GameObject gO) {
        numEnemies--;
    }
}
