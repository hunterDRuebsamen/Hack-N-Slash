using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public delegate void leavePause();
    public static event leavePause onUnpause;


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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            onUnpause();
            Resume();           
        }
    }
}
