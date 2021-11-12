using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PotionHeal : MonoBehaviour
{
    public enum PickupObject{POTION};
    public PickupObject currentObject;
    public PlayerHealth playerHealthObject;
 
    void OnTriggerEnter2D(Collider2D other)
    {
        print(other);
        if(other.tag == "Player" || other.tag == "Weapon")
        { 
            Debug.Log("Player is healed");

            if(playerHealthObject.currentHealth == playerHealthObject.maxHealth)
            {
                playerHealthObject.currentHealth = 100;
            }
            else
            {
                Debug.Log("Player is healed");

                playerHealthObject.currentHealth += 20;
            
                //playerHealthObject.healthBar.SetHealth(playerHealthObject.currentHealth);

                Debug.Log("Player Health: " + playerHealthObject.currentHealth);
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
