using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    //Adjustable speed for the car forward and turning movement
    public float speed;
    //Factor of when the phone just start changing direction input out of 1f

    private float tiltFactor;
    //Orginal car rotation on the z axis
    private float orginalZRotation;
    //Store car rotation
    private float rotationDirection;
	//Is car moving
	private bool isCarMoving = false;
	//Cars camera
	private Camera mainCamera;
	private Vector3 defaultCameraRotation;
	


	void Start()
    {
		//Setting tilt factor
		tiltFactor = SettingsMenu.tiltFactor;
		//Set orginal z rotation
        orginalZRotation = transform.rotation.z;
		//Set mainCamera
		mainCamera = Camera.main;
		//Set default camera rotation
		defaultCameraRotation = mainCamera.transform.rotation.eulerAngles;
    }
    void FixedUpdate()
    {
		if(isCarMoving)
		{
			Vector3 v3 = transform.up;
			v3.y = 0.0f;
			//Move the car upward(As the model is already rotated)
			transform.Translate(v3 * Time.fixedDeltaTime * speed, Space.World);

			//Move car left or right based on phone tilt
			if (Input.acceleration.normalized.x > tiltFactor)
			{
				transform.Rotate(0f, 0f, 1f);
				rotationDirection = 1f;
			}
			else if (Input.acceleration.normalized.x <= -tiltFactor)
			{
				transform.Rotate(0f, 0f, -1f);
				rotationDirection = -1f;
			}
			else if (!Mathf.Approximately(orginalZRotation, transform.rotation.z))
			{
				transform.Rotate(0f, 0f, rotationDirection);
			}
		}
		else
		{
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
		mainCamera.transform.rotation = Quaternion.Euler(defaultCameraRotation);
    }

	//Car collision method
    void OnCollisionEnter(Collision collision)
    {
		//If car has hit an object, called end game method
        if(collision.gameObject.tag == "Object")
        {
			transform.Translate(new Vector3(0, 0, -4));
			TurnOnOffCar(false);
			GameObject.Find("GameManger").GetComponent<GenScript>().EndGameScreenDisplay();
		}
    }

	public void TurnOnOffCar(bool engine)
	{
		if(engine)
		{
			isCarMoving = true;
		}
		else
		{
			isCarMoving = false;
			
		}
	}
}