using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    //Adjustable speed for the car forward and turning movement
    public float speed;
    //Factor of when the phone just start changing direction input out of 1f
    [SerializeField]
    private float tiltFactor;

    void FixedUpdate()
    {
        var v3 = transform.up;
        v3.y = 0.0f;
        //Move the car upward(As the model is already rotated)
        transform.Translate(v3 * speed * Time.deltaTime, Space.World);

        //Move car left or right based on phone tilt
        if (Input.acceleration.normalized.x > tiltFactor)
        {
            transform.Rotate(0f, 0f, 1f);
        }
        else if(Input.acceleration.normalized.x <= -tiltFactor)
        {
            transform.Rotate(0f, 0f, -1f);
        }
    }
}