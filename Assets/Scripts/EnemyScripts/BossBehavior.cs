using System;
using System.Collections;
using UnityEngine;

public class BossBehavior : EnemyBehavior
{

    int maxHealth = 0;
    GameObject shield;
    GameObject handProjectile;

    public GameObject projectile_boss = null;

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
    public override void Attack() {
        canAttack = false;
        emitAttack(AttackType.Melee);
        // enable the hitbox on the weapon
        //hitBoxCollider.enabled = true;
        if (canDamage) {
            // we have not parried, so check for damage
            if (hitBoxCollider.IsTouching(target.GetComponent<CapsuleCollider2D>())) {
                // the hitbox is touching the player capsule collider, deal damage!
                damagePlayerEvent(AttackType.Melee);
            }
        }
    }

    public void Shoot()
    {
        emitAttack(AttackType.Projectile);
        canAttack = false;
        Transform firePoint = transform.GetChild(3).GetChild(2).GetChild(1);
        if (projectile_boss != null)
        {
            Rigidbody2D rbBullet = Instantiate(projectile_boss, firePoint.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            rbBullet.GetComponent<projectile>().enemyBehavior = this;

            Vector3 differenceVect = (target.transform.position - transform.position).normalized;
            Vector2 shootVect = new Vector2(differenceVect.x, differenceVect.y);
            rbBullet.AddForce(shootVect * 2f, ForceMode2D.Impulse);
        }
    }

    override protected void Move() 
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
            if(canAttack) {
                canAttack = false;
                int rand = UnityEngine.Random.Range(0, 4);
                Debug.Log("random attack: " + rand);

                if (rand == 0) {
                animator.SetTrigger("stab");
                }
                else if (rand == 1) {
                    animator.SetTrigger("uppercut");
                }
                else if (rand == 2) {
                    animator.SetTrigger("attack");
                }
                else if (rand == 3) {
                    animator.SetTrigger("enraged");
                }
            }

        } else {
            animator.SetBool("inRange", false);
        }
    }

    public void startCoolDown() {
        StartCoroutine(AttackCoolDown(cooldown));
    }
}