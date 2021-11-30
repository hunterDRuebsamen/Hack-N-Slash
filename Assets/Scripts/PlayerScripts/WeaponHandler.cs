using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    // Start is called before the first frame update
    //List<Transform> allChildren = new List<Transform>();
    Component[] allChildren;
    Transform child;
    List<GameObject> weaponObjects = new List<GameObject>();

    HingeJoint2D arm;
    void Start()
    {
        

        allChildren = GetComponentsInChildren<Transform>();
        child = this.gameObject.transform;
        arm = this.gameObject.transform.parent.GetChild(1).GetComponent<HingeJoint2D>();
        //Stores all possible weapon children inside an array
        for(int i = 0; i < 4; i++) {
            if(child.GetChild(i).gameObject.activeSelf)
                weaponObjects.Add(child.GetChild(i).gameObject);
        }
        // foreach (Transform child in allChildren)
        // { 
        //     if(child != null)
        //         weaponObjects.Add(child.gameObject);
        // }
        arm.connectedBody = weaponObjects[0].GetComponent<Rigidbody2D>();
        
        

    }
}
