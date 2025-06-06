using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [Header("Settings Panels")]
    public GameObject settingsPanel;
    private GameObject previousPanel;

    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle musicMuteToggle;
    public Toggle sfxMuteToggle;   

    private void Start()
    {
        musicSlider.value = 1f;
        sfxSlider.value = 1f;

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
    }

    public void OpenSettings(GameObject fromPanel)
    {
        previousPanel = fromPanel;
        fromPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void Back()
    {
        settingsPanel.SetActive(false);
        if (previousPanel != null)
        {
            previousPanel.SetActive(true);
        }
    }

    public void SetMusicVolume(float volume)
    {
        float volumeM = Mathf.Clamp(musicSlider.value, 0.0001f, 1f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volumeM) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        float volumeS = Mathf.Clamp(sfxSlider.value, 0.0001f, 1f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volumeS) * 20);
    }

    public void ToggleMuteMusic(bool isMuted)
    {
        if (isMuted)
        {
            audioMixer.SetFloat("MusicVolume", -80f);
            Debug.Log("Music Muted");
        }
        else
        {
            float volume = Mathf.Clamp(musicSlider.value, 0.0001f, 1f);
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
            Debug.Log("Music Unmuted");
        }
        audioMixer.GetFloat("MusicVolume", out float currentVolume);
        Debug.Log($"Current Music Volume: {currentVolume}");
    }

    public void ToggleMuteSFX(bool isMuted)
    {
        if (isMuted)
        {
            audioMixer.SetFloat("SFXVolume", -80f);
            Debug.Log("SFX Muted");
        }
        else
        {
            float volume = Mathf.Clamp(sfxSlider.value, 0.0001f, 1f);
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
            Debug.Log("SFX Unmuted");
        }
        audioMixer.GetFloat("SFXVolume", out float currentVolume); 
        Debug.Log($"Current SFX Volume: {currentVolume}"); 
    }
}