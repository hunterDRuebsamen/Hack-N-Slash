using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpark : MonoBehaviour
{
    [SerializeField] ParticleSystem weaponSparks = null;
    [SerializeField] float timeBetweensparks = .5f;

    private void Awake()
    {
        weaponSparks.Stop();
    }

    private void OnEnable()
    {
        WeaponBase.onEnemyDamaged += spark;
    }

    private void OnDisable()
    {
        WeaponBase.onEnemyDamaged -= spark;
    }

    void spark(float dmg, GameObject enemy)
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
