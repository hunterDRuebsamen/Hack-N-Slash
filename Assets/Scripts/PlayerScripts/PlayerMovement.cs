using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Tooltip("Max horizontal speed, in units per second, that the character moves.")]
    float horizontalSpeed = 3;
    [SerializeField, Tooltip("Max vertical speed, in units per second, that the character moves.")]
    float verticalSpeed = 3;
    [SerializeField, Tooltip("Dodge Speed Factor, how many times faster is a dodge move")]
    float dodgeFactor = 3.5f;
    [SerializeField, Tooltip("Acceleration while grounded.")]
    float acceleration = 75;
    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float deceleration = 70;

    [SerializeField, Tooltip("Limits the postiive vertical distance that the character can travel.")]
    public float maxY = 4f;
    [SerializeField, Tooltip("Limits the negative vertical distance that the character can travel.")]
    public float minY = 2.5f;

    [SerializeField, Tooltip("Cooldown Timer before you get your dodge boosts back")]
    float dodgeCoolDownTime = 10f;
    [SerializeField, Tooltip("Max Number of Dodge Boosts")]
    int maxDodgeBoosts = 3;
    [SerializeField, Tooltip("Length in Time a Dodge boost lasts")]
    public float dodgeLength = 0.5f;

    float horizontalInput;
    float verticalInput;
 
    private bool canDodge = true;
    private bool isDodging = false;
    private float numDodgeLeft;  // how many dodge boosts are left  
    private float coolDownTimer = 0f;
    private int lastMove = 0;

    private HingeJoint2D Arm_02;
    private HingeJoint2D Arm_01;
    private Animator playerAnim;
    private Transform healthBar;
   
 
    private float mulFactor = 1;
    private CapsuleCollider2D capsuleCollider;
    private Vector2 velocity;
 
    private void Awake()
    {
        Arm_01 = transform.GetChild(0).GetComponent<HingeJoint2D>();
        Arm_02 = transform.GetChild(1).GetComponent<HingeJoint2D>();       
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        numDodgeLeft = maxDodgeBoosts;

        healthBar = transform.GetChild(3).GetChild(0);
        playerAnim = transform.GetChild(4).GetComponent<Animator>();
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
 
        // Dodge Cooldown Timer
        if ((numDodgeLeft < maxDodgeBoosts) && !isDodging) {
            coolDownTimer += Time.deltaTime;
 
            if (coolDownTimer >= dodgeCoolDownTime) {
                // we have cooled down, reset the number of dodges to max
                numDodgeLeft = maxDodgeBoosts;
                canDodge = true;
            }
        }
    }
 
    private void move() {
        if (Input.GetButtonDown("Jump") && canDodge) 
        {
            numDodgeLeft--;
            coolDownTimer = 0; // reset cooldown timer on dodge
            StartCoroutine(DodgeRoutine(dodgeLength));
        }

        //Handles flip case for "a" inputs
        if(Input.GetKeyDown("a"))
        {
            StartCoroutine(flipRoutine(0.5f, -1));
        }
        //Handles flip case for "d" inputs
        else if(Input.GetKeyDown("d"))
        {
            StartCoroutine(flipRoutine(0.5f, 1));
        }
 
        if (horizontalInput != 0)
        {
            playerAnim.SetBool("isWalking", true);
            //Calculate x component of velocity with an acceleration rate when pressing "A" or "D"
            velocity.x = Mathf.MoveTowards(velocity.x, horizontalSpeed * mulFactor * horizontalInput, acceleration * Time.deltaTime);

            // registers "A" or "D" presses to check if player is trying to flip their character
        }
        else
        {
            //Deccelerates x component of velocity when no longer pressing "A" or "D"
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        }
        if (verticalInput != 0) 
        {
            playerAnim.SetBool("isWalking", true);
            // This will keep the player's movement within specified y bounds (courdicy of @courtesy)
            if (verticalInput > 0)
            {
                //checks if the player has moved too far up and prevents any more lateral movement when they do 
                if (transform.position.y > maxY)
                {
                    verticalInput = 0;
                }

            }
            else if (verticalInput < 0)
            {
                // checks if the player has moved too far up and prevents any more lateral movement when they do
                if (transform.position.y < minY)
                {
                   verticalInput = 0;
                }
            }
            velocity.y = Mathf.MoveTowards(velocity.y, verticalSpeed * mulFactor * verticalInput, acceleration * Time.deltaTime);
        }
        else 
        {
            //Deccelerates x component of velocity when no longer pressing "A" or "D"
            velocity.y = Mathf.MoveTowards(velocity.y, 0, deceleration * Time.deltaTime);
        }
        if (horizontalInput == 0 && verticalInput == 0) {
            playerAnim.SetBool("isWalking", false);
        }
        //Moves our player by the velocity vector we have calculated multiplied by the amount of time that has elasped
        //to get the total units that should be moved
        transform.Translate(velocity * Time.deltaTime);
    }
 
    private void collision (Collider2D[] hits) {
         foreach (Collider2D hit in hits)
        {
            // Ignore our own collider.
            if (hit.tag == "Player" || hit.tag == "Weapon" || hit.tag == "EnemyWeapon" || hit.tag == "Loot")
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


    void characterFlip(int direction)
    {
        //Joint limit object populated with current Arm limit values
        JointAngleLimits2D limit_01 = Arm_01.limits;
        JointAngleLimits2D limit_02 = Arm_02.limits;

        if (direction == -1) //Character Flip to left
        {
            Vector3 charScale = transform.localScale;
            charScale.x = -1;
            transform.localScale = charScale;

            //Changes the limits of the bicep arm
            limit_01.min = -70;
            limit_01.max = 70;
            Arm_01.limits = limit_01;

            //Changes the limits of the forearm arm
            limit_02.min = 0;
            limit_02.max = 90;
            Arm_02.limits = limit_02;

            //Keeps the health bar in the correct direction
            healthBar.localScale = new Vector3(-1,1,1);
        }
        else if (direction == 1) //Character Flip to right
        {
            Vector3 charScale = transform.localScale;
            charScale.x = 1;
            transform.localScale = charScale;
            
            //Changes the limits of the bicep arm
            limit_01.min = -50;
            limit_01.max = 75;
            Arm_01.limits = limit_01;

            //Changes the limits of the forearm arm
            limit_02.min = 270;
            limit_02.max = 360;
            Arm_02.limits = limit_02;

            //Keeps the health bar in the correct direction
            healthBar.localScale = new Vector3(1,1,1);
        }

    }

 
    /*
    This function freezes the character's ability to dodge after a given number of dodges. 
    then continues after an inputted number of seconds
    */
    IEnumerator DodgeRoutine(float time) { 
        canDodge = false;                       // canDodge is false so you can't dodge
        Debug.Log("canDodge = false");
        mulFactor = dodgeFactor;                //Changes speed multiplier when dodging
        yield return new WaitForSeconds(time);     // wait for 3 seconds until you can dodge
        mulFactor = 1.0f;                       //Resets speed multiplier to base speed  
        if (numDodgeLeft > 0)
            canDodge = true;                        // now you can dodge
    }

    /*
    This Coroutine stores the direction of the last move within a variable and keeps a timer of the time elasped between the last input.
    When the last move equals the current input of the user, flipRoutine calls the characterFlip function 
    */
    IEnumerator flipRoutine(float time, int direction) { 
        if (lastMove == direction) {
            characterFlip(direction);
        }
        lastMove = direction;
        yield return new WaitForSeconds(time);     // wait designated time 
        lastMove = 0;
    }    
}