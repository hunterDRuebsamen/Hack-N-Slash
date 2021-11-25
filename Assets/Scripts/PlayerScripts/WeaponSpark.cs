using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpark : MonoBehaviour
{
    [SerializeField, Tooltip("Primary particle system object, contains the spark between weapons")]
    ParticleSystem weaponSparks = null;
    [SerializeField, Tooltip("Pariticle system for the player's blood splatter")]
    ParticleSystem bloodSpark = null;
    [SerializeField, Tooltip("Pariticle system for the enemy's blood splatter")]
    ParticleSystem enemyBloodSpark = null;
    [SerializeField, Tooltip("Time that will occur between each weapon spark")]
    float timeBetweensparks = .5f;
    [SerializeField, Tooltip("Time that will occur between each blood splatter")]
    float timeBetweenSplat = .5f;
    [SerializeField, Tooltip("Reference to the main player object(used for player location on screen)")]
    GameObject player = null;
    
    private void Awake()
    {
        // make sure the the particle systems do not start running on game start
        weaponSparks.Stop();
        bloodSpark.Stop();
        enemyBloodSpark.Stop();
    }

    // allow the spark system to trigger during these specific events
    private void OnEnable()
    {
        EnemyBehavior.parriedEvent += spark;
        projectile.onProjecParry += spark;
        WeaponBase.onEnemyDamaged += enemySplat;
        EnemyBehavior.onPlayerDamaged += playerSplat;

    }
    private void OnDisable()
    {
        EnemyBehavior.parriedEvent -= spark;
        projectile.onProjecParry -= spark;
        WeaponBase.onEnemyDamaged -= enemySplat;
        EnemyBehavior.onPlayerDamaged -= playerSplat;
    }

        
    void spark(GameObject enemy)
    {
        Debug.Log("Weapon sparking should trigger");
        StartCoroutine( sparkEffect(enemy));
    }

    void enemySplat(float test, GameObject enemy)
    {
        Debug.Log("Splat should be triggered");
        StartCoroutine(bloodSplat(enemy));
    }
    void playerSplat(EnemyBehavior.AttackType y, float x)
    {
        Debug.Log("Splat should be triggered");
        StartCoroutine(bloodSplat());
    }
    
    //routine for blood splat for enemy
    IEnumerator bloodSplat(GameObject enemy)
    {
        //relocates the weaponSparks object to the enemy's position
        this.transform.localPosition = enemy.transform.localPosition;

        //Turns on the bloodSpark(for enemy) and waits some time before stopping it
        enemyBloodSpark.Play();
        yield return new WaitForSeconds(timeBetweensparks);
        enemyBloodSpark.Stop();
    }

    //routrine for blood splat for player
    IEnumerator bloodSplat()
    {
        //relocates the weaponSparks object to the enemy's position and changes the rotation of the object to point away from the player
        this.transform.localPosition = player.transform.localPosition;
        this.transform.RotateAround(weaponSparks.transform.position, Vector3.up, 180);

        //Turns on the bloodSpark(for player) and waits some time before stopping it
        bloodSpark.Play();
        yield return new WaitForSeconds(timeBetweenSplat);
        bloodSpark.Stop();

        // Waits another set interval before reverting the rotation to prevent the blood splatter to rotate in the process
        yield return new WaitForSeconds(timeBetweenSplat);     
        this.transform.RotateAround(weaponSparks.transform.position, Vector3.up, -180);
    }

    //routine for spark effects upon any parry event
    IEnumerator sparkEffect(GameObject enemy)
    {
        //relocates the weaponSparks object to the enemy's position
        this.transform.localPosition = enemy.transform.localPosition;

        //Turns on the weaponSparks system for a set interval to be turned off later
        weaponSparks.Play();
        yield return new WaitForSeconds(timeBetweensparks);
        weaponSparks.Stop();
    }
}
