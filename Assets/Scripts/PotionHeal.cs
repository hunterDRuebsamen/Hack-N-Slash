using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PotionHeal : MonoBehaviour
{
    public enum PickupObject{POTION};
    public PickupObject currentObject;
    public PlayerHealth playerHealthObject;
    public HealthBar healthBar;

    private int fullHealth = 100;
    private int health;

    void OnTriggerEnter2D(Collider2D other)
    {
        health = GameObject.Find("PlayerV4").GetComponent<PlayerHealth>().getHealth();

        healthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBar>();

        if(other.tag == "Player" || other.tag == "Weapon")
        {            
            if(health != fullHealth && health < 100)
            {
                health += 20;
                healthBar.SetMaxHealth(health);
                Debug.Log("Player is healed");
                Debug.Log("Player Health: " + health);
            }
            else
            {
                health = 100;
                Debug.Log("Player health full");
            }

            if(health > 100)
            {
                health = 100;
                Debug.Log("Player Health: " + health);
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
