using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private AudioSource audioSource;

    [SerializeField] List<AudioClip> playerHitSound;
    [SerializeField] List<AudioClip> parrySound;
    [SerializeField] AudioClip enemyHitSound;
    [SerializeField] AudioClip playerDeathSound;
    [SerializeField] AudioClip bulletHitSound;
    [SerializeField] AudioClip lootPickupSound;
    [SerializeField] AudioClip potionPickupSound;
    [SerializeField] AudioClip healSound;
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
        PlayerHealth.healCall += onHeal;
        PlayerHealth.onPlayerDeath += onPlayerDeath;
        PlayerHealth.onPlayerHealthChanged += onPlayerHealth;
        EnemyBehavior.onAttack += onEnemyAttack;
        EnemyBase.onEnemyBlocked += onEnemyBlocked;
        EnemyBase.onEnemyDeath += onEnemyDeath;
        LootBase.onLootPickup += onLootPickup;
    } 
    private void OnDisable() {
        EnemyBehavior.onPlayerDamaged -= onPlayerHit;
        EnemyBehavior.parriedEvent -= onParry;
        WeaponBase.onEnemyDamaged -= onEnemyHit;
        PlayerHealth.healCall -= onHeal;
        PlayerHealth.onPlayerDeath -= onPlayerDeath;
        PlayerHealth.onPlayerHealthChanged -= onPlayerHealth;
        EnemyBehavior.onAttack -= onEnemyAttack;
        EnemyBase.onEnemyBlocked -= onEnemyBlocked;
        EnemyBase.onEnemyDeath -= onEnemyDeath;
        LootBase.onLootPickup -= onLootPickup;
    } 

    private void onPlayerHit(EnemyBehavior.AttackType attackType, float dmg)
    {
        if (attackType == EnemyBehavior.AttackType.Projectile)
            audioSource.PlayOneShot(bulletHitSound);
        else {
            if (dmg < 5) {
                audioSource.PlayOneShot(playerHitSound[2]); // play hit sound
            } else if (dmg < 10) {
                audioSource.PlayOneShot(playerHitSound[1]); // play hit sound
            } else {
                audioSource.PlayOneShot(playerHitSound[0]); // play hit sound
            }
        }
    }

    private void onPlayerHealth(int cur_health) 
    {
        if (cur_health > 0 && cur_health <= phObject.criticalHealthLevel) {
            audioSource.Play(); // play heartbeat
        } else {
            audioSource.Stop();
        }
    }
    
    private void onParry(GameObject enemyObject) 
    {
        int randomInt = UnityEngine.Random.Range(0,parrySound.Count);
        audioSource.PlayOneShot(parrySound[randomInt]);
    }
    private void onEnemyBlocked() 
    {
        int randomInt = UnityEngine.Random.Range(0,parrySound.Count);
        audioSource.PlayOneShot(parrySound[randomInt]);
    }

    private void onEnemyHit(float dmg, GameObject enemyObject) 
    {
        audioSource.PlayOneShot(enemyHitSound);
        AudioClip clip = enemyObject.GetComponent<EnemyBehavior>().hurtSound;
        if (clip != null) 
            audioSource.PlayOneShot(clip);
    }

    private void onPlayerDeath() {
        audioSource.Stop(); // stop heartbeat
        audioSource.PlayOneShot(playerDeathSound);
    }

    private void onEnemyAttack(GameObject enemyObject, EnemyBehavior.AttackType attackType) {
        AudioClip clip = enemyObject.GetComponent<EnemyBehavior>().attackSound;
        if (clip != null) 
            audioSource.PlayOneShot(clip);
    }

    private void onEnemyDeath(GameObject enemyObject) {
        AudioClip clip = enemyObject.GetComponent<EnemyBehavior>().deathSound;
        if (clip != null) 
            audioSource.PlayOneShot(clip);
    }

    private void onLootPickup(LootBase.LootType type, int val) {
        if (type == LootBase.LootType.Potion) {
            audioSource.PlayOneShot(potionPickupSound,1.0f);
        } else {
            audioSource.PlayOneShot(lootPickupSound);
        }
    }

    private void onHeal() {
        audioSource.PlayOneShot(healSound);
    }
}
