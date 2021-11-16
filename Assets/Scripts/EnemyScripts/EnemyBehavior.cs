using System;
using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    public float speed = 9;
    [SerializeField, Tooltip("Attack damage")]
    public float damage = 1f;
    [SerializeField, Tooltip("Attack distance")]
    private float attackDist = 3.0f;
    [SerializeField, Tooltip("Attack cooldown in seconds")]
    float cooldown = 5f;
    [SerializeField, Tooltip("Parry knockback")]
    float parryKnockback = 0.5f;
    [SerializeField, Tooltip("Projectile for ranged enemies")]
    GameObject projectile = null;
    [SerializeField, Tooltip("How high the y-velocity of player sword must be to parry")]
    float parryVelocity = 1.5f;
    private PlayerMovement playerMovement;
    private GameObject player;
    
    private Transform enemylocal; 
    private Animator animator;
    private GameObject target;
    private CapsuleCollider2D capsuleCollider;
    private BoxCollider2D hitBoxCollider;
    private BoxCollider2D playerWeaponCollider;
    private Rigidbody2D rb;
    //private SpriteRenderer enemyBodySprite;

    private bool canAttack = true; 
    private bool canDamage = true;
    public enum AttackType
    {
        Melee,
        Projectile
    };

    public static event Action<AttackType, float> onPlayerDamaged;
    public static event Action<AttackType> onAttack;
    public static event Action<GameObject> parriedEvent;

    private EnemyBase enemyBase;
    private float scaleX;

    void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        //enemyBodySprite = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerHealth>().gameObject;

        rb = GetComponent<Rigidbody2D>();
        hitBoxCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        target = GameObject.Find("PlayerV4"); // find the player game object and target him
        playerWeaponCollider = GameObject.FindGameObjectWithTag("Weapon").GetComponent<BoxCollider2D>();
        scaleX = transform.localScale.x;
        playerMovement = player.GetComponent<PlayerMovement>();
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
            if (hit.tag == "Enemy" || hit.tag == "EnemyWeapon" || hit.tag == "Weapon" || hit.tag == "Loot")
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

        if (transform.position.y > playerMovement.maxY)
        {

            transform.Translate(Vector3.down * (transform.position.y - playerMovement.maxY));
        }
        else if(transform.position.y < playerMovement.minY)
        {
            transform.Translate(Vector3.up *  (playerMovement.maxY - transform.position.y));
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
            if (canAttack) {
                //StartCoroutine(AttackRoutine(0.5f));
                animator.SetTrigger("attack");
            } 
        } else {
            animator.SetBool("inRange", false);
        }
    }

    public void CheckParry() {
        // check if player's weapon touches our hitbox at the right moment
        if (canDamage) {
            if (hitBoxCollider.IsTouching(playerWeaponCollider)) {
                Rigidbody2D playerWeaponRB = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Rigidbody2D>();
                // check if the player sword is moving upward
                if (playerWeaponRB.velocity.y > parryVelocity) {
                    parryEvent();
                    canDamage = false;
                    StartCoroutine(enemyBase.FakeAddForceMotion(parryKnockback));
                }
            }
        }
    }

    // this function is called from the animation player on attack
    public void Attack() {
        canAttack = false;
        onAttack?.Invoke(AttackType.Melee);
        // enable the hitbox on the weapon
        //hitBoxCollider.enabled = true;
        if (canDamage) {
            // we have not parried, so check for damage
            if (hitBoxCollider.IsTouching(target.GetComponent<CapsuleCollider2D>())) {
                // the hitbox is touching the player capsule collider, deal damage!
                damagePlayerEvent(AttackType.Melee);
            }
        }
        //hitBoxCollider.enabled = false;
        
        animator.ResetTrigger("attack");
        StartCoroutine(AttackCoolDown(cooldown));
    }

    public void Shoot(){ 
        animator.ResetTrigger("attack");
        onAttack?.Invoke(AttackType.Projectile); 
        canAttack = false;
        Transform firePoint = transform.GetChild(1);
        if(projectile != null){
           Rigidbody2D rbBullet = Instantiate(projectile, firePoint.position, Quaternion.identity).GetComponent<Rigidbody2D>();
           rbBullet.GetComponent<projectile>().enemyBehavior = this;

           Vector3 differenceVect = (target.transform.position - transform.position).normalized;
           Vector2 shootVect = new Vector2(differenceVect.x, differenceVect.y);
           rbBullet.AddForce(shootVect * 2f, ForceMode2D.Impulse); 
        }
        
        StartCoroutine(AttackCoolDown(cooldown));
    }

    public IEnumerator AttackCoolDown(float time) { 
        yield return new WaitForSeconds(time);     // wait for 3 seconds until enemy can attack again
        canAttack = true;
        canDamage = true;
    }

    public IEnumerator riposteReset(float time) {
        yield return new WaitForSeconds(time);
        animator.SetBool("riposted", false);
    }

    public float getWeaponDamage() {
        return damage;
    }

    public void parryEvent() {
        parriedEvent?.Invoke(gameObject);
    }

    public void damagePlayerEvent(AttackType attackType) {
        onPlayerDamaged?.Invoke(attackType, getWeaponDamage());
    }
}
