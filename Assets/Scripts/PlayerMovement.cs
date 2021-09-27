using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Tooltip("Max horizontal speed, in units per second, that the character moves.")]
    float horizontalSpeed = 0;

    [SerializeField, Tooltip("Max vertical speed, in units per second, that the character moves.")]
    float verticalSpeed = 0;
    
    [SerializeField, Tooltip("Dodge Speed, in units per second, that the character moves")]
    float dodgeSpeed = 10;
 
    [SerializeField, Tooltip("Acceleration while grounded.")]
    float acceleration = 75;
 
    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float deceleration = 70;

    [SerializeField, Tooltip("Number of times character can dodge before resetting")]
    public float maxDodge = 3f;

    float horizontalInput;
    float verticalInput;

    float NumberOfDodging = 0f;
    private bool canDodge = true;

    private CapsuleCollider2D capsuleCollider;
 
    private Vector2 velocity;

 
    private void Awake()
    {      
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }
 

    private void Update()
    {
        // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, capsuleCollider.size, 0);

        move(); // executes movement of the player
        collision(hits); // Applies Collisions to Kinematic Rigidbodies

        if (Input.GetButtonDown("Jump") && canDodge)  // Did you press space, and is canDodge true?                                                                              
        {
            Dodge(); // If so, call the function Dodge()
            NumberOfDodging++; // Gets the number of times you pressed space or the number of times you dodged with the player                                                                                                     

            if (NumberOfDodging == maxDodge) // If you dodged with the maximum amount of times you can
            {
                StartCoroutine(CoolDown(3f)); // Call the function CoolDown()
            }
        }     
    }

    private void move() {
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
    }

    private void collision (Collider2D[] hits) {
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


    /*
    This function freezes the character's ability to dodge after a given number of dodges. 
    then continues after an inputted number of seconds
    */
    IEnumerator CoolDown(float timeBetweenDodges) { 
        NumberOfDodging = 0f;                   // Number of times dodged is 0 before you start dodging
        canDodge = false;                       // canDodge is false so you can't dodge
        Debug.Log("canDodge = false");
        yield return new WaitForSeconds(timeBetweenDodges);     // wait for 3 seconds until you can dodge
        canDodge = true;                        // now you can dodge
    }

    void Dodge() {
        if (horizontalInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, dodgeSpeed * horizontalInput, acceleration * Time.deltaTime);
            Debug.Log("Dodge horizontal");
        }
        if (verticalInput != 0) 
        {
            velocity.y = Mathf.MoveTowards(velocity.y, dodgeSpeed * verticalInput, acceleration * Time.deltaTime);
            Debug.Log("Dodge Vertical");
        }
        transform.Translate(velocity * Time.deltaTime);
        }
        
}