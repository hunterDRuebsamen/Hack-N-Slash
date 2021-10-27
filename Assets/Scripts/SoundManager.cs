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
        WeaponBase.parriedEvent += onParry;
        WeaponBase.onEnemyDamaged += onEnemyHit;
        PlayerHealth.onPlayerDeath += onPlayerDeath;
    } 
    private void onDisable() {
        EnemyBehavior.onPlayerDamaged -= onPlayerHit;
        WeaponBase.parriedEvent -= onParry;
        WeaponBase.onEnemyDamaged -= onEnemyHit;
        PlayerHealth.onPlayerDeath -= onPlayerDeath;
    } 

    private void onPlayerHit(float dmg)
    {
        audioSource.PlayOneShot(playerHitSound);
        if (phObject.currentHealth > 0 && phObject.currentHealth <= 35) {
            audioSource.Play(); // play heartbeat
        }
    }

    private void onParry(GameObject enemyObject) 
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
}
