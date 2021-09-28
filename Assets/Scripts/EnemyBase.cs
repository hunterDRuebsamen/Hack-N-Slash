using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField, Tooltip("Enemy Health (hitpoints)")]
    int health = 15;
    [SerializeField, Tooltip("Knockback Force")]
    float knockback = 0.6f;

    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // We use OnTriggerEnter2D instead of OnCollisionEnter2D because the player hand is set to "isTrigger"
    private void OnTriggerEnter2D(Collider2D col) 
    {
        // check to see what we just got hit with
        if (col.tag == "Weapon")
        {
            // add a knockback effect
            StartCoroutine(FakeAddForceMotion(knockback));

            // take damage (perhaps based on weapon type)
            
        }     
    }

    // This function adds a fake force to a Kinematic body
    IEnumerator FakeAddForceMotion(float forceAmount)
    {
        float i = 0.01f;
        while (forceAmount > i)
        {
            rigidBody.velocity = new Vector2(forceAmount / i, rigidBody.velocity.y); // !! For X axis positive force
            i = i + Time.deltaTime;
            yield return new WaitForEndOfFrame();      
        }
        rigidBody.velocity = Vector2.zero;
        yield return null;
    }
}
