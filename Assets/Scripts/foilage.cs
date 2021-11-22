using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foilage : MonoBehaviour
{
    Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    void OnTriggerEnter2D(Collider2D other)
    { //coin is counted in counter and is then destroyed
        if(other.tag == "Player")
        {
            anim.Play(); 
        }
    }
}
