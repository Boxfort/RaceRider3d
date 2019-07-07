using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    public float speed;


    void Update()
    {

        var v3 = transform.forward;
        v3.y = 0.0f;
        transform.Translate(v3 * speed * Time.deltaTime, Space.World);
    }
}