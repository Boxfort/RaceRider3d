using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunScript : MonoBehaviour
{
    //Light object
    [SerializeField]
    private Light sun;
    //Seconds in day
    [SerializeField]
    private float secondsInFullDay = 120f;
    //Current time of day 
    [Range(0, 1)]
    private float currentTimeOfDay = 0;
    
    //The default multiplayer before seconds
    [HideInInspector]
    private float timeMultiplier = 1f;

    //Starting sun value
    private float sunInitialIntensity;



    void Start()
    {
        //Set suns intensity
        sunInitialIntensity = sun.intensity;
    }

    void Update()
    {
        //Update the sun every frame
        UpdateSun();

        //Increase the time of day by Time by the seconds set and default multiplayer
        currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

        //Cap the current time of day
        if (currentTimeOfDay >= 1)
        {
            currentTimeOfDay = 0;
        }
    }

    //Update the sun rotation and intensity based on time
    void UpdateSun()
    {
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);

        float intensityMultiplier = 1;
        if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)
        {
            intensityMultiplier = 0;
        }
        else if (currentTimeOfDay <= 0.25f)
        {
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }
        else if (currentTimeOfDay >= 0.73f)
        {
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
        }

        sun.intensity = sunInitialIntensity * intensityMultiplier;
    }
}
