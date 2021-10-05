using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField, Tooltip("Enemy Health (hitpoints)")]
    int health = 15;
    [SerializeField, Tooltip("Knockback Multiplier")]
    float knockbackFactor = 0.6f;

    Rigidbody2D rigidBody;
    public static event Action<GameObject> onEnemyDeath;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    private void OnEnable() { // Watches for when the enemy gets hit
        WeaponBase.onWeaponTriggerHit += onEnemyHit;
    } 
    private void onDisable() {
        WeaponBase.onWeaponTriggerHit -= onEnemyHit;
    } 

    // We use OnTriggerEnter2D instead of OnCollisionEnter2D because the player hand is set to "isTrigger"
    private void onEnemyHit(float damage) 
    {
        health -= (int)Math.Round(damage);
        if(health <= 0) {
            //send death event
            onEnemyDeath?.Invoke(this.gameObject);
        }
        Debug.Log("Enemy Health: "+health);
        //Calculate knockback force
        StartCoroutine(FakeAddForceMotion(damage*knockbackFactor));     
    }

    // This function adds a fake force to a Kinematic body
    IEnumerator FakeAddForceMotion(float forceAmount)
    {
        float i = 0.01f;
        while (forceAmount > i)
        {
            rigidBody.velocity = new Vector2(forceAmount / i, rigidBody.velocity.y); // !! For X axis positive force
            i = i + Time.deltaTime;
            yield return new WaitForEndOfFrame();      
        }
        rigidBody.velocity = Vector2.zero;
        yield return null;
    }
}
