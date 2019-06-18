using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;

}


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
    private int levelScore;
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



    void Start()
    {
        LevelScoreText.text = levelScore.ToString();
        //Generate the first tile section
        car = GameObject.FindGameObjectWithTag("car");
        sectionList.Add(Instantiate(road, roadGenPos, Quaternion.identity));
        roadGenPos.z += tileSize;
    }

    void FixedUpdate()
    {

        timer += Time.deltaTime;
        if(timer > obstaclesPerSecond)
        {
            //0 to -28 left side
            //22 to 50 right side
            rand = Random.Range(0, 2);
            if (rand == 0)
            {
                forestObstaclesList.Add(Instantiate(forestObstacles[Random.Range(0, forestObstacles.Length)], new Vector3(Random.Range(-1, -50), roadGenPos.y, Random.Range(roadGenPos.z, (roadGenPos.z * 5) + (tileSize+1))), Quaternion.identity));
            }
            else
            {
                forestObstaclesList.Add(Instantiate(forestObstacles[Random.Range(0, forestObstacles.Length)], new Vector3(Random.Range(21, 70), roadGenPos.y, Random.Range(roadGenPos.z, (roadGenPos.z * 5) + (tileSize+1))), Quaternion.identity));
            }
            for(int i = 0; i < 25; i ++)
            {
                foliageObjectList.Add(Instantiate(foliage[Random.Range(0, foliage.Length)], new Vector3(Random.Range(-6, -50), roadGenPos.y, Random.Range(roadGenPos.z, (roadGenPos.z * 5) + (tileSize + 1))), Quaternion.identity));
                foliageObjectList.Add(Instantiate(foliage[Random.Range(0, foliage.Length)], new Vector3(Random.Range(26, 70), roadGenPos.y, Random.Range(roadGenPos.z, (roadGenPos.z * 5) + (tileSize + 1))), Quaternion.identity));
            }
            
            rand = Random.Range(0, 9);
            if(rand > 5)
            {
                roadObstaclesList.Add(Instantiate(roadObstacles[Random.Range(0,roadObstacles.Length)], new Vector3(Random.Range(0,20), roadGenPos.y+1f, Random.Range(roadGenPos.z, (roadGenPos.z * 10) + (tileSize + 1))), Quaternion.identity));

            }
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
            }
            if(forestObstaclesList.Count > 50 && Camera.main.transform.position.z > forestObstaclesList[50].transform.position.z)
            {
                for(int i = 0; i < 50; i++)
                {
                    Debug.Log("Destroying trees");
                    Destroy(forestObstaclesList[i]);
                    Destroy(foliageObjectList[i]);
                    foliageObjectList.RemoveAt(i);
                    forestObstaclesList.RemoveAt(i);
                }
            }

            roadGenPos.z += tileSize;
            levelCounter++;
            //Check if new level and then increase the tree spawn
            if(levelCounter >= levelSizeByTile)
            {
                car.GetComponent<CarScript>().speed += speedIncrease;
                levelCounter = 0;
                obstaclesPerSecond -= obstaclesIncreasePerLevel;
                levelScore++;
                LevelScoreText.text = levelScore.ToString();
            }
        }

        
    }
}


