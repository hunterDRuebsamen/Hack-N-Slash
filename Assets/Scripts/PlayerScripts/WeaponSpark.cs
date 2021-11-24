using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpark : MonoBehaviour
{
    [SerializeField] ParticleSystem weaponSparks = null;
    [SerializeField] float timeBetweensparks = .5f;
    [SerializeField] GameObject player = null;

    private void Awake()
    {
        weaponSparks.Stop();
    }

    private void OnEnable()
    {
        EnemyBehavior.parriedEvent += spark;

    }

    private void OnDisable()
    {
        EnemyBehavior.parriedEvent += spark;
    }

        
    void spark(GameObject enemy)
    {
        StartCoroutine( sparkEffect(enemy));
    }

    IEnumerator sparkEffect(GameObject enemy)
    {
        weaponSparks.transform.localPosition = enemy.transform.localPosition;
        weaponSparks.Play();
        yield return new WaitForSeconds(timeBetweensparks);
        weaponSparks.Stop();
    }
}
