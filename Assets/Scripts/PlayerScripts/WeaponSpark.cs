using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpark : MonoBehaviour
{
    [SerializeField] ParticleSystem weaponSparks = null;
    [SerializeField] ParticleSystem bloodSpark = null;
    [SerializeField] float timeBetweensparks = .5f;
    [SerializeField] float timeBetweenSplats = 1f;
    [SerializeField] GameObject player = null;
    
    private void Awake()
    {
        weaponSparks.Stop();
    }

    private void OnEnable()
    {
        EnemyBehavior.parriedEvent += spark;
        WeaponBase.onEnemyDamaged += enemySplat;
        PlayerHealth.onPlayerHealthChanged += playerSplat;

    }

    private void OnDisable()
    {
        EnemyBehavior.parriedEvent += spark;
        WeaponBase.onEnemyDamaged -= enemySplat;
        PlayerHealth.onPlayerHealthChanged -= playerSplat;
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
    void playerSplat(int test)
    {
        Debug.Log("Splat should be triggered");
        StartCoroutine(bloodSplat());
    }

    IEnumerator bloodSplat(GameObject enemy)
    {
        weaponSparks.transform.localPosition = enemy.transform.localPosition;
        bloodSpark.Play();
        yield return new WaitForSeconds(timeBetweensparks);
        bloodSpark.Stop();
    }
    IEnumerator bloodSplat()
    {
        weaponSparks.transform.localPosition = player.transform.localPosition;
        Quaternion rotate_particle = weaponSparks.transform.localRotation;
        //rotate_particle.y *= -1;
        //weaponSparks.transform.localRotation = rotate_particle;
        bloodSpark.Play();
        yield return new WaitForSeconds(timeBetweenSplats);
        //rotate_particle.y *= -1;
        //weaponSparks.transform.localRotation = rotate_particle;
        bloodSpark.Stop();
    }
    IEnumerator sparkEffect(GameObject enemy)
    {
        weaponSparks.transform.localPosition = enemy.transform.localPosition;
        weaponSparks.Play();
        yield return new WaitForSeconds(timeBetweensparks);
        weaponSparks.Stop();
    }
}
