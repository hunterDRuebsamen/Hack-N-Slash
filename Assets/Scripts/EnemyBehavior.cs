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
    float cooldown = 5f;

    private GameObject target;
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

        target = GameObject.FindWithTag("Player"); // find the player game object and target him
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
            // player is to the right;
            attackAnimation.clip = attackAnimation.GetClip("EnemySwordRight");
            enemyBodySprite.flipX = true;
            weaponGameObject.transform.localPosition = new Vector3(weaponXpos,weaponGameObject.transform.localPosition.y,0);
        }
        else
        {
            // player is to the left
            attackAnimation.clip = attackAnimation.GetClip("EnemySwordLeft");
            enemyBodySprite.flipX = false;
            weaponGameObject.transform.localPosition = new Vector3(-weaponXpos,weaponGameObject.transform.localPosition.y,0);
        }

        float distToPlayer = Math.Abs(target.transform.position.x - transform.position.x);
        if (distToPlayer <= attackDist)
        {
            //Debug.Log("attack");
            // attack the player
            if (!isAttacking) {
                attackAnimation.Play();
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
