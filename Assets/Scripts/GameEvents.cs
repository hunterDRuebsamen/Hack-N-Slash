using System; // Added "using System" for System.Action to function
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private void OnEnable() {
        EnemyBase.onEnemyTriggerHit += enemyHit;
        PlayerMovement.onPlayerTriggerHit += playerHit;
    } 
    private void onDisable() {
        EnemyBase.onEnemyTriggerHit -= enemyHit;
        PlayerMovement.onPlayerTriggerHit -= playerHit;
    } 
    private void enemyHit() {
        Debug.Log("Enemy has been hit.");
    }
    private void playerHit() {
        Debug.Log("Player has been hit.");
    }
}
