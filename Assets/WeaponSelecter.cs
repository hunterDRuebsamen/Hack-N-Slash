using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelecter : MonoBehaviour
{
    // Start is called before the first frame update
    private float defaultWeapon = 1;
    private Button weaponButton;
    void Start()
    {
        if(!GlobalVariables.HasKey("weaponValue"))
        {
            GlobalVariables.Set("weaponValue", defaultWeapon);
        }
        weaponButton.onClick.AddListener(ButtonClicked);
    }

    void ButtonClicked() {
        if(weaponButton.name == "WeaponSelectOption1")
            GlobalVariables.Set("weaponValue", 1);
        if(weaponButton.name == "WeaponSelectOption2")
            GlobalVariables.Set("weaponValue", 2);
        if(weaponButton.name == "WeaponSelectOption3")
            GlobalVariables.Set("weaponValue", 3);
        if(weaponButton.name == "WeaponSelectOption4")
            GlobalVariables.Set("weaponValue", 4);
        Debug.Log(weaponButton.name);
    }


}
