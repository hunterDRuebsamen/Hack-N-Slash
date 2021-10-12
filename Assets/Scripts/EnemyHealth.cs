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
        WeaponBase.onWeaponTriggerHit += TakeDamage;
    }

    void onDisable() {
        WeaponBase.onWeaponTriggerHit -= TakeDamage;
    }

    void TakeDamage(float damage)
    {
        currentHealth -= (int)Math.Round(damage);

        EnemyHealthbar.SetHealth(currentHealth);
    }
}