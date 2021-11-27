using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static event Action onUnpause;
    
    public GameObject pauseMenuUI;
    bool deadPlayer = false;
    private void OnEnable(){
        PlayerHealth.onPlayerDeath+=playerdeath;
    }

    private void OnDisable(){
        PlayerHealth.onPlayerDeath-=playerdeath;
    }
    private void playerdeath (){
        deadPlayer = true; 
    }
    void Resume()
    {
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
    }

    void Pause()
    {
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !deadPlayer)
        {
            Pause();
        }
        else if(Input.GetKeyDown(KeyCode.Space) && !deadPlayer)
        {
           
            Resume(); 
            if (Input.GetKeyDown(KeyCode.Space)){
                onUnpause?.Invoke();      
            }     

                 
        }
    }
}
