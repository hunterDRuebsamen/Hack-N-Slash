using System;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField]
    private float damageFactor = 0.5f;
    [SerializeField]
    private string Name;
    private Rigidbody2D rb; 
    
    //Event for weapon hit
    public static event Action<float> onWeaponTriggerHit;

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
        }     
    }

}
