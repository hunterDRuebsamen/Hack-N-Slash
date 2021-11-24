using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpark : MonoBehaviour
{
    [SerializeField] ParticleSystem weaponSparks = null;
    [SerializeField] ParticleSystem bloodSpark = null;
    [SerializeField] ParticleSystem enemyBloodSpark = null;
    [SerializeField] float timeBetweensparks = .5f;
    [SerializeField] float timeBetweenSplat = .5f;
    [SerializeField] GameObject player = null;
    
    private void Awake()
    {
        weaponSparks.Stop();
    }

    private void OnEnable()
    {
        EnemyBehavior.parriedEvent += spark;
        WeaponBase.onEnemyDamaged += enemySplat;
        EnemyBehavior.onPlayerDamaged += playerSplat;

    }

    private void OnDisable()
    {
        EnemyBehavior.parriedEvent += spark;
        WeaponBase.onEnemyDamaged -= enemySplat;
        EnemyBehavior.onPlayerDamaged -= playerSplat;
    }

        
    void spark(GameObject enemy)
    {
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

    IEnumerator bloodSplat(GameObject enemy)
    {
        weaponSparks.transform.localPosition = enemy.transform.localPosition;
        enemyBloodSpark.Play();
        yield return new WaitForSeconds(timeBetweensparks);
        enemyBloodSpark.Stop();
    }
    IEnumerator bloodSplat()
    {
        weaponSparks.transform.localPosition = player.transform.localPosition;
        weaponSparks.transform.RotateAround(weaponSparks.transform.position, Vector3.up, 180);

        bloodSpark.Play();
        yield return new WaitForSeconds(timeBetweenSplat);
        bloodSpark.Stop();
        yield return new WaitForSeconds(timeBetweenSplat);     
        weaponSparks.transform.RotateAround(weaponSparks.transform.position, Vector3.up, -180);
    }
    IEnumerator sparkEffect(GameObject enemy)
    {
        weaponSparks.transform.localPosition = enemy.transform.localPosition;
        weaponSparks.Play();
        yield return new WaitForSeconds(timeBetweensparks);
        weaponSparks.Stop();
    }
}
