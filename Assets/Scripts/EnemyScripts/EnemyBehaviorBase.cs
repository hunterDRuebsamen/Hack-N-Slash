using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyBehaviorBase : MonoBehaviour
{
    protected virtual void Move()
    {}

    public virtual void Attack()
    {}

    public virtual void Shoot()
    {}
}