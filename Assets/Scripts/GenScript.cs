using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    private UnityEngine.UI.Text LevelScoreText;
    private decimal levelScore;
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
        car = GameObject.FindGameObjectWithTag("car");
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
        //Incremented timer, level score and the level score text 
        timer += Time.fixedDeltaTime;
        levelScore += (decimal)Time.fixedDeltaTime;
        LevelScoreText.text = levelScore.ToString();
        //Timer activation to generate obstacles
        if(timer > obstaclesPerSecond)
        {
            //0 to -28 left side
            //22 to 50 right side
            //Generate forest obstacles on left or right
            rand = Random.Range(0, 2);
            if (rand == 0)
            {
                forestObstaclesList.Add(Instantiate(forestObstacles[Random.Range(0, forestObstacles.Length)], new Vector3(Random.Range(-1, -50), roadGenPos.y, Random.Range(roadGenPos.z, (roadGenPos.z * 5) + (tileSize+1))), Quaternion.Euler(-90f,0,0)));
            }
            else
            {
                forestObstaclesList.Add(Instantiate(forestObstacles[Random.Range(0, forestObstacles.Length)], new Vector3(Random.Range(21, 70), roadGenPos.y, Random.Range(roadGenPos.z, (roadGenPos.z * 5) + (tileSize+1))), Quaternion.Euler(-90f, 0, 0)));
            }
            //Generate foliage on left and right in chunks of 25
            for(int i = 0; i < 25; i ++)
            {
                //Foliage limit
                if(foliageObjectList.Count < 300)
                {
                    foliageObjectList.Add(Instantiate(foliage[Random.Range(0, foliage.Length)], new Vector3(Random.Range(-6, -50), roadGenPos.y, Random.Range(roadGenPos.z, (roadGenPos.z * 5) + (tileSize + 1))), Quaternion.identity));
                    foliageObjectList.Add(Instantiate(foliage[Random.Range(0, foliage.Length)], new Vector3(Random.Range(26, 70), roadGenPos.y, Random.Range(roadGenPos.z, (roadGenPos.z * 5) + (tileSize + 1))), Quaternion.identity));
                }
            }
            //Generate road obstacles
            rand = Random.Range(0, 9);
            if(rand > 5)
            {
                roadObstaclesList.Add(Instantiate(roadObstacles[Random.Range(0,roadObstacles.Length)], new Vector3(Random.Range(0,20), roadGenPos.y+1f, Random.Range(roadGenPos.z, (roadGenPos.z * 10) + (tileSize + 1))), Quaternion.identity));

            }
            //Reset timer
            timer = 0;
        }
        //If car is further than road, create a new batch of roads
        if (car.transform.position.z > (roadGenPos.z-tileSize))
        {
            //Add the new road and delete old section if behind camera
            sectionList.Add(Instantiate(road, roadGenPos, Quaternion.identity));
            if(Camera.main.transform.position.z - tileSize > sectionList[0].transform.position.z)
            {
                Destroy(sectionList[0]);
                sectionList.RemoveAt(0);
                //Delete forest Obstacles
                for(int i = 0; i < forestObstaclesList.Count-1;i++)
                {
                    if(forestObstaclesList[i].transform.position.z < Camera.main.transform.position.z- tileSize)
                    {
                        Destroy(forestObstaclesList[i]);
                        forestObstaclesList.RemoveAt(i);
                    }
                }
                //Delete foliage obstacles
                for(int i = 0; i < foliageObjectList.Count-1;i++)
                {
                    if(foliageObjectList[i].transform.position.z < Camera.main.transform.position.z - tileSize)
                    {
                        Destroy(foliageObjectList[i]);
                        foliageObjectList.RemoveAt(i);
                    }
                }
                //Delete road obstacles
                for(int i = 0; i < roadObstaclesList.Count-1;i++)
                {
                    if(roadObstaclesList[i].transform.position.z < Camera.main.transform.position.z - tileSize)
                    {
                        Destroy(roadObstaclesList[i]);
                        roadObstaclesList.RemoveAt(i);
                    }
                }
            }
            //Increase generated z by tile size
            roadGenPos.z += tileSize;
            //Add 1 to level counter
            levelCounter++;
            //Check if new level and then increase the tree spawn
            if(levelCounter >= levelSizeByTile)
            {
                car.GetComponent<CarScript>().speed += speedIncrease;
                levelCounter = 0;
                obstaclesPerSecond -= obstaclesIncreasePerLevel;
            }
        }
    }
}


