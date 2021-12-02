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

    // Start is called before the first frame update
    void Start()
    {
        score = GameObject.Find("ScoreSystem").GetComponent<Score>();
        //player = GameObject.Find("PlayerV4").GetComponent<PlayerHealth>();
        weaponBase = GetComponent<WeaponBase>();
        enemy = GetComponent<EnemyBehavior>();
        hitBoxCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        playerWeaponCollider = GameObject.FindGameObjectWithTag("Weapon").GetComponent<BoxCollider2D>();
    }
 
    private void Update()
    {
        if (score.scoreValue > 1 && score.scoreValue % 5 == 0 ) // checks to see if score is multiple of 5
        {
            weaponPowerUp = true;
        }

    }

/* What I'm trying to do here is to have the weapon only be able to have ability when the score is divisible of 5
and then I'm trying to have it so that when weaponPowerUp is true and you click left mouse button, it'll detect once
the weapon touches an enemy and gets stunned */

/* I tried looking at EnemyBehavior for the animator's animations and how you wrote the collision of the weapon and the enemy
so some of the code is "inspired" from that script but I'm not sure if it's right */

    void enemyStun()
    {
        if (weaponPowerUp = true && Input.GetKey(KeyCode.Mouse0)) // checks to see if score is multiple of 5
        {
            if (hitBoxCollider.IsTouching(playerWeaponCollider)) // checking to see if weapon colides with enemy
            {
                Rigidbody2D playerWeaponRB = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Rigidbody2D>();
                //enemy.animator.SetTrigger("stunned"); // stun the enemy (doesn't have animation yet, might just freeze enemy)
            }
            weaponPowerUp = false;
        }
    }

}
