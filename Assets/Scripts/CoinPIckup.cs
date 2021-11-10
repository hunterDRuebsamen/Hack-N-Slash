using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CoinPickup : MonoBehaviour
{
    public enum PickupObject{COIN};
    public PickupObject currentObject;
    
    public int pickupQuantity = 1;
    public int coins = 1;
 
    void OnTriggerEnter2D(Collider2D other)
    { //coin is counted in counter and is then destroyed
        print("Collided with: " + other);
        if(other.name == "PlayerV2" || other.name == "Arm_02")
        { 
            if(currentObject == PickupObject.COIN)
            {
                coins += pickupQuantity;
                CoinCounter.coinAmount += 1;
                Debug.Log("Coins: " + coins);
            }
            Destroy(gameObject);
        }
    }
}
