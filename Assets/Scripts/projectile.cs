using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    CapsuleCollider2D collider;
    public EnemyBehavior enemyBehavior;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CapsuleCollider2D>();
        
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player") 
        { 
            //We hit the player
            Destroy(this.gameObject);
            enemyBehavior.damagePlayer();
            
        } else if(collision.gameObject.tag == "Weapon") {
            //We hit the player's weapon
            Destroy(this.gameObject);
        } else {
            //We hit something else
        }
    }


}
