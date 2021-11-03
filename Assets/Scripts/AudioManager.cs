using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GetComponent<AudioSource>();
        audioManager.volume = GlobalVariables.Get<float>("musicVolume");
    }

 
}
