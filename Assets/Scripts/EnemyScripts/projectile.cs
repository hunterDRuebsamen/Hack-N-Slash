using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class projectile : MonoBehaviour
{
    CapsuleCollider2D col;
    public EnemyBehavior enemyBehavior;
    //Event when the player parries the projectile
    public static event Action<GameObject> onProjecParry;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CapsuleCollider2D>();
        destroyBullet(3500);
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
            onProjecParry?.Invoke(this.gameObject);
            Destroy(this.gameObject);
        } else {
            //We hit something else
        }
    }

    private async void destroyBullet(int duration) {
        await Task.Delay(duration); //Duration is in miliseconds
        Destroy(this.gameObject);
    }
}
