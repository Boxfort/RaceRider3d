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
	//Locked icon
	[SerializeField]
	private GameObject lockedIcon;
	//Buy button
	[SerializeField]
	private GameObject buyButton;
	//Select car button
	[SerializeField]
	private GameObject selectButton;
	//Select check mark
	[SerializeField]
	private GameObject selectCheckMark;
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
		//Load player cars, player 1 is default unlocked
		LoadCars();
		UpdateCarData();
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
		
		//Taxi
		carsDetailsArray[2].name = "Taxi";
		carsDetailsArray[2].price = 300;
		carsDetailsArray[2].speed = 17;
		//Sports car
		carsDetailsArray[3].name = "Sports";
		carsDetailsArray[3].price = 500;
		carsDetailsArray[3].speed = 19;
		//Cop car
		carsDetailsArray[4].name = "Cop";
		carsDetailsArray[4].price = 1000;
		carsDetailsArray[4].speed = 20;
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

	//Method for when the buy button on the customise screen is pressed
	public void BuyButton()
	{
		//If player has enough gold, minus the gold and update car data and text data
		if(MainMenuScirpt.gold >= carsDetailsArray[currentCarPosition].price)
		{
			MainMenuScirpt.SetGold(MainMenuScirpt.GetGold() - carsDetailsArray[currentCarPosition].price);
			carsDetailsArray[currentCarPosition].unlocked = true;
			UpdateCarData();
			UpdateText();
		}
	}

	//Method for when the select button on the customise screen is pressed
	public void SelectButton()
	{
		MainMenuScirpt.selectedCar = currentCarPosition;
		UpdateText();
	}

	//Update all customise text field and the unlocked icon
	public void UpdateText()
	{
		nameText.text = "Name: " + carsDetailsArray[currentCarPosition].name;
		priceText.text = "Price: " + carsDetailsArray[currentCarPosition].price;
		speedText.text = "Speed: " + carsDetailsArray[currentCarPosition].speed;

		if(!carsDetailsArray[currentCarPosition].unlocked)
		{
			lockedIcon.SetActive(true);
			buyButton.SetActive(true);
			selectButton.SetActive(false);
			selectCheckMark.SetActive(false);
		}
		else
		{
			lockedIcon.SetActive(false);
			buyButton.SetActive(false);
			if(MainMenuScirpt.selectedCar != currentCarPosition)
			{
				selectButton.SetActive(true);
				selectCheckMark.SetActive(false);
			}
			else
			{
				selectButton.SetActive(false);
				selectCheckMark.SetActive(true);
			}
			
		}
	}

	//Load cars, Unlocked car is the playerpref key for it has been found
	private void LoadCars()
	{
		//Load cars, car 1 is default unlocked
		if (PlayerPrefs.HasKey("car2Unlocked"))
		{
			if (PlayerPrefs.GetInt("car2Unlocked") == 0)
			{
				carsDetailsArray[1].unlocked = false;
			}
			else if (PlayerPrefs.GetInt("car2Unlocked") == 1)
			{
				carsDetailsArray[1].unlocked = true;
			}
		}
		else
		{
			PlayerPrefs.SetInt("car2Unlocked", 0);
			carsDetailsArray[1].unlocked = false;
		}

		if (PlayerPrefs.HasKey("car3Unlocked"))
		{
			if (PlayerPrefs.GetInt("car3Unlocked") == 0)
			{
				carsDetailsArray[2].unlocked = false;
			}
			else if (PlayerPrefs.GetInt("car3Unlocked") == 1)
			{
				carsDetailsArray[2].unlocked = true;
			}
		}
		else
		{
			PlayerPrefs.SetInt("car3Unlocked", 0);
			carsDetailsArray[2].unlocked = false;
		}

		if (PlayerPrefs.HasKey("car4Unlocked"))
		{
			if (PlayerPrefs.GetInt("car4Unlocked") == 0)
			{
				carsDetailsArray[3].unlocked = false;
			}
			else if (PlayerPrefs.GetInt("car4Unlocked") == 1)
			{
				carsDetailsArray[3].unlocked = true;
			}
		}
		else
		{
			PlayerPrefs.SetInt("car4Unlocked", 0);
			carsDetailsArray[3].unlocked = false;
		}

		if (PlayerPrefs.HasKey("car5Unlocked"))
		{
			if (PlayerPrefs.GetInt("car5Unlocked") == 0)
			{
				carsDetailsArray[4].unlocked = false;
			}
			else if (PlayerPrefs.GetInt("car5Unlocked") == 1)
			{
				carsDetailsArray[4].unlocked = true;
			}
		}
		else
		{
			PlayerPrefs.SetInt("car5Unlocked", 0);
			carsDetailsArray[4].unlocked = false;
		}
		PlayerPrefs.Save();
	}

	//Update cars player pref if changed to true
	private void UpdateCarData()
	{
		if(carsDetailsArray[1].unlocked == true)
		{
			PlayerPrefs.SetInt("car2Unlocked", 1);
		}
		else if(carsDetailsArray[2].unlocked == true)
		{
			PlayerPrefs.SetInt("car3Unlocked", 1);
		}
		else if (carsDetailsArray[3].unlocked == true)
		{
			PlayerPrefs.SetInt("car4Unlocked", 1);
		}
		else if (carsDetailsArray[4].unlocked == true)
		{
			PlayerPrefs.SetInt("car5Unlocked", 1);
		}
		PlayerPrefs.Save();
	}
}
