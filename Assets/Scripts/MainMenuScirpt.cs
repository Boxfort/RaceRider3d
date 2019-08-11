using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScirpt : MonoBehaviour
{

    void Start()
    {

    }

    //Method for when the start game button is pressed
    public void StartGameButton()
    {
        SceneManager.LoadScene("game");
    }

    //Method for when the exit game button is pressed
    public void ExitGameButton()
    {
        Application.Quit();
    }
}
