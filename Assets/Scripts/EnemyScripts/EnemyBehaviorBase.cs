using System;
using System.Collections;
using UnityEngine;

public class EnemyBehaviorBase : MonoBehaviour
{
    protected virtual void Move()
    {}

    public virtual void Attack()
    {}
}