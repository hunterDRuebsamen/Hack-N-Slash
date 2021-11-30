using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
    // Axe = 0
    // Kama = 1
    // Katana = 2
    // Sword (Dull) = 3
    Dropdown m_Dropdown;
    // Start is called before the first frame update
    void Start()
    {
        m_Dropdown = GetComponent<Dropdown>();

        GlobalVariables.Set("WeaponType", 0); // default to Axe

        // add listenser for when the value of the Dropdown changes, to take action
        m_Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(m_Dropdown);
        });
    }

    // output new value of the Dropdown
    void DropdownValueChanged(Dropdown change) {
        GlobalVariables.Set("WeaponType", change.value);
    }
}
