using System;
using UnityEngine;

public class EnemyWeaponBase : MonoBehaviour
{
    private EnemyBehavior enemyBehavior;

    // Start is called before the first frame update
    void Start()
    {
        enemyBehavior = transform.parent.GetComponent<EnemyBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if (enemyBehavior.isEnemyAttacking()) 
        {
            if (col.tag == "Player") 
            {
                // we hit the player.  Send onPlayerHit event
                enemyBehavior.damagePlayerEvent();
            }
        }    
    }
}
