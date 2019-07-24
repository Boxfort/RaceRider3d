using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    //Adjustable speed for the car forward and turning movement
    public float speed;

    void FixedUpdate()
    {
        //Move the car upward(As the model is already rotated)
        var v3 = transform.up;
        v3.y = 0.0f;
        transform.Translate(v3 * speed * Time.deltaTime, Space.World);

        //Rotate car based input
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0f, 0f, 1f);
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0f, 0f, -1f);
        }
    }
}