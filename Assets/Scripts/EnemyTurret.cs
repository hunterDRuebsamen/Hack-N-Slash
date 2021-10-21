using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    public float range;
    public Transform target;
    bool detected = false;
    Vector2 direction;
    public GameObject fireProjectile;
    public GameObject projectile;
    public float fireRate;
    float nextTimeToFire = 0;
    public Transform firePoint;
    public float force;

    // Update is called once per frame
    void Update()
    {
        Vector2 targetPosition = target.position;
        direction = targetPosition - (Vector2)transform.position;
        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, direction, range);

        if (rayInfo)
        {
            if (rayInfo.collider.gameObject.tag == "Player")
            {
                if (detected == false)
                {
                    detected = true;
                }
            }
            else
            {
                if (detected == true)
                {
                    detected = false;
                }
            }
        }

        if (detected)
        {
            fireProjectile.transform.up = direction;

            if (Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1 / fireRate;
                fire();
            }
        }
    }

    void fire()
    {
        GameObject BulletInstantiate = Instantiate(projectile, firePoint.position, Quaternion.identity);
        BulletInstantiate.GetComponent<Rigidbody2D>().AddForce(direction * force);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
