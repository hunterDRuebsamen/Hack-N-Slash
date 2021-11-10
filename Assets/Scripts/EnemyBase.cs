using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField, Tooltip("Enemy Health (hitpoints)")]
    public int health = 15;
    [SerializeField, Tooltip("Knockback Multiplier")]
    float knockbackFactor = 0.2f;
    
    Rigidbody2D rigidBody; 
    BoxCollider2D enemyWeapon;
    Animator animator;
    public static event Action<GameObject> onEnemyDeath;

    //public GameObject coinDrop;
    //public GameObject potionDrop;

    // Start is called before the first frame update
    void Start()
    {
        enemyWeapon = transform.GetChild(0).GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable() { // Watches for when the enemy gets hit
        WeaponBase.onEnemyDamaged += onEnemyHit;
    } 
    private void onDisable() {
        WeaponBase.onEnemyDamaged -= onEnemyHit;
    } 

    // when we receive the onEnemyHit event, we do knockback + damage.
    private void onEnemyHit(float damage, GameObject enemyObject) 
    {
        // check to see if the enemy that was hit is this enemy.
        if (this != null && this.gameObject == enemyObject) {
            health -= (int)Math.Round(damage);
            if(health <= 0) {
                onEnemyDeath?.Invoke(this.gameObject);
                animator.SetTrigger("death");
                StartCoroutine(Death());
                //Instantiate(coinDrop, transform.position, Quaternion.identity);
                //Instantiate(potionDrop, transform.position, Quaternion.identity);
            } else {
                Debug.Log("Enemy Health: "+health);
                animator.SetTrigger("hit");
                //Calculate knockback force
                StartCoroutine(FakeAddForceMotion(damage*knockbackFactor));
            }
        }
    }

    // This function adds a fake force to a Kinematic body
    public IEnumerator FakeAddForceMotion(float forceAmount)
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

    private IEnumerator Death() {
        //animator.ResetTrigger("death");
        yield return new WaitForSeconds(0.6f);
        Destroy(this.gameObject);
    }
}
