using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    CapsuleCollider2D col;
    public EnemyBehavior enemyBehavior;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CapsuleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player") 
        { 
            //We hit the player
            enemyBehavior.damagePlayerEvent(EnemyBehavior.AttackType.Projectile);
            Destroy(this.gameObject);
        } else if(collision.gameObject.tag == "Weapon") {
            //We hit the player's weapon
            enemyBehavior.parryEvent();
            Destroy(this.gameObject);
        } else {
            //We hit something else
        }
    }


}
