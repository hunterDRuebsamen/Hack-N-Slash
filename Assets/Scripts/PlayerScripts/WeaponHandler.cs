using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    // Start is called before the first frame update
    Transform[] allChildren = this.gameObject.GetComponentsInChildren<Transform>();
    List<GameObject> weaponObject = new List<GameObject>();
    HingeJoint2D arm;
    void Start()
    {
        arm = this.gameObject.transform.parent.GetChild(1).GetComponent<HingeJoint2D>();
        //Stores all possible weapon children inside an array
        foreach (Transform child in allChildren)
        { 
            weaponObject.Add(child.gameObject);
        }
        
        

    }
}
