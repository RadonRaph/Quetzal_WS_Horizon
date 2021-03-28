using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Audio;


public class Settings : MonoBehaviour
{
    public Volume volume;
    public VolumeProfile[] profiles;

    public Slider QualitySlider;
    public Slider MusicSlider;
    public Slider SFXSlider;
    public Slider SensibilitySlider;

    public AudioMixer mixer;

    public Controller_WithFocusPoint player;
    float lowSensibility = 0;
    float highSensibility = 10;

    // Start is called before the first frame update
    void Start()
    {
        lowSensibility = 0.95f * player.MouseSensibility;
        highSensibility = 1.07f * player.MouseSensibility;

        if (PlayerPrefs.HasKey("Quality"))
        {
            QualitySlider.value = PlayerPrefs.GetInt("Quality");
        }

        if (PlayerPrefs.HasKey("Music"))
        {
            MusicSlider.value = PlayerPrefs.GetFloat("Music");
        }

        if (PlayerPrefs.HasKey("SFX"))
        {
            SFXSlider.value = PlayerPrefs.GetFloat("SFX");
        }

        if (PlayerPrefs.HasKey("Sensi"))
        {
            SensibilitySlider.value = PlayerPrefs.GetFloat("Sensi");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QualityChange()
    {
        volume.profile = profiles[(int)QualitySlider.value];
        PlayerPrefs.SetInt("Quality", (int)QualitySlider.value);
    }

    public void MusicChange()
    {
        float val = Remap(MusicSlider.value, 0, 1.2f, 0, 1f);
        mixer.SetFloat("MusicVol", PercentToDb(val*130));
        PlayerPrefs.SetFloat("Music", MusicSlider.value);
    }

    public void SFXChange()
    {
        float val = Remap(SFXSlider.value, 0, 1.2f, 0, 1f);
        Debug.Log(val);
        mixer.SetFloat("SFXVol", PercentToDb(val*130));
        PlayerPrefs.SetFloat("SFX", SFXSlider.value);
    }

    public void SensibilityChange()
    {
        player.MouseSensibility = Mathf.Lerp(SensibilitySlider.value, lowSensibility, highSensibility);
        PlayerPrefs.SetFloat("Sensi", SensibilitySlider.value);
    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    float PercentToDb(float percent)
    {
        float db = -((percent * percent) / 375f) + ((16 * percent) / 15) - 80;
        return db;
    }
}
