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
    private float attackDist = 3.0f;
    [SerializeField, Tooltip("Attack cooldown in seconds")]
    float cooldown = 5f;
    [SerializeField, Tooltip("Parry knockback")]
    float parryKnockback = 0.5f;
    private Animator animator;
    private Animation attackAnimation;
    private Animation parryTint;
    private GameObject target;
    private CapsuleCollider2D capsuleCollider;
    private BoxCollider2D hitBoxCollider;
    private GameObject weaponGameObject;
    private Rigidbody2D rb;
    private SpriteRenderer enemyBodySprite;

    private float weaponXpos = -0.4f;
    private float weaponYpos = 0f;
    private float weaponObjectXpos = 0.68f;

    private bool canAttack = true; 
    private bool canDamage = true;

    public static event Action<float> onPlayerDamaged;
    public static event Action onAttack;

    private EnemyBase enemyBase;

    void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        enemyBodySprite = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();
        weaponGameObject = transform.GetChild(0).gameObject;  // grab first child gameobject
        hitBoxCollider = weaponGameObject.GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        attackAnimation = transform.GetChild(0).GetComponent<Animation>();
        parryTint = GetComponent<Animation>();
        target = GameObject.FindWithTag("Player"); // find the player game object and target him

        // get weapon offset so we can later flip enemy
        weaponXpos = weaponGameObject.GetComponent<BoxCollider2D>().offset.x;
        weaponYpos = weaponGameObject.GetComponent<BoxCollider2D>().offset.y;
    }

    void OnEnable() {
        WeaponBase.parriedEvent += attackParried;
    }
    void OnDisable() {
        WeaponBase.parriedEvent -= attackParried;
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
            if (hit.tag == "Enemy" || hit.tag == "EnemyWeapon" || hit.tag == "Weapon")
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

            if (animator != null) { // sprite-sheet based enemies
                enemyBodySprite.flipX = false;
                weaponGameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-weaponXpos,weaponYpos);
            } else { // other enemy types
                enemyBodySprite.flipX = true; 
                weaponGameObject.transform.localPosition = new Vector3(weaponObjectXpos,weaponGameObject.transform.localPosition.y,0);
                attackAnimation.clip = attackAnimation.GetClip("EnemySwordRight");
            }
        }
        else
        {
            // player is to the left
            if (animator != null) {
                enemyBodySprite.flipX = true;
                weaponGameObject.GetComponent<BoxCollider2D>().offset = new Vector2(weaponXpos,weaponYpos);
            } else {
                enemyBodySprite.flipX = false;
                weaponGameObject.transform.localPosition = new Vector3(-weaponObjectXpos,weaponGameObject.transform.localPosition.y,0);
                attackAnimation.clip = attackAnimation.GetClip("EnemySwordLeft");
            }
        }

        float distToPlayer = Math.Abs(target.transform.position.x - transform.position.x);
        if (distToPlayer <= attackDist)
        {
            if (animator != null)
                animator.SetFloat("xvel_magnitude", 0);
            //Debug.Log("attack");
            // attack the player
            if (canAttack) {
                StartCoroutine(AttackRoutine(0.5f));
            } 
        } else {
            // move toward player
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
            
            if (animator != null)
                animator.SetFloat("xvel_magnitude", 1);
        }
    }

    IEnumerator AttackRoutine(float delay) {
        if (canAttack) {
            canAttack = false;
            if (animator != null)
                animator.SetTrigger("attack");
            else {
                parryTint.Play();
                attackAnimation.Play();
            }
            onAttack?.Invoke();
            yield return new WaitForSeconds(delay);
            hitBoxCollider.enabled = true;
            yield return new WaitForSeconds(delay);
            hitBoxCollider.enabled = false;
            if (animator != null)
                animator.ResetTrigger("attack");
            StartCoroutine(AttackCoolDown(cooldown));
        }
    }

    IEnumerator AttackCoolDown(float time) { 
        yield return new WaitForSeconds(time);     // wait for 3 seconds until enemy can attack again
        canAttack = true;
        canDamage = true;
    }

    // accessor function for the isAttacking bool
    public bool isEnemyAttacking() 
    {
        return hitBoxCollider.enabled;
    }

    public float getWeaponDamage() {
        return damage;
    }

    public void damagePlayerEvent() {
        if (canDamage) {
            // send the onPlayerDamaged event.  This public function can be called from the EnemyBaseWeapon script
            onPlayerDamaged?.Invoke(getWeaponDamage());
            // dont damage player immediately again.
            canDamage = false;
            //StartCoroutine(AttackCoolDown(cooldown));
        }
    }

    public void attackParried(GameObject enemyObject) {
        // send the onparried event.  This public function can be called from the EnemyBaseWeapon script
        if (this.gameObject == enemyObject) {
            Debug.Log("Parried");
            canDamage = false;
            StartCoroutine(AttackCoolDown(cooldown));
            StartCoroutine(enemyBase.FakeAddForceMotion(parryKnockback));
        }
    }
}
