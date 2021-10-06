using System;
using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 9;
    [SerializeField, Tooltip("Attack damage")]
    public float damage = 1f;
    [SerializeField, Tooltip("Attack distance")]
    private const float attackDist = 2.5f;
    [SerializeField, Tooltip("Attack cooldown in seconds")]
    float cooldown = 2f;
    [SerializeField, Tooltip("target of the enemy pathing")]
    GameObject target;
    private CapsuleCollider2D capsuleCollider;
    private GameObject weaponGameObject;
    private SpriteRenderer enemyBodySprite;
    private Animation attackAnimation;

    private const float weaponXpos = 0.68f;

    private bool isAttacking; 

    public static event Action<float> onPlayerDamaged;
    public static event Action onAttack;

    void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        enemyBodySprite = GetComponent<SpriteRenderer>();
        attackAnimation = transform.GetChild(0).GetComponent<Animation>();
        weaponGameObject = transform.GetChild(0).gameObject;  // grab first child gameobject
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, capsuleCollider.size, 0);
 
        foreach (Collider2D hit in hits)
        {
            // Ignore our own collider.
            if (hit.tag == "Enemy" || hit.tag == "EnemyWeapon")
                continue;
 
            ColliderDistance2D colliderDistance = hit.Distance(capsuleCollider);
 
            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider
            // pushing us out of this one.
            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
            }
        }
    }

    void Move() 
    {
        // check if player is to the right or left of enemy, flip enemy gameobjects based on player position
        if (target.transform.position.x > transform.position.x)
        {
            enemyBodySprite.flipX = true;
            weaponGameObject.transform.localPosition = new Vector3(weaponXpos,weaponGameObject.transform.localPosition.y,0);
        }
        else
        {
            enemyBodySprite.flipX = true;
            weaponGameObject.transform.localPosition = new Vector3(-weaponXpos,weaponGameObject.transform.localPosition.y,0);
        }

        float distToPlayer = Math.Abs(target.transform.position.x - transform.position.x);
        if (distToPlayer <= attackDist)
        {
            //Debug.Log("attack");
            // attack the player
            if (!isAttacking) {
                attackAnimation.Play("EnemySword");
                onAttack?.Invoke();
            }
            StartCoroutine(AttackCoolDown(cooldown));
        } else {
            // move toward player
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
        }
    }

    IEnumerator AttackCoolDown(float time) { 
        if (!isAttacking) 
        {
            isAttacking = true;
            yield return new WaitForSeconds(time);     // wait for 3 seconds until you can dodge
            isAttacking = false;
        }
    }

    // accessor function for the isAttacking bool
    public bool isEnemyAttacking() 
    {
        return isAttacking;
    }

    public float getWeaponDamage() {
        return damage;
    }

    public void damagePlayerEvent() {
        // send the onPlayerDamaged event.  This public function can be called from the EnemyBaseWeapon script
        onPlayerDamaged?.Invoke(getWeaponDamage());
    }
}
