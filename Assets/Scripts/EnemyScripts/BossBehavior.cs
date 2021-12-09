using System;
using System.Collections;
using UnityEngine;
using Cinemachine;

public class BossBehavior : EnemyBehavior
{

    int maxHealth = 0;
    GameObject shield;
    GameObject handProjectile;
    public bool escaped = false;
    float meleeAttackDist = 0f;
    float enrageAttackDist = 0f;
    CinemachineVirtualCamera vcam;

    public static event Action onSpawnMinion;

    void Start() {
        maxHealth = enemyBase.health;
        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        //handProjectile = this.gameObject.transform.GetChild(3).GetChild(2).GetChild(1).gameObject;
        meleeAttackDist = attackDist;
        enrageAttackDist = attackDist*3.5f;
    }
    private void OnEnable() { // Watches for when the enemy gets hit
        WeaponBase.onEnemyDamaged += onBossHit;
    } 
    private void OnDisable() {
        WeaponBase.onEnemyDamaged -= onBossHit;
    }

    private void onBossHit(float damage, GameObject enemyObject) 
    {
        if (this != null && this.gameObject == enemyObject && !escaped) {
            if(enemyBase.health <= maxHealth/2)
            {
                animator.SetTrigger("escape");
                escaped = true;
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
        startCoolDown();
    }

    public override void Shoot()
    {
        emitAttack(AttackType.Projectile);
        canAttack = false;
        Transform firePoint = transform.GetChild(1).GetChild(2).GetChild(0);
        if (projectile != null)
        {
            Rigidbody2D rbBullet = Instantiate(projectile, firePoint.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            rbBullet.GetComponent<projectile>().enemyBehavior = this;

            Vector3 differenceVect = (target.transform.position - transform.position).normalized;
            Vector2 shootVect = new Vector2(differenceVect.x, differenceVect.y);
            rbBullet.AddForce(shootVect * 2f, ForceMode2D.Impulse);
        }
        startCoolDown();
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
                int rand = UnityEngine.Random.Range(0, 3);
                
                if(enemyBase.health <= maxHealth/2){
                    rand = UnityEngine.Random.Range(0, 6);
                }
                if (rand == 0) {
                    animator.SetTrigger("stab");
                }
                else if (rand == 1) {
                    animator.SetTrigger("uppercut");
                }
                else if (rand == 2) {
                    animator.SetTrigger("attack");
                }
                else if (rand > 3 && enemyBase.health <= maxHealth/2) {
                    animator.SetTrigger("enraged"); // (this is the projectile attack)
                    attackDist = enrageAttackDist;
                    StartCoroutine(EnrageCoolDown(4));
                }
            }

        } else {
            animator.SetBool("inRange", false);
        }
    }

    public void startCoolDown() {
        StartCoroutine(AttackCoolDown(cooldown));
    }

    public void escape() {
        Vector2 newPos = Vector2.MoveTowards(rb.position, new Vector2(vcam.transform.position.x +20f, 0), speed*Time.deltaTime*7);
        rb.MovePosition(newPos);
    }

    public void spawnMinion() {
        onSpawnMinion?.Invoke();
    }

    public IEnumerator EnrageCoolDown(float time) { 
        yield return new WaitForSeconds(time);     // wait for 3 seconds until enemy can attack again
        attackDist = meleeAttackDist;
    }
}