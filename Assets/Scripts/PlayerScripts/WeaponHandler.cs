using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    // Start is called before the first frame update
    //List<Transform> allChildren = new List<Transform>();
    Component[] allChildren;
    Transform child;

    [SerializeField]
    List<GameObject> weaponObjects;

    HingeJoint2D arm;

    void Awake()
    {   
        arm = this.gameObject.transform.parent.GetChild(1).GetComponent<HingeJoint2D>();

        int curWeapon = 0; // default weapon
        if (GlobalVariables.HasKey("WeaponType")) 
            curWeapon = GlobalVariables.Get<int>("WeaponType");

        // check to see we don't try and choose a weapon not in the list
        if (curWeapon < weaponObjects.Count) {
            GameObject weapon = Instantiate(weaponObjects[curWeapon]);
            weapon.transform.parent = transform;  // set this gameObject (Weapon) to be the parent
            arm.connectedBody = weapon.GetComponent<Rigidbody2D>();
        }
        else  // something went wrong
            Debug.LogAssertion("Error: Weapon does not exist");
    }
}
