using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dodge : MonoBehaviour
{
    public float MoveSpeed;
    public float NumberOfDodging = 0f;
    public bool canDodge = true;
    public float maxDodge = 3f;
    public float time;
    public float timeToReachTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator CoolDown() // This function stops the dodging after a specific number of times, then continues after some seconds
    {
        NumberOfDodging = 0f;                   // Number of times dodged is 0 before you start dodging
        canDodge = false;                       // canDodge is false so you can't dodge
        yield return new WaitForSeconds(3);     // wait for 3 seconds until you can dodge
        canDodge = true;                        // now you can dodge
    }

    IEnumerator MoveToPosition()
    {
        float t = 0f;
        float timeToReachTarget = 0.5f;
        float speedMultiplier = 10f;
        float MoveSpeed = 3f;

        while (t < timeToReachTarget)
        {
            transform.position += new Vector3(speedMultiplier*MoveSpeed, 0, 0) * Input.GetAxis("Horizontal") * Time.deltaTime;      // Do the dodging for the amount of time
            t += Time.deltaTime;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(MoveSpeed, 0, 0) * Input.GetAxis("Horizontal") * Time.deltaTime;                          // Make the player move right or left

        if (Input.GetButtonDown("Jump") && canDodge)                                                                                // Did you press space, and is canDodge true?
        {
            StartCoroutine(MoveToPosition());                                                                                       // If so, call the function MoveToPosition()

            NumberOfDodging++;                                                                                                      // Gets the number of times you pressed space or the number of times you dodged with the player

            if (NumberOfDodging == maxDodge)                                                                                        // If you dodged with the maximum amount of times you can
            {
                StartCoroutine(CoolDown());                                                                                         // Call the function CoolDown()
            }
        }
    }
  
}
