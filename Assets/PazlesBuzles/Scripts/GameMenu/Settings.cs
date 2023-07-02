using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown QualityDropdown;
    [SerializeField]
    private TMP_Dropdown ResolutionDropdown;
    [SerializeField]
    private Slider MusicSlider;
    [SerializeField]
    private Slider SoundSlider;

    private Resolution[] _resolutions;
    private string prefsQualityName = "QualitySettings";
    private string prefsResolutionName = "ResolutionSettings";
    private string prefsFullscreenName = "FullscreenSettings";
    private string prefsMusicSettingsName = "MusicSettings";
    private string prefsSoundSettingsName = "SoundSettings";

    private void Start()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        int curResIdx = SetupResolutionsInfo();
        LoadSettings(curResIdx);
    }

    public void SetFullScreen(bool val)
    {
        Screen.fullScreen = val;
    }

    public void SetResolution(int resolutionIdx)
    {
        Resolution resolution = _resolutions[resolutionIdx];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIdx)
    {
        QualitySettings.SetQualityLevel(qualityIdx);
    }

    public void SetMusicVolume(float val)
    {
        AllServices.GetService<SoundManager>().MusicVolume = val;
    }

    public void SetSoundVolume(float val)
    {
        AllServices.GetService<SoundManager>().SoundVolume = val;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt(prefsQualityName, QualityDropdown.value);
        PlayerPrefs.SetInt(prefsResolutionName, ResolutionDropdown.value);
        PlayerPrefs.SetInt(prefsFullscreenName, System.Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat(prefsMusicSettingsName, MusicSlider.value);
        PlayerPrefs.SetFloat(prefsSoundSettingsName, SoundSlider.value);
    }

    private void LoadSettings(int curResIdx)
    {
        QualityDropdown.value = PlayerPrefs.GetInt(prefsQualityName, 2);
        ResolutionDropdown.value = PlayerPrefs.GetInt(prefsResolutionName, curResIdx);
        Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt(prefsFullscreenName, 1));

        float savedVal = PlayerPrefs.GetFloat(prefsMusicSettingsName, 0.5f);
        SetMusicVolume(savedVal);
        MusicSlider.value = savedVal;

        savedVal = PlayerPrefs.GetFloat(prefsSoundSettingsName, 0.5f);
        SetSoundVolume(savedVal);
        SoundSlider.value = savedVal;
    }

    private int SetupResolutionsInfo()
    {
        ResolutionDropdown.ClearOptions();
        List<string> options = new();
        _resolutions = Screen.resolutions;
        int curResIdx = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            Resolution resolution = _resolutions[i];
            options.Add($"{resolution.width}x{resolution.height} {resolution.refreshRateRatio}Hz");
            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
                curResIdx = i;
        }

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.RefreshShownValue();
        return curResIdx;
    }
}
