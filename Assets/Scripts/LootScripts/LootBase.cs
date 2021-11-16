using System;
using UnityEngine;

public class LootBase : MonoBehaviour
{
    [SerializeField, Tooltip("how much health or points to apply")] int value;

    [SerializeField] float spawnRate = 0.10f;

    private GameObject playerGO;
    private Score score;

    public static event Action<LootType, int> onLootPickup;

    public enum LootType
    {
        Coin,
        Potion,
        Weapon
    };

    [SerializeField] LootType type;

    void OnTriggerEnter2D(Collider2D other)
    { //coin is counted in counter and is then destroyed
        print("Collided with: " + other);
        if(other.tag == "Player")
        { 
            onLootPickup?.Invoke(type, value);
            Destroy(gameObject);
        }
    }
}