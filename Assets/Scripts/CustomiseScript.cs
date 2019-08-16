using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct carDetails
{
	public string name;
	public int price;
	public int speed;
	public bool unlocked;
};

public class CustomiseScript : MonoBehaviour
{
	//Transform of where the cat should be placed and then rotated
	[SerializeField]
	private Transform cameraCarRotationTransform;
	//Name,price and speed text
	[SerializeField]
	private UnityEngine.UI.Text nameText;
	[SerializeField]
	private UnityEngine.UI.Text priceText;
	[SerializeField]
	private UnityEngine.UI.Text speedText;
	//An array of all the cars in the game
	[SerializeField]
	private GameObject[] cars;
	//Directionary of price and car info
	private carDetails[] carsDetailsArray = new carDetails[5];
	//Car currently being displayed and the position in the array of that car
	private GameObject currentDisplayedCar;
	private int currentCarPosition = 0;

    // Start is called before the first frame update
    void Start()
    {
		//Set first car
		currentDisplayedCar = Instantiate(cars[currentCarPosition], cameraCarRotationTransform.position,Quaternion.identity);
		//Setting car details
		//Normal car
		carsDetailsArray[0].name = "Normal";
		carsDetailsArray[0].price = 0;
		carsDetailsArray[0].speed = 10;
		carsDetailsArray[0].unlocked = true;
		//Suv
		carsDetailsArray[1].name = "Suv";
		carsDetailsArray[1].price = 100;
		carsDetailsArray[1].speed = 15;
		carsDetailsArray[1].unlocked = false;
		//Taxi
		carsDetailsArray[2].name = "Taxi";
		carsDetailsArray[2].price = 300;
		carsDetailsArray[2].speed = 17;
		carsDetailsArray[2].unlocked = false;
		//Sports car
		carsDetailsArray[3].name = "Sports";
		carsDetailsArray[3].price = 500;
		carsDetailsArray[3].speed = 19;
		carsDetailsArray[3].unlocked = false;
		//Cop car
		carsDetailsArray[4].name = "Cop";
		carsDetailsArray[4].price = 1000;
		carsDetailsArray[4].speed = 20;
		carsDetailsArray[4].unlocked = false;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		//Spin currently displayed car
		currentDisplayedCar.transform.Rotate(0, 60 * Time.fixedDeltaTime, 0, Space.World);
		UpdateText();
		
    }

	//Method for when the forward button on the customise screen is pressed
	public void ForwardButton()
	{
		if(currentCarPosition < cars.Length-1)
		{	
			Destroy(currentDisplayedCar);
			currentCarPosition++;
			currentDisplayedCar = Instantiate(cars[currentCarPosition], cameraCarRotationTransform.position, Quaternion.identity);
			UpdateText();
		}
	}

	//Method for when the backwards button on the customise screen is presssed
	public void BackwardsButton()
	{
		if (currentCarPosition > 0)
		{
			Destroy(currentDisplayedCar);
			currentCarPosition--;
			currentDisplayedCar = Instantiate(cars[currentCarPosition], cameraCarRotationTransform.position, Quaternion.identity);
			UpdateText();
		}
	}

	public void UpdateText()
	{
		nameText.text = "Name: " + carsDetailsArray[currentCarPosition].name;
		priceText.text = "Price: " + carsDetailsArray[currentCarPosition].price;
		speedText.text = "Speed: " + carsDetailsArray[currentCarPosition].speed;
	}
}
