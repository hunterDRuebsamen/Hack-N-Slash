using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField]
    private float damageFactor = 0.5f;
    [SerializeField]
    private float coolDownTimer = 0.6f;
    [SerializeField, Tooltip("Player Attack cooldown in seconds")]
    private string Name;
    private bool canAttack = true;
    private Rigidbody2D rb; 

    // sword must be moving at this velocity before its considered an attack
    const float minMagnitude = 1f;

    //Event for weapon hit
    public static event Action<float, GameObject> onEnemyDamaged;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        // check to see what we just got hit with
        if (col.tag == "Enemy")
        {
            if (canAttack && (rb.velocity.magnitude > minMagnitude)) {
                //Calculate the damage based on velocity
                float vel = rb.velocity.magnitude;
                float damage = vel * damageFactor;
                onEnemyDamaged?.Invoke(damage, col.gameObject);
                canAttack = false;
                StartCoroutine(AttackCoolDown(coolDownTimer));
            }
        }
    }

    //Attack cool down timer for player
    IEnumerator AttackCoolDown(float time) { 
        yield return new WaitForSeconds(time);     // wait for time seconds until attacks register again
        canAttack = true;
    }
}
