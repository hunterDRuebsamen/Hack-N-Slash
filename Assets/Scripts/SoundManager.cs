using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private AudioSource audioSource;

    [SerializeField]
    AudioClip playerHitSound;
    [SerializeField]
    AudioClip parrySound;
    [SerializeField]
    AudioClip enemyHitSound;
    [SerializeField]
    AudioClip playerDeathSound;
    [SerializeField]
    AudioClip cannonFireSound;
    [SerializeField]
    AudioClip bulletHitSound;
    AudioClip heartbeatSound;
    

    private PlayerHealth phObject;

    // Start is called before the first frame updat
    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        phObject = FindObjectOfType<PlayerHealth>();
    }

    // Register events to listen for
    private void OnEnable() { // Watches for when the enemy gets hit
        EnemyBehavior.onPlayerDamaged += onPlayerHit;
        EnemyBehavior.parriedEvent += onParry;
        WeaponBase.onEnemyDamaged += onEnemyHit;
        PlayerHealth.onPlayerDeath += onPlayerDeath;
        PlayerHealth.onPlayerHealthChanged += onPlayerHealth;
        EnemyBehavior.onAttack += onEnemyAttack;
        EnemyBase.onEnemyBlocked += onEnemyBlocked;
    } 
    private void onDisable() {
        EnemyBehavior.onPlayerDamaged -= onPlayerHit;
        EnemyBehavior.parriedEvent -= onParry;
        WeaponBase.onEnemyDamaged -= onEnemyHit;
        PlayerHealth.onPlayerDeath -= onPlayerDeath;
        PlayerHealth.onPlayerHealthChanged -= onPlayerHealth;
        EnemyBehavior.onAttack -= onEnemyAttack;
        EnemyBase.onEnemyBlocked -= onEnemyBlocked;
    } 

    private void onPlayerHit(EnemyBehavior.AttackType attackType, float dmg)
    {
        if (attackType == EnemyBehavior.AttackType.Projectile)
            audioSource.PlayOneShot(bulletHitSound);
        else
            audioSource.PlayOneShot(playerHitSound); // play hit sound
    }

    private void onPlayerHealth(int cur_health) 
    {
        if (cur_health > 0 && cur_health <= phObject.criticalHealthLevel) {
            audioSource.Play(); // play heartbeat
        }
    }
    
    private void onParry(GameObject enemyObject) 
    {
        audioSource.PlayOneShot(parrySound);
    }
     private void onEnemyBlocked() 
    {
        audioSource.PlayOneShot(parrySound);
    }

    private void onEnemyHit(float dmg, GameObject enemyObject) 
    {
        audioSource.PlayOneShot(enemyHitSound);
    }

    private void onPlayerDeath() {
        audioSource.Stop(); // stop heartbeat
        audioSource.PlayOneShot(playerDeathSound);
    }

    private void onEnemyAttack(EnemyBehavior.AttackType attackType) {
        if (attackType == EnemyBehavior.AttackType.Projectile)
            audioSource.PlayOneShot(cannonFireSound);
    }
}
