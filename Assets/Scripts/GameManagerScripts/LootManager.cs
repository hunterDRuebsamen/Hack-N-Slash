using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    Score scoreObj;
    PlayerHealth health;
    Transform player;

    [SerializeField, Tooltip("List of Spawnable objects")]
    List<GameObject> lootList;

    // Start is called before the first frame update
    void Start()
    {
        // grab reference to player health
        player = GameObject.Find("PlayerV4").transform;
        health = player.GetComponent<PlayerHealth>();

        // grab score reference
        scoreObj = GameObject.Find("ScoreSystem").GetComponent<Score>();
    }

    // Update is called once per frame
    
    private void OnEnable() {
       LootBase.onLootPickup += lootPickup;
       EnemyBase.onEnemyDeath += spawnLoot;
    }
    
    private void OnDisable() {
       LootBase.onLootPickup -= lootPickup;
       EnemyBase.onEnemyDeath -= spawnLoot;
    }

    private void lootPickup(LootBase.LootType type, int value) {
       if (type == LootBase.LootType.Coin) {
           scoreObj.updateScore(value);
       } else if (type == LootBase.LootType.Potion) {
           health.editHealth(value);
       } else {
           // weapon pickup could go here.
       }
    }

    private void spawnLoot(GameObject enemy) {

        foreach(GameObject loot in lootList) {
            float val = Random.value;
            if (val <= loot.GetComponent<LootBase>().spawnRate) {
                if (player.localScale.x > 0) {
                    // player is facing right, spawn loot +x
                    Instantiate(loot, new Vector3(enemy.transform.position.x+0.6f, enemy.transform.position.y, 0), Quaternion.identity);
                } else {
                    // player is facing right, spawn loot -x
                    Instantiate(loot, new Vector3(enemy.transform.position.x-0.6f, enemy.transform.position.y, 0), Quaternion.identity);
                }
                    break;
            }
        }

    }
}
