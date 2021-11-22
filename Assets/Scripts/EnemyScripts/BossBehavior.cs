using System;
using System.Collections;
using UnityEngine;

public class BossBehavior : EnemyBehavior
{

    int maxHealth = 0;
    GameObject shield;
    GameObject handProjectile;

    void Start() {
        maxHealth = enemyBase.health;
        shield = this.gameObject.transform.GetChild(3).GetChild(2).GetChild(0).gameObject;
        handProjectile = this.gameObject.transform.GetChild(3).GetChild(2).GetChild(1).gameObject;
    }
    private void OnEnable() { // Watches for when the enemy gets hit
        WeaponBase.onEnemyDamaged += onBossHit;
    } 
    private void OnDisable() {
        WeaponBase.onEnemyDamaged -= onBossHit;
    }

    private void onBossHit(float damage, GameObject enemyObject) 
    {
        // check to see if the enemy that was hit is this enemy.
        if (this != null && this.gameObject == enemyObject) {
            if(enemyBase.health <= maxHealth/2)
            {
                animator.SetBool("isEnraged", true);
                shield.SetActive(false);
                handProjectile.SetActive(true);
            }
        }

    }

    void Move() 
    {
        // check if player is to the right or left of enemy, flip enemy gameobjects based on player position
        if (target.transform.position.x > transform.position.x)
        {
            // player is to the right;
            transform.localScale = new Vector3(-scaleX,transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // player is to the left
            transform.localScale = new Vector3(scaleX,transform.localScale.y, transform.localScale.z);
        }

        float distToPlayer = Math.Abs(target.transform.position.x - transform.position.x);
        if (animator.GetBool("inRange")) {
            // give a little buffer when player goes outside of attack range
            distToPlayer -= 0.35f;
        }
        if (distToPlayer <= attackDist)
        {
            animator.SetBool("inRange", true);
            int rand = UnityEngine.Random.Range(0, 3);

            if (rand == 0) {
            animator.SetTrigger("stab");
            }
            else if (rand == 1) {
                animator.SetTrigger("uppercut");
            }
            else if (rand == 2) {
                animator.SetTrigger("attack");
            }
        } else {
            animator.SetBool("inRange", false);
        }
    }
}