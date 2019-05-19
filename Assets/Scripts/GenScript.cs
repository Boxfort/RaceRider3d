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
    //Tree Object
    [SerializeField]
    private GameObject tree;
    //Tile size
    [SerializeField]
    private float tileSize = 400f;
    //Level Size By Tile
    [SerializeField]
    private int levelSizeByTile = 10;
    private int levelCounter = 0;
    //Tree Decrease per level
    [SerializeField]
    private float treeDecrease = 0.1f;
    [SerializeField]
    private UnityEngine.UI.Text LevelScoreText;
    private int levelScore;
    //Random int
    private int rand;
    //Timer
    private float timer;
    //Speed
    private float speed = 100f;
    //Tree gen amount
    private float treeAmountPerSecond = 1f;
    //Position of new Road placements
    private Vector3 roadGenPos = new Vector3(0, 0, 0);
    //Section List
    private List<GameObject> sectionList = new List<GameObject>();
    //Tree List
    private List<GameObject> trees = new List<GameObject>();



    void Start()
    {
        LevelScoreText.text = levelScore.ToString();
        //Generate the first tile section
        car = GameObject.FindGameObjectWithTag("car");
        carRB = car.GetComponent<Rigidbody>();
        sectionList.Add(Instantiate(road, roadGenPos, Quaternion.identity));
        roadGenPos.z += tileSize;
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(timer > treeAmountPerSecond)
        {
            //0 to -28 left side
            //22 to 50 right side
            rand = Random.Range(0, 2);
            if (rand == 0)
            {
                trees.Add(Instantiate(tree, new Vector3(Random.Range(-1, -30), roadGenPos.y, Random.Range(roadGenPos.z, (roadGenPos.z * 10) + (tileSize+1))), Quaternion.identity));
            }
            else
            {
                trees.Add(Instantiate(tree, new Vector3(Random.Range(21, 50), roadGenPos.y, Random.Range(roadGenPos.z, (roadGenPos.z * 10) + (tileSize+1))), Quaternion.identity));
            }
            timer = 0;
        }
        //Move car based on Time.deltaTime
        carRB.GetComponent<Rigidbody>().MovePosition(new Vector3(car.transform.position.x, car.transform.position.y, car.transform.position.z + (speed * Time.deltaTime)));
        Vector3 acc = Input.acceleration;
        carRB.AddForce(acc.x * speed, 0, acc.y * speed);
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
            if(trees.Count > 50 && Camera.main.transform.position.z > trees[24].transform.position.z)
            {
                for(int i = 0; i < 25; i++)
                {
                    Debug.Log("Destroying trees");
                    Destroy(trees[i]);
                    trees.RemoveAt(i);
                }
            }

            roadGenPos.z += tileSize;
            levelCounter++;
            //Check if new level and then increase the tree spawn
            if(levelCounter >= levelSizeByTile)
            {
                levelCounter = 0;
                treeAmountPerSecond -= treeDecrease;
                levelScore++;
                LevelScoreText.text = levelScore.ToString();
            }
        }

        
    }
}
