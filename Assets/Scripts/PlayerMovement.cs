using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] 
    int movementSpeed = 1;

    private float horizontal;
    private float vertical;

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontal * movementSpeed, vertical * movementSpeed, 0.0f);
        transform.position = transform.position + movement * Time.deltaTime;
    }

}
