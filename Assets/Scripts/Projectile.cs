using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    IEnumerator OnCollisionEnter2D(Collision2D disappear)
    {
        yield return new WaitForSeconds(4);
        Destroy(this.gameObject);
    } 
}
