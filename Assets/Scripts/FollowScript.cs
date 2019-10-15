using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    Vector3 offset = new Vector3(0, 20, -10);
    GameObject car;

    void Start()
    {
        car = GameObject.FindGameObjectWithTag("car");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = car.transform.position + offset;
    }
}
