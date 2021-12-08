using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour
{
    private GameObject loseMenuUI;

    void Start()
    {
        loseMenuUI = this.gameObject.transform.GetChild(0).gameObject;
        loseMenuUI.SetActive(false);
    }

    private void OnEnable() { // Watches for when the enemy gets hit
        PlayerHealth.onPlayerDeath += Lose;
    } 
    private void OnDisable() {
        PlayerHealth.onPlayerDeath -= Lose;
    }

    void Lose()
    {
        loseMenuUI.SetActive(true);
        Time.timeScale = 0;
        StartCoroutine(LostTheGame());
    }

    IEnumerator LostTheGame()
    {
        yield return new WaitForSecondsRealtime(5);
        SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);
        Time.timeScale = 1;
    }
}
