using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScirpt : MonoBehaviour
{
    public void StartGameButton()
    {
        SceneManager.LoadScene("game");
    }
}
