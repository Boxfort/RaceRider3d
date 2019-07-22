using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    //Adjustable speed for the car forward and turning movement
    public float speed;

    //Right and left front wheels
    [SerializeField]
    private GameObject frontRightWheel;
    [SerializeField]
    private GameObject frontLeftWheel;

    //Rotation angle of the wheels
    private float rotangle;
    void Update()
    {
        //Move the car upward(As the model is already rotated)
        var v3 = transform.up;
        v3.y = 0.0f;
        transform.Translate(v3 * speed * Time.deltaTime, Space.World);
        //Allinged each Wheel
        AllignedWheel(frontLeftWheel);
        AllignedWheel(frontRightWheel);

        //Rotate wheel and set rotangle based on user Input
        if (Input.GetKey(KeyCode.D) && rotangle <= 180)
        {
            rotangle = rotangle + 40 * Time.deltaTime;
            frontLeftWheel.transform.Rotate(0, 40 * Time.deltaTime, 0, Space.World);
        }

        if (Input.GetKey(KeyCode.A) && rotangle >= -180)
        {
            rotangle = rotangle - 40 * Time.deltaTime;
            frontRightWheel.transform.Rotate(0, -40 * Time.deltaTime, 0, Space.World);//Updated the 40 to -40
        }
    }

    //Align wheel with the body
    void AllignedWheel(GameObject wheel)
    {
        //Checking whether body is aligned with the wheel
        if (!Mathf.Approximately(wheel.transform.localEulerAngles.y, 0f))
        {

            //Since the wheel is attached to the body its rotation in world space will change when body rotates.
            //To prevent this we maintain the wheel's rotation in world space.
            Quaternion origWheelRot = wheel.transform.rotation;

            //Rotate body towards wheel's rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, wheel.transform.rotation, speed);
            wheel.transform.rotation = origWheelRot;
        }

    }
}