using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictCollider : MonoBehaviour
{
    [SerializeField, Tooltip("Restricts the movement of the collider to this y-axis")]
    float y_axis = 0;

    // Update is called once per frame and will restrict the movement of the collider
    void Update()
    {
        transform.position = new Vector2(transform.position.x, y_axis);
    }
}
