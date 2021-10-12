using System; // Added "using System" for System.Action to function
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private void OnEnable() {
        WeaponBase.onWeaponTriggerHit += weaponHit;
        PlayerMovement.onPlayerTriggerHit += playerHit;
    } 
    private void OnDisable() {
        WeaponBase.onWeaponTriggerHit -= weaponHit;
        PlayerMovement.onPlayerTriggerHit -= playerHit;
    } 
    private void weaponHit(float damage) {
        Debug.Log("Enemy has been hit for: "+ damage);
    }
    private void playerHit() {
        Debug.Log("Player has been hit.");
    }
}
