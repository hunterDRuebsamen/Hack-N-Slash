using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class minotaurBehavior : EnemyBehavior
{
    int maxHealth;
    bool charge = true;
    private float chargeDistance = 5f;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = enemyBase.health;   
    }

    public override void Attack() {
        Debug.Log("minotaur attack");
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
                animator.SetTrigger("attack");
                emitAttack(AttackType.Melee);
            }
        }
        else if (distToPlayer > attackDist && distToPlayer < (attackDist + chargeDistance) && charge){
            if(canAttack) {
                animator.SetBool("inChargeRange", true);
                canAttack = false;
                Debug.Log("Charge");
                animator.SetTrigger("charge");
                //emitAttack(AttackType.Melee);
            }
        } else {
            animator.SetBool("inRange", false);
            animator.SetBool("inChargeRange", false);
        }
    }

    public async void chargeTimer(int duration) {
        charge = false;
        await Task.Delay(duration);
        charge = true;
    }

    public void startCoolDown() {
        StartCoroutine(AttackCoolDown(cooldown));
    }


}
