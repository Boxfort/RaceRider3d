using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenScript : MonoBehaviour
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
    private UnityEngine.UI.Text LevelScoreText;
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
    //Section List
    private List<GameObject> sectionList = new List<GameObject>();
    //Forest obstacles list
    private List<GameObject> forestObstaclesList = new List<GameObject>();
    //Foilage list
    private List<GameObject> foliageObjectList = new List<GameObject>();
    //Road Obstacles list
    private List<GameObject> roadObstaclesList = new List<GameObject>();


    //Start function
    void Start()
    {
        //Set level score
        LevelScoreText.text = levelScore.ToString();
        //Assing the main car object in the scene
        if(MainMenuScirpt.selectedCar == 0)
		{
			car = Instantiate(normalCar, carSpawnTransform.position,carSpawnTransform.rotation);
		}
		else if(MainMenuScirpt.selectedCar == 1)
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

		//Generate the first tile section
		sectionList.Add(Instantiate(road, roadGenPos, Quaternion.identity));
        //After generation first road move roadGen position
        roadGenPos.z += tileSize;
        //Set timer to max out at start
        timer = obstaclesPerSecond;
    }

    //FixedUpdate function
    void FixedUpdate()
    {
		if(!gameEnded)
		{
			//Incremented timer, level score and the level score text 
			timer += Time.fixedDeltaTime;
			levelScore += Time.fixedDeltaTime;
			LevelScoreText.text = Mathf.Round(levelScore).ToString();
			//Timer activation to generate obstacles
			if (timer > obstaclesPerSecond)
			{
				//0 to -28 left side
				//22 to 50 right side
				//Generate forest obstacles on left or right
				rand = Random.Range(0, 2);
				if (rand == 0)
				{
					GameObject forestObject = Instantiate(forestObstacles[Random.Range(0, forestObstacles.Length)], new Vector3(Random.Range(-1, -50), roadGenPos.y, Random.Range(roadGenPos.z, (roadGenPos.z * 2) + (tileSize + 1))), Quaternion.Euler(-90f, 0, 0));
					forestObject.transform.parent = sectionList[sectionList.Count - 1].transform;
				}
				else
				{
					GameObject forestObject = Instantiate(forestObstacles[Random.Range(0, forestObstacles.Length)], new Vector3(Random.Range(21, 70), roadGenPos.y, Random.Range(roadGenPos.z, (roadGenPos.z * 2) + (tileSize + 1))), Quaternion.Euler(-90f, 0, 0));
					forestObject.transform.parent = sectionList[sectionList.Count - 1].transform;
				}
				//Generate foliage on left and right in chunks of 25
				for (int i = 0; i < 25; i++)
				{
					//Foliage limit
					if (foliageObjectList.Count < 300)
					{
						GameObject foliageObject1 = Instantiate(foliage[Random.Range(0, foliage.Length)], new Vector3(Random.Range(-6, -50), roadGenPos.y, Random.Range(roadGenPos.z, (roadGenPos.z * 2) + (tileSize + 1))), Quaternion.identity);
						GameObject foliageObject2 = Instantiate(foliage[Random.Range(0, foliage.Length)], new Vector3(Random.Range(26, 70), roadGenPos.y, Random.Range(roadGenPos.z, (roadGenPos.z * 2) + (tileSize + 1))), Quaternion.identity);
						foliageObject1.transform.parent = sectionList[sectionList.Count - 1].transform;
						foliageObject2.transform.parent = sectionList[sectionList.Count - 1].transform;
					}
				}
				//Generate road obstacles
				rand = Random.Range(0, 9);
				if (rand > 3)
				{
					GameObject roadObject = Instantiate(roadObstacles[Random.Range(0, roadObstacles.Length)], new Vector3(Random.Range(0, 20), roadGenPos.y + 1f, Random.Range(roadGenPos.z, (roadGenPos.z * 10) + (tileSize + 1))), Quaternion.identity);
					roadObject.transform.parent = sectionList[sectionList.Count - 1].transform;
				}
				//Reset timer
				timer = 0;
			}
			//If car is further than road, create a new batch of roads
			if (car.transform.position.z > (roadGenPos.z - tileSize))
			{
				//Add the new road and delete old section if behind camera
				sectionList.Add(Instantiate(road, roadGenPos, Quaternion.identity));
				if (sectionList.Count > 5)
				{
					Destroy(sectionList[0]);
					sectionList.RemoveAt(0);
				}
				//Increase generated z by tile size
				roadGenPos.z += tileSize;
				//Add 1 to level counter
				levelCounter++;
				//Check if new level and then increase the tree spawn
				if (levelCounter >= levelSizeByTile)
				{
					car.GetComponent<CarScript>().speed += speedIncrease;
					levelCounter = 0;
					obstaclesPerSecond -= obstaclesIncreasePerLevel;
				}
			}
		}
    }

	//Method called when the game is over
	public void EndGameScreenDisplay()
	{
		//Show end canvas and stop generation
		endGameCanvas.SetActive(true);
		//Set end score and end gold text
		endScoreText.text = Mathf.RoundToInt(levelScore).ToString();
		endGoldText.text = Mathf.RoundToInt(levelScore).ToString();
		//Add gold to players playerprefs
		MainMenuScirpt.gold += Mathf.RoundToInt(levelScore);
		PlayerPrefs.SetInt("gold", MainMenuScirpt.gold);
		PlayerPrefs.Save();
		//set game to end and levelscore text to empty
		LevelScoreText.text = "";
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
}


