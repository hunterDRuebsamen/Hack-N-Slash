using System; // Added "using System" for System.Action to function
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private PlayerHealth phObject;
    private SpriteRenderer bloodOverlayRenderer;
    private Animation bloodAnim;

    private void Start() {
        phObject = FindObjectOfType<PlayerHealth>();
        bloodOverlayRenderer = GameObject.Find("BloodOverlay").GetComponent<SpriteRenderer>();
        bloodAnim = GameObject.Find("BloodOverlay").GetComponent<Animation>();
    }

    private void OnEnable() {
        EnemyBehavior.onPlayerDamaged += onPlayerHit;
        WeaponBase.onEnemyDamaged += onEnemyHit;
        PlayerHealth.onPlayerHealthChanged += onPlayerHealth;
    } 
    private void OnDisable() {
        EnemyBehavior.onPlayerDamaged -= onPlayerHit;
        WeaponBase.onEnemyDamaged -= onEnemyHit;
    } 
    private void onEnemyHit(float damage, GameObject enemyObject) {
        Debug.Log("Enemy has been hit for: "+ damage);
    }
    private void onPlayerHit(float damage) {
        Debug.Log("Player has been hit." + damage);

    }

    // called when the player's health changes
    private void onPlayerHealth(int cur_health) {
        if (cur_health > 0 && cur_health <= phObject.criticalHealthLevel) {
            bloodOverlayRenderer.enabled = true;
            bloodAnim.Play();
        } else {
            bloodOverlayRenderer.enabled = false;
            bloodAnim.Stop();
        }
    }
}
