using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField]
    private float damageFactor = 0.5f;
    private float coolDownTimer = 0.5f;
    [SerializeField]
    private string Name;
    private bool canAttack = true;
    private Rigidbody2D rb; 

    //Event for weapon hit
    public static event Action<float> onWeaponTriggerHit;
    public static event Action parriedEvent;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        // check to see what we just got hit with
        if (col.tag == "Enemy")
        {
            //Calculate the damage based on velocity
            float vel = rb.velocity.magnitude;
            float damage = vel * damageFactor;
            //Debug.Log("Wepaon hit damage: "+damage);
            onWeaponTriggerHit?.Invoke(damage);
            canAttack = false;
            StartCoroutine(AttackCoolDown(coolDownTimer));
        } else if (col.tag == "EnemyWeapon") {
            if(rb.velocity.magnitude >= 5.5f)
                parriedEvent?.Invoke();
        }    
    }
    //Attack cool down timer for player
    IEnumerator AttackCoolDown(float time) { 
        yield return new WaitForSeconds(time);     // wait for time seconds until attacks register again
        canAttack = true;
    }
}
