using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScirpt : MonoBehaviour
{
    //The rect of title text
    [SerializeField]
    private RectTransform titleTextTransform;
    //Where title text should end up
    [SerializeField]
    private float endYTitleText;
    //Starting Position of title text
    private Vector3 startingPositionTiltleText;
    //End position of title Text
    private Vector3 endPositionTitleText;
	//Each canvas menu
	[SerializeField]
	private GameObject mainMenuCanvas;
	[SerializeField]
	private GameObject CustomiseMenuCanvas;
	[SerializeField]
	private GameObject leaderBoardMenuCanvas;
	[SerializeField]
	private GameObject settingMenuCanvas;

    //Start method
    void Start()
    {
        //Set starting position and end position vectors
        startingPositionTiltleText = titleTextTransform.localPosition;
        endPositionTitleText = new Vector3(startingPositionTiltleText.x, endYTitleText, startingPositionTiltleText.z);
    }

    //Update method
    void FixedUpdate()
    {
        //If title text is not at end position lerp it towards it
       if(titleTextTransform.localPosition.y != endYTitleText)
       {
            titleTextTransform.localPosition = Vector3.Lerp(startingPositionTiltleText, endPositionTitleText, 1 * Time.fixedTime);
       } 
    }

    //Method for when the start game button is pressed
    public void StartGameButton()
    {
        SceneManager.LoadScene("game");
    }

	//Method for when the Customise button is pressed
	public void CustomiseButton()
	{
		mainMenuCanvas.SetActive(false);
		CustomiseMenuCanvas.SetActive(true);
	}

	//Method for when the leaderBoard button is pressed
	public void LeaderBoardButton()
	{
		mainMenuCanvas.SetActive(false);
		leaderBoardMenuCanvas.SetActive(true);
	}

	//Method for when ther settings button is pressed
	public void SettingButton()
	{
		mainMenuCanvas.SetActive(false);
		settingMenuCanvas.SetActive(true);
	}

	//Method for when the back button is pressed on the phone or in game
	public void BackButton()
	{
		mainMenuCanvas.SetActive(true);
		CustomiseMenuCanvas.SetActive(false);
		leaderBoardMenuCanvas.SetActive(false);
		settingMenuCanvas.SetActive(false);	
	}

	//Method for when the exit game button is pressed
	public void ExitGameButton()
    {
        Application.Quit();
    }
}
