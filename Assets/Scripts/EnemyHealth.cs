using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    
    public int maxHealth = 15;
    public int currentHealth;

    public EnemyHealthbar EnemyHealthbar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        EnemyHealthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    //void Update()
    /*{
        if (Input.GetKeyDown("y"))
        {
            TakeDamage(3);
        }
    }*/

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        EnemyHealthbar.SetHealth(currentHealth);
    }
}