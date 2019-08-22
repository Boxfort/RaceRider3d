using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
	//TiltFactor Slider
	[SerializeField]
	private UnityEngine.UI.Slider tiltFactorSlider;
	//TiltFactor text
	[SerializeField]
	private UnityEngine.UI.Text tiltFactorText;
	//TiltFactor 
	public static float tiltFactor = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
		if (PlayerPrefs.HasKey("tiltFactor"))
		{
			tiltFactor = PlayerPrefs.GetFloat("tiltFactor");
		}
		else
		{
			PlayerPrefs.SetFloat("tiltFactor", 0.25f);
		}
		tiltFactorSlider.value = tiltFactor;
		tiltFactorText.text = "Tilt Factor: " + tiltFactorSlider.value.ToString("0.00");
		PlayerPrefs.Save();
	}

    // Update is called once per frame
    void Update()
    {
        if(tiltFactorSlider.value != tiltFactor)
		{
			tiltFactor = tiltFactorSlider.value;
			tiltFactorText.text = "Tilt Factor: " + tiltFactorSlider.value.ToString("0.00");
			PlayerPrefs.SetFloat("tiltFactor", tiltFactor);
			PlayerPrefs.Save();
		}
    }
}
