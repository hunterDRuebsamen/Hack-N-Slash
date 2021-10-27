using System; // Added "using System" for System.Action to function
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private void OnEnable() {
        EnemyBehavior.onPlayerDamaged += playerHit;
        WeaponBase.onWeaponTriggerHit += weaponHit;
    } 
    private void OnDisable() {
        EnemyBehavior.onPlayerDamaged -= playerHit;
        WeaponBase.onWeaponTriggerHit -= weaponHit;
    } 
    private void weaponHit(float damage) {
        Debug.Log("Enemy has been hit for: "+ damage);
    }
    private void playerHit(float damage) {
        Debug.Log("Player has been hit." + damage);
    }
}
