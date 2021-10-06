using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource effectSource1;

    [SerializeField]
    AudioClip hitImpactClip;

    // Start is called before the first frame updat

    // Register events to listen for
    private void OnEnable() { // Watches for when the enemy gets hit
        EnemyBehavior.onPlayerDamaged += onPlayerHit;
    } 
    private void onDisable() {
        EnemyBehavior.onPlayerDamaged -= onPlayerHit;
    } 

    private void onPlayerHit(float dmg)
    {
        //Debug.Log("play auido");
        effectSource1.PlayOneShot(hitImpactClip);
    }
}
