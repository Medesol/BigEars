using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("Level0-Scene");
        Debug.Log("Game Start");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SelectLevel(int i)
    {
        SceneManager.LoadSceneAsync(i);
    }
    public void BackHome()
    {
        SceneManager.LoadScene("StartMenu-Scene");
        Debug.Log("Back Home");
    }
}