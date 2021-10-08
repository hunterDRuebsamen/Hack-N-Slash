using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject enemyPrefab1;
    // Start is called before the first frame update
    void Start()
    {
        // Spawn one enemy when the game starts at position -5,0,0
        Instantiate(enemyPrefab1, new Vector3(-5,0,0), Quaternion.identity);
    }
}
