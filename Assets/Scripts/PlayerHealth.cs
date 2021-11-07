using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Tooltip("Maximum amount of Health the player has")]
    private int maxHealth = 100;
    private int currentHealth;

    public int criticalHealthLevel = 35;
    private HealthBar healthBar;

    public static event Action onPlayerDeath;
    public static event Action<int> onPlayerHealthChanged;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBar>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Watches for when the enemy gets hit
    private void OnEnable() 
    { 
        EnemyBehavior.onPlayerDamaged += onPlayerHit;
    } 
    private void OnDisable() 
    {
        EnemyBehavior.onPlayerDamaged -= onPlayerHit;
    } 

    // when we receive the playerHit event, we take damage
    void onPlayerHit(float damage)
    {
        currentHealth -= (int)Math.Round(damage);

        healthBar.SetHealth(currentHealth);

        Debug.Log("Player Health: " + currentHealth);
        onPlayerHealthChanged?.Invoke(currentHealth);
        if(currentHealth <= 0) {
            onPlayerDeath?.Invoke();
            Debug.Log("Player has died");
        }

    }

    public void addHealth(int addhealth) {
        currentHealth += addhealth;
        onPlayerHealthChanged?.Invoke(currentHealth);
    }

    public int getHealth() {
        return currentHealth;
    }
}
