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
        if(other.name == "PlayerV2" || other.name == "Arm_02")
        { 
            if(currentObject == PickupObject.POTION)
            {
                PlayerHealth.currentHealth += 20;
 
                //PlayerHealth.healthBar.SetHealth(PlayerHealth.currentHealth);
 
                Debug.Log("Player is healed");
 
                Debug.Log("Player Health: " + PlayerHealth.currentHealth);
            }
            Destroy(gameObject);
        }
    }
}
