using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnTrigger : MonoBehaviour
{

    [SerializeField] BossEnemySpawner bes;


    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("GameObject1 collided with " + col.name);
        if(col.name == "PlayerV4") {
            bes.chunkReached(0,0);
        }
    }
}
