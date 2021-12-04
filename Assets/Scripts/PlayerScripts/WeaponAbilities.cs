using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAbilities : MonoBehaviour
{
    private Score score;
    //private Player player;
    private WeaponBase weaponBase;

    private bool weaponPowerUp = false;

    private BoxCollider2D playerWeaponCollider;
    private Rigidbody2D rb;

    private EnemyBehavior enemy;
    private BoxCollider2D hitBoxCollider;
    [SerializeField]
    private int hitLimitKatana = 5;
    [SerializeField]
    private int hitLimitKama = 10;
    [SerializeField]
    private int hitLimitAxe = 10;
    private int curWeapon = 0;
    public bool breakBlock = false;


    // Start is called before the first frame update
    void Start()
    {
        score = GameObject.Find("ScoreSystem").GetComponent<Score>();
    
        weaponBase = GetComponent<WeaponBase>();
        enemy = GetComponent<EnemyBehavior>();
        hitBoxCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        playerWeaponCollider = GameObject.FindGameObjectWithTag("Weapon").GetComponent<BoxCollider2D>();

        if (GlobalVariables.HasKey("WeaponType")) 
            curWeapon = GlobalVariables.Get<int>("WeaponType");
    }

    private void OnEnable() { // Watches for when the enemy gets hit
        WeaponBase.onEnemyDamaged += weaponAbilitySelector;
        EnemyBase.onEnemyBlocked += isBlocking;
    } 
    private void OnDisable() {
        WeaponBase.onEnemyDamaged -= weaponAbilitySelector;
        EnemyBase.onEnemyBlocked -= isBlocking;
    } 

    private void weaponAbilitySelector(float damage, GameObject enemyObject) {
        int weaponLimit = 0;
        //This if block determines the hitLimit we should be using, based on the weapon
        if (curWeapon == 1){
            weaponLimit = hitLimitKatana;
        }
        else if (curWeapon == 2) {
            weaponLimit = hitLimitKama;
        }
        else if (curWeapon == 3) {
            weaponLimit = hitLimitAxe;
        }

        if (score.scoreValue < weaponLimit) {
            return;
        }
        //Checks for whether or not player is activiating their ability
        else {
            // break block ability for katana
            if(Input.GetMouseButton(0) && curWeapon == 1) {
                breakEnemyBlock(enemyObject);
            }
            // stun ability for axe
            else if(Input.GetMouseButton(0) && curWeapon == 3) {
                enemyStun(enemyObject);
            }
        }
    
    private void isBlocking() {
        pass
    }


    }

/* What I'm trying to do here is to have the weapon only be able to have ability when the score is divisible of 5
and then I'm trying to have it so that when weaponPowerUp is true and you click left mouse button, it'll detect once
the weapon touches an enemy and gets stunned */

/* I tried looking at EnemyBehavior for the animator's animations and how you wrote the collision of the weapon and the enemy
so some of the code is "inspired" from that script but I'm not sure if it's right */

    void enemyStun(GameObject enemyObject)
    {
        Animator enemyAnimator = enemyObject.GetComponent<Animator>();
        enemyAnimator.SetTrigger("stunned"); // stun the enemy (doesn't have animation yet, might just freeze enemy)
    }

    void breakEnemyBlock(GameObject enemyObject)
    {
        Animator enemyAnimator = enemyObject.GetComponent<Animator>();
        breakBlock = true;
        enemyAnimator.SetTrigger("guardBroken");
    }


}
