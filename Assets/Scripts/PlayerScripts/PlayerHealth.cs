using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Tooltip("Maximum amount of Health the player has")]
    private int maxHealth = 100;
    private int currentHealth;
    public int healLimitCount = 15;
    public int criticalHealthLevel = 35;
    private HealthBar healthBar;

    public static event Action onPlayerDeath;
    public static event Action<int> onPlayerHealthChanged;
    public static event Action healCall;
    private Score score;

    // Start is called before the first frame update
    void Start()
    {
        score = GameObject.Find("ScoreSystem").GetComponent<Score>();
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
    
    void Update() {
        heal();
    }

    // when we receive the playerHit event, we take damage
    void onPlayerHit(EnemyBehavior.AttackType attackType, float damage)
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

    void heal() {
        if (score.scoreValue >= healLimitCount && Input.GetKeyDown("h")) {
            currentHealth += 10;
            healthBar.SetHealth(currentHealth);
            healCall?.Invoke();
            Debug.Log("Player has healed" + currentHealth);
            score.scoreValue = 0;
        }
        else {
            return;
        }
    }

    public void editHealth(int addhealth) {
        currentHealth += addhealth;
        onPlayerHealthChanged?.Invoke(currentHealth);
    }

    public int getHealth() {
        return currentHealth;
    }

}
