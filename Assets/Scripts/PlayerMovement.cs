using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Tooltip("Max horizontal speed, in units per second, that the character moves.")]
    float horizontalSpeed = 9;

    [SerializeField, Tooltip("Max vertical speed, in units per second, that the character moves.")]
    float verticalSpeed = 9;
 
 
    [SerializeField, Tooltip("Acceleration while grounded.")]
    float acceleration = 75;
 
    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float deceleration = 70;
 
    private CapsuleCollider2D capsuleCollider;
 
    private Vector2 velocity;
 
    private void Awake()
    {      
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }
 
    private void Update()
    {
        // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
 
 
        if (horizontalInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, horizontalSpeed * horizontalInput, acceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        }

        if (verticalInput != 0) 
        {
            velocity.y = Mathf.MoveTowards(velocity.y, verticalSpeed * verticalInput, acceleration * Time.deltaTime);
        }
        else 
        {
            velocity.y = Mathf.MoveTowards(velocity.y, 0, deceleration * Time.deltaTime);
        }
 
        transform.Translate(velocity * Time.deltaTime);
 
        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, capsuleCollider.size, 0);
 
        foreach (Collider2D hit in hits)
        {
            // Ignore our own collider.
            if (hit.tag == "Player")
                continue;
 
            ColliderDistance2D colliderDistance = hit.Distance(capsuleCollider);
 
            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider
            // pushing us out of this one.
            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
            }
        }
    }
}