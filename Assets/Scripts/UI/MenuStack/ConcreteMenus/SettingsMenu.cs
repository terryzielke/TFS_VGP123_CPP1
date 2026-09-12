using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : BaseMenu
{
    [SerializeField] private AudioMixer GameAudio;

    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider musicVolSlider;
    [SerializeField] private Slider sfxVolSlider;

    [SerializeField] private TMP_Text masterVolText;
    [SerializeField] private TMP_Text musicVolText;
    [SerializeField] private TMP_Text sfxVolText;

    public override void Initialize(MenuController context)
    {
        base.Initialize(context);
        menuState = MenuState.SettingsMenu;

        if (allButtons.Length == 0)
        {
            Debug.LogWarning("No buttons found in MainMenu. Please assign buttons in the inspector or ensure that they are children of the MainMenu GameObject.");
            return;
        }

        foreach (Button button in allButtons)
        {
            if (button == null) continue;
            if (button.name.Contains("Back")) button.onClick.AddListener(JumpBack);
        }

        //slider setup here
        if (masterVolSlider && masterVolText)
        {
            SetupSliderInformation(masterVolSlider, masterVolText, "MasterVol");
        }
        if (musicVolSlider && musicVolText)
        {
            SetupSliderInformation(musicVolSlider, musicVolText, "MusicVol");
        }
        if (sfxVolSlider && sfxVolText)
        {
            SetupSliderInformation(sfxVolSlider, sfxVolText, "SFXVol");
        }
    }

    private void SetupSliderInformation(Slider slider, TMP_Text text, string parameterName)
    {
        slider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, slider, text, parameterName));
        OnSliderValueChanged(slider.value, slider, text, parameterName);
    }

    private void OnSliderValueChanged(float value, Slider slider, TMP_Text text, string parameterName)
    {
        if (value == 0)
        {
            value = -80f; // Set to minimum volume in decibels
            text.text = "0%";
        }
        else
        {
            value = Mathf.Log10(value) * 20f; // Convert linear value to decibels
            text.text = Mathf.RoundToInt(slider.value * 100f) + "%"; // Display percentage
        }

        GameAudio.SetFloat(parameterName, value);
    }
}