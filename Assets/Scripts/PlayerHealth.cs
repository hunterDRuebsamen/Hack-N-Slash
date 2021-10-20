using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    public static event Action onPlayerDeath;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Watches for when the enemy gets hit
    private void OnEnable() 
    { 
        EnemyBehavior.onPlayerDamaged += playerHit;
        //PlayerMovement.onPlayerTriggerHit += playerHit;
    } 
    private void onDisable() 
    {
        EnemyBehavior.onPlayerDamaged -= playerHit;
        //PlayerMovement.onPlayerTriggerHit -= playerHit;
    } 

    // when we receive the playerHit event, we take damage
    void playerHit(float damage)
    {
        currentHealth -= (int)Math.Round(damage);

        healthBar.SetHealth(currentHealth);

        Debug.Log("Player Health: " + currentHealth);

        if(currentHealth <= 0) {
            onPlayerDeath?.Invoke();
            Debug.Log("Player has died");
        }

    }


}
