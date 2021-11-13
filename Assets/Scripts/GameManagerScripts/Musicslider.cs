using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Musicslider : MonoBehaviour
{
    [SerializeField]
    private float defaultMusicVolume = 0.2f;
    private Slider volumeSlider;
    private AudioSource backgroundMusic;
 
    void Start()
    {
        // Get the slider component
        volumeSlider = GameObject.Find("Slider").GetComponent<Slider>();
        // Get the background music audio source
        backgroundMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();

        // if the key doesn't exist, create it
        if(!GlobalVariables.HasKey("musicVolume"))
        {
            GlobalVariables.Set("musicVolume", defaultMusicVolume);
            volumeSlider.value = defaultMusicVolume;
            backgroundMusic.volume = defaultMusicVolume;
        }
        else
        {
            // Grab the music Volume from the GlobalConfiguration
            volumeSlider.value = GlobalVariables.Get<float>("musicVolume");
            backgroundMusic.volume = volumeSlider.value;
        }

        // Add a listener to this slider.onValueChanged to invoke ChangeVolume() when the value changes
        volumeSlider.onValueChanged.AddListener(delegate {ChangeVolume(); });
    }
    public void ChangeVolume()
    {
        GlobalVariables.Set("musicVolume", volumeSlider.value);
        backgroundMusic.volume = volumeSlider.value;
    }
}
