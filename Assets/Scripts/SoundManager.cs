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

    // Start is called before the first frame updat
    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Register events to listen for
    private void OnEnable() { // Watches for when the enemy gets hit
        EnemyBehavior.onPlayerDamaged += onPlayerHit;
        WeaponBase.parriedEvent += onParry;
        WeaponBase.onEnemyDamaged += onEnemyHit;
    } 
    private void onDisable() {
        EnemyBehavior.onPlayerDamaged -= onPlayerHit;
        WeaponBase.parriedEvent -= onParry;
        WeaponBase.onEnemyDamaged += onEnemyHit;
    } 

    private void onPlayerHit(float dmg)
    {
        audioSource.PlayOneShot(playerHitSound);
    }

    private void onParry() 
    {
        audioSource.PlayOneShot(parrySound);
    }

    private void onEnemyHit(float dmg, GameObject enemyObject) 
    {
        audioSource.PlayOneShot(enemyHitSound);
    }
}
