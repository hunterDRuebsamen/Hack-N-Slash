using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioManager;
    private const float defaultMusicVolume = 0.25f;
    
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GetComponent<AudioSource>();
        if (GlobalVariables.HasKey("musicVolume")) {
            audioManager.volume = GlobalVariables.Get<float>("musicVolume");
        } else {
            audioManager.volume = defaultMusicVolume;
            GlobalVariables.Set("musicVolume", defaultMusicVolume);
        }
    }
    
    void onEnable (){
        PauseMenu.onUnpause+=changeMusicvol;

    }

    void onDisable (){
        PauseMenu.onUnpause-=changeMusicvol;
    }

    void changeMusicvol(){
        audioManager.volume = GlobalVariables.Get<float>("musicVolume");
    }
    


}
