using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    Score scoreObj;
    PlayerHealth health;

    // Start is called before the first frame update
    void Start()
    {
        // grab reference to player health
        health = GameObject.Find("PlayerV4").GetComponent<PlayerHealth>();

        // grab score reference
        scoreObj = GameObject.Find("ScoreSystem").GetComponent<Score>();
    }

    // Update is called once per frame
    
    private void OnEnable() {
       LootBase.onLootPickup += lootPickup;
    }
    
    private void onDisable() {
       LootBase.onLootPickup -= lootPickup;
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
}
