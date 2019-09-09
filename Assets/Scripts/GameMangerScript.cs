using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMangerScript : MonoBehaviour
{

	//Road Section  object
	[SerializeField]
	private GameObject road;
	//Car object, already placed in the scene
	private GameObject car;
	private Rigidbody carRB;
	[SerializeField]
	private GameObject[] foliage;
	[SerializeField]
	private GameObject[] forestObstacles;
	[SerializeField]
	private GameObject[] roadObstacles;
	//Tile size
	[SerializeField]
	private float tileSize = 400f;
	//Level Size By Tile
	[SerializeField]
	private int levelSizeByTile = 10;
	private int levelCounter = 0;
	//Tree Decrease per level
	[SerializeField]
	private float obstaclesIncreasePerLevel = 0.1f;
	//Level score text UI component and the player level score
	[SerializeField]
	private GameObject LevelScoreText;
	private float levelScore;
	//EndGame Canvas which is displayed at a game over and gameEnded is set to true, to stop generation
	[SerializeField]
	private GameObject endGameCanvas;
	private bool gameEnded = false;
	//End gold and score texts
	[SerializeField]
	private UnityEngine.UI.Text endGoldText;
	[SerializeField]
	private UnityEngine.UI.Text endScoreText;
	//Car spawn transform
	[SerializeField]
	private Transform carSpawnTransform;
	//Cars gameobject
	[SerializeField]
	private GameObject normalCar;
	[SerializeField]
	private GameObject suvCar;
	[SerializeField]
	private GameObject taxiCar;
	[SerializeField]
	private GameObject sportsCar;
	[SerializeField]
	private GameObject copCar;
	//CountDown gameobject And images
	[SerializeField]
	private GameObject countDownGameObject;
	[SerializeField]
	private Sprite countDownImage1;
	[SerializeField]
	private Sprite countDownImage2;
	[SerializeField]
	private Sprite countDownImage3;
	private float countDownTimer = 3f;
	//Image to turn black
	[SerializeField]
	private UnityEngine.UI.Image blackScreen;
	//Random int
	private int rand;
	private int rand2;
	//Timer
	private float timer;
	//Speed
	private float speed = 100f;
	private float speedIncrease = 10f;
	//Tree gen amount
	private float obstaclesPerSecond = 0.4f;
	//Position of new Road placements
	private Vector3 roadGenPos = new Vector3(0, 0, 0);
	//Sections list
	private GameObject[] roadPieces = new GameObject[3];
	[SerializeField]
	private GameObject section;
	//Forest obstacles list
	private List<GameObject> forestObstaclesList = new List<GameObject>();
	//Foilage list
	private List<GameObject> foliageObjectList = new List<GameObject>();
	//Road Obstacles list
	private List<GameObject> roadObstaclesList = new List<GameObject>();
	//Ready and go auido sources and files
	[SerializeField]
	private AudioSource countDownAudioSource;
	[SerializeField]
	private AudioClip readyClip;
	[SerializeField]
	private AudioClip goClip;
	//Triple counter
	private bool passedFirstRoad = false;


	// Start is called before the first frame update
	//Start function
	void Start()
	{
		//Set level score
		LevelScoreText.GetComponent<UnityEngine.UI.Text>().text = levelScore.ToString();

		//Load in foliage
		for (int i = 0; i < 75; i++)
		{
			foliageObjectList.Add(Instantiate(foliage[Random.Range(0, foliage.Length)], Vector3.zero, Quaternion.identity));
		}
		//Load in trees
		for (int i = 0; i < 30; i++)
		{
			forestObstaclesList.Add(Instantiate(forestObstacles[Random.Range(0, forestObstacles.Length)], new Vector3(100, 100, -100), Quaternion.Euler(-90, 0, 0)));
		}
		//Load in rocks
		for (int i = 0; i < 5; i++)
		{
			roadObstaclesList.Add(Instantiate(roadObstacles[Random.Range(0, roadObstacles.Length)], new Vector3(100, 100, -100), Quaternion.identity));
		}
		//Create first road pieces
		//Road 1 + foliage + trees
		roadPieces[0] = Instantiate(section, new Vector3(0, 0, 0), Quaternion.identity);
		MoveFoliageUp(0, (foliageObjectList.Count - 1) / 3, 0, tileSize * 3);
		MoveTreesUp(0, (forestObstaclesList.Count - 1) / 3, 0, tileSize * 3);
		MoveRocksUp(0, (roadObstaclesList.Count - 1) / 3, 0, tileSize * 3);
		//Road 2 + foliage + trees
		roadPieces[1] = Instantiate(section, new Vector3(0, 0, tileSize), Quaternion.identity);
		//Road 3 + foliage + trees
		roadPieces[2] = Instantiate(section, new Vector3(0, 0, tileSize * 2), Quaternion.identity);


		//Set fade to black image
		blackScreen = GameObject.Find("FadeImage").GetComponent<UnityEngine.UI.Image>();

		//Assing the main car object in the scene
		if (MainMenuScirpt.selectedCar == 0)
		{
			car = Instantiate(normalCar, carSpawnTransform.position, carSpawnTransform.rotation);
		}
		else if (MainMenuScirpt.selectedCar == 1)
		{
			car = Instantiate(suvCar, carSpawnTransform.position, carSpawnTransform.rotation);
		}
		else if (MainMenuScirpt.selectedCar == 2)
		{
			car = Instantiate(taxiCar, carSpawnTransform.position, carSpawnTransform.rotation);
		}
		else if (MainMenuScirpt.selectedCar == 3)
		{
			car = Instantiate(sportsCar, carSpawnTransform.position, carSpawnTransform.rotation);
		}
		else if (MainMenuScirpt.selectedCar == 4)
		{
			car = Instantiate(copCar, carSpawnTransform.position, carSpawnTransform.rotation);
		}

		//Set audio sources volume
		countDownAudioSource.volume = SettingsMenu.musicVolume;
	}

	//FixedUpdate function
	void FixedUpdate()
	{
		if (!gameEnded)
		{
			if (countDownTimer <= 0)
			{
				//Turn off countdown object once counting has stop
				countDownGameObject.SetActive(false);

				//Start car
				if (!car.GetComponent<CarScript>().isCarMoving)
				{
					car.GetComponent<CarScript>().TurnOnOffCar(true);
				}

				//Incremented timer, level score and the level score text 
				levelScore += Time.fixedDeltaTime * (car.GetComponent<CarScript>().speed / 100);
				LevelScoreText.GetComponent<UnityEngine.UI.Text>().text = Mathf.Round(levelScore).ToString();

				//Move road pieces infront of each other as the car drives
				if (car.transform.position.z > tileSize)
				{
					if (car.transform.position.z > roadPieces[0].transform.position.z && passedFirstRoad)
					{
						roadPieces[2].transform.position = new Vector3(roadPieces[1].transform.position.x, roadPieces[1].transform.position.y, roadPieces[1].transform.position.z + tileSize);
						MoveFoliageUp(0, ((foliageObjectList.Count - 1) / 3), roadPieces[1].transform.position.z + tileSize, roadPieces[1].transform.position.z + (tileSize * 2));
						MoveTreesUp(0, ((forestObstaclesList.Count - 1) / 3), roadPieces[1].transform.position.z + tileSize, roadPieces[1].transform.position.z + (tileSize * 2));
						MoveRocksUp(0, ((roadObstaclesList.Count - 1) / 3), roadPieces[1].transform.position.z + tileSize, roadPieces[1].transform.position.z + (tileSize * 2));
						Debug.Log("Moving 2");
					}
					if (car.transform.position.z > roadPieces[1].transform.position.z)
					{
						roadPieces[0].transform.position = new Vector3(roadPieces[2].transform.position.x, roadPieces[2].transform.position.y, roadPieces[2].transform.position.z + tileSize);
						MoveFoliageUp(((foliageObjectList.Count - 1) / 3), ((foliageObjectList.Count - 1) / 2), roadPieces[2].transform.position.z + tileSize, roadPieces[2].transform.position.z + (tileSize * 2));
						MoveTreesUp(((forestObstaclesList.Count - 1) / 3), ((forestObstaclesList.Count - 1) / 2), roadPieces[2].transform.position.z + tileSize, roadPieces[2].transform.position.z + (tileSize * 2));
						MoveRocksUp(((roadObstaclesList.Count - 1) / 3), ((roadObstaclesList.Count - 1) / 2), roadPieces[2].transform.position.z + tileSize, roadPieces[2].transform.position.z + (tileSize * 2));
						Debug.Log("Moving 0");
					}
					if (car.transform.position.z > roadPieces[2].transform.position.z)
					{
						roadPieces[1].transform.position = new Vector3(roadPieces[0].transform.position.x, roadPieces[0].transform.position.y, roadPieces[0].transform.position.z + tileSize);
						MoveFoliageUp(((foliageObjectList.Count - 1) / 2), foliageObjectList.Count - 1, roadPieces[0].transform.position.z + tileSize, roadPieces[0].transform.position.z + (tileSize * 2));
						MoveTreesUp(((forestObstaclesList.Count - 1) / 2), forestObstaclesList.Count - 1, roadPieces[0].transform.position.z + tileSize, roadPieces[0].transform.position.z + (tileSize * 2));
						MoveRocksUp(((roadObstaclesList.Count - 1) / 2), roadObstaclesList.Count - 1, roadPieces[0].transform.position.z + tileSize, roadPieces[0].transform.position.z + (tileSize * 2));
						Debug.Log("Moving 1");
						passedFirstRoad = true;
					}
				}

			}
			else
			{
				if (countDownTimer >= 0 && countDownTimer < 1 && countDownGameObject.GetComponent<UnityEngine.UI.Image>().sprite != countDownImage1)
				{
					countDownGameObject.GetComponent<UnityEngine.UI.Image>().sprite = countDownImage1;
					countDownGameObject.GetComponent<UnityEngine.UI.Image>().color = Color.green;
					countDownAudioSource.PlayOneShot(goClip);
				}
				else if (countDownTimer >= 1 && countDownTimer < 2 && countDownGameObject.GetComponent<UnityEngine.UI.Image>().sprite != countDownImage2)
				{
					countDownGameObject.GetComponent<UnityEngine.UI.Image>().sprite = countDownImage2;
					countDownAudioSource.PlayOneShot(readyClip);
					countDownAudioSource.PlayOneShot(readyClip);
					countDownAudioSource.PlayOneShot(readyClip);
					countDownAudioSource.PlayOneShot(readyClip);
					countDownAudioSource.PlayOneShot(readyClip);
					Debug.Log("Playing sound");
				}
				else if (countDownTimer >= 2 && countDownTimer < 3 && countDownGameObject.GetComponent<UnityEngine.UI.Image>().sprite != countDownImage3)
				{
					countDownGameObject.GetComponent<UnityEngine.UI.Image>().sprite = countDownImage3;
					countDownAudioSource.PlayOneShot(readyClip);
				}
				countDownTimer -= Time.fixedDeltaTime;
			}

		}
	}


	//-50 to -1 left side
	//22 to 75 right side
	//Randomly moved a certain section of grass
	private void MoveFoliageUp(int startIndex, int endIndex, float startZ, float endZ)
	{

		for (int i = startIndex; i < endIndex; i++)
		{
			if (i < endIndex - i)
			{
				foliageObjectList[i].transform.position = new Vector3(Random.Range(-1, -50), roadGenPos.y, Random.Range(startZ, endZ));
			}
			else
			{
				foliageObjectList[i].transform.position = new Vector3(Random.Range(22, 75), roadGenPos.y, Random.Range(startZ, endZ));
			}

		}
	}
	private void MoveTreesUp(int startIndex, int endIndex, float startZ, float endZ)
	{
		for (int i = startIndex; i < endIndex; i++)
		{
			if (i < endIndex - i)
			{
				forestObstaclesList[i].transform.position = new Vector3(Random.Range(-1, -50), roadGenPos.y, Random.Range(startZ, endZ));
			}
			else
			{
				forestObstaclesList[i].transform.position = new Vector3(Random.Range(22, 75), roadGenPos.y, Random.Range(startZ, endZ));
			}

		}
	}

	private void MoveRocksUp(int startIndex, int endIndex, float startZ, float endZ)
	{
		for (int i = startIndex; i < endIndex; i++)
		{
			roadObstaclesList[i].transform.position = new Vector3(Random.Range(0, 22), roadGenPos.y+1f, Random.Range(startZ, endZ));
		}
	}
	
	//Method called when the game is over
	public void EndGameScreenDisplay()
	{
		//FadeCamerToBlack
		FadeCameraToBlack();
		Vector3 oldPosition = LevelScoreText.GetComponent<RectTransform>().localPosition;
		Vector3 newPosition = new Vector3(oldPosition.x, 90, oldPosition.z);
		LevelScoreText.GetComponent<RectTransform>().localPosition = Vector3.Lerp(oldPosition, newPosition, 2);
		//Show end canvas and stop generation
		endGameCanvas.SetActive(true);
		//Set end score and end gold text
		endScoreText.text = Mathf.RoundToInt(levelScore).ToString();
		endGoldText.text = Mathf.RoundToInt(levelScore).ToString();
		//Add gold to players playerprefs
		MainMenuScirpt.SetGold(MainMenuScirpt.GetGold() + Mathf.RoundToInt(levelScore));
		//set game to end and levelscore text to empty
		gameEnded = true;
	}

	//Method for when restart game is pressed
	public void RestartGameButton()
	{
		//Load game again
		SceneManager.LoadScene("game");
	}

	//Method for when exit button is pressed
	public void ExitToMenuButton()
	{
		//Sent player to the main menu
		SceneManager.LoadScene("MainMenu");
	}

	//Method for when sumbit score button is pressed
	public void SumbitScore()
	{
	}

	public void FadeCameraToBlack()
	{
		blackScreen.color = Color.black;
		blackScreen.canvasRenderer.SetAlpha(0.0f);
		blackScreen.CrossFadeAlpha(1.0f, 2, false);
	}
}
