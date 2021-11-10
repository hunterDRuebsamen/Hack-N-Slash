using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    CapsuleCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CapsuleCollider2D>();
    }

    void OnTriggerEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Player") 
        { 
            //We hit the player
        } else if(collision.gameObject.tag == "Weapon") {
            //We hit the player's weapon
        } else {
            //We hit something else
        }
    }
}
