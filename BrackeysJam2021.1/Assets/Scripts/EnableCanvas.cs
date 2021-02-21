using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnableCanvas : MonoBehaviour
{
    [SerializeField] private GameObject canvasToBringUp = null;
    [SerializeField] private string menuSceneName = "MenuScene";

    public void EnableThisCanvas()
    {
        canvasToBringUp.SetActive(true);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
