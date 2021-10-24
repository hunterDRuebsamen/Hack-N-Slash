using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    
    public int currentHealth;

    public EnemyHealthbar EnemyHealthbar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = transform.parent.parent.GetComponent<EnemyBase>().health;
        EnemyHealthbar.SetMaxHealth(currentHealth);
    }

    void OnEnable() {
        WeaponBase.onEnemyDamaged += TakeDamage;
    }

    void onDisable() {
        WeaponBase.onEnemyDamaged -= TakeDamage;
    }

    void TakeDamage(float damage, GameObject enemyObject)
    {
        // check to see if this enemy is the one taking damage
        if (this.gameObject == enemyObject) {
            currentHealth -= (int)Math.Round(damage);

            EnemyHealthbar.SetHealth(currentHealth);
        }
    }
}