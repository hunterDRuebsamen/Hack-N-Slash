using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField, Tooltip("The enemies that will be spawned")]
    public GameObject enemyPrefab1;
    [SerializeField, Tooltip("Takes the x position of the player or GameManager as a whole to determine where the enemy is spawned")]
    public float fromPlayer; //= transform.position.x
   [SerializeField, Tooltip("The maximum number of enemies that will appear")]
    public float enemyLimit ;
    [ Tooltip("The current amount of enemies on screen")]
    private float enemyCount = 0;
    [SerializeField, Tooltip("How much time should occur between spawns")]
    public float spawnTime;
    [SerializeField, Tooltip("Store the time taken inbetween spawns and tells the system when to spawn an enemy")]
    private float btwSpawns;

    private void Update()
    {
        if (btwSpawns <= 0 && enemyCount < enemyLimit)
        {
            Instantiate(enemyPrefab1, new Vector3(fromPlayer + 10, 0, 0) , Quaternion.identity);
            btwSpawns = spawnTime;
            enemyCount += 1;
        }
        else
        {
            btwSpawns -= Time.deltaTime;
        }
    }
    void Start()
    {
        // Spawn one enemy when the game starts at position -5,0,0
        //Instantiate(enemyPrefab1, new Vector3(-5,0,0), Quaternion.identity);
    }
}
