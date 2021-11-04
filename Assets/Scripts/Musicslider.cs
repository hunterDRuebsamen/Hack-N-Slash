using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Musicslider : MonoBehaviour
{
    [SerializeField]
    private float defaultMusicVolume = 0.5f;
    private Slider volumeSlider;
 
    void Start()
    {
        // Get the slider component
        volumeSlider = GetComponent<Slider>();

        // if the key doesn't exist, create it
        if(!GlobalVariables.HasKey("musicVolume"))
        {
            GlobalVariables.Set("musicVolume", defaultMusicVolume);
            volumeSlider.value = defaultMusicVolume;
        }
        else
        {
            // Grab the music Volume from the GlobalConfiguration
            volumeSlider.value = GlobalVariables.Get<float>("musicVolume");
        }

        // Add a listener to this slider.onValueChanged to invoke ChangeVolume() when the value changes
        volumeSlider.onValueChanged.AddListener(delegate {ChangeVolume(); });
    }
    public void ChangeVolume()
    {
        GlobalVariables.Set("musicVolume", volumeSlider.value);
    }
}
