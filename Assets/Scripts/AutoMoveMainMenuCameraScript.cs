using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class AutoMoveMainMenuCameraScript : MonoBehaviour
{
    //Starting position of camera and end position
    private Vector3 startingPosition;
    private Vector3 endPosition;
    //How much the camera should extend on the z axis
    [SerializeField]
    private float cameraExtendZ;
    //CameraSpeed
    [SerializeField]
    private float cameraSpeed;
    void Start()
    {
        //Set starting point
        startingPosition = transform.position;
        //Set end point with the extended z axis
        endPosition = startingPosition;
        endPosition.z += cameraExtendZ;
    }

    void FixedUpdate()
    {
        //Ping pong between the start and end positions
        transform.position = Vector3.Lerp(startingPosition, endPosition, Mathf.PingPong(Time.fixedTime* cameraSpeed, 1));
    }
}
