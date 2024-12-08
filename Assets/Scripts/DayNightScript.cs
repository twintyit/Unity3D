using System.Linq;
using UnityEngine;

public class DayNightScript : MonoBehaviour
{
    [SerializeField]
    private Material daySkybox;
    [SerializeField]
    private Material nightSkybox;

    private Light[] dayLights;
    private Light[] nightLights;
    private bool isDay;
    private AudioSource daySound;
    private AudioSource nightSound;

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        daySound = audioSources[0];
        nightSound = audioSources[1];

        dayLights = GameObject
            .FindGameObjectsWithTag("DayLight")
            .Select(x=> x.GetComponent<Light>())
            .ToArray();
        nightLights = GameObject
           .FindGameObjectsWithTag("NightLight")
           .Select(x => x.GetComponent<Light>())
           .ToArray();
        SwitchDayNight(true);
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.N)) 
        {
            SwitchDayNight(!isDay);
        }
    }

    private void SwitchDayNight(bool isDay)
    {
        this.isDay = isDay;
        foreach (Light light in dayLights)
        {
            light.enabled = isDay;
        }
        foreach (Light light in nightLights)
        {
            light.enabled = !isDay;
        }
        RenderSettings.skybox = isDay ? daySkybox : nightSkybox;
        RenderSettings.ambientIntensity = isDay ? 1.0f : 0.29f;
        if(isDay)
        {
            nightSound.Stop();
            daySound.Play();
        }
        else
        {
            daySound.Stop();
            nightSound.Play();
        }
    }

}
