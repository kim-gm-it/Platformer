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

    private bool isMusicMuted = false;
    private bool isSFXMuted = false;

    private void Start()
    {
        if (musicSlider != null)
        {
            SetMusicVolume(musicSlider.value);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            SetSFXVolume(sfxSlider.value);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        if (musicMuteToggle != null)
        {
            musicMuteToggle.onValueChanged.AddListener(ToggleMuteMusic);
        }

        if (sfxMuteToggle != null)
        {
            sfxMuteToggle.onValueChanged.AddListener(ToggleMuteSFX);
        }
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
        if (isMusicMuted) return;
        float volumeM = Mathf.Clamp(volume, 0.0001f, 1f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volumeM) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        if (isSFXMuted) return;
        float volumeS = Mathf.Clamp(volume, 0.0001f, 1f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volumeS) * 20);
    }

    public void ToggleMuteMusic(bool isMuted)
    {
        isMusicMuted = isMuted;

        if (isMuted)
        {
            audioMixer.SetFloat("MusicVolume", -80f);
        }
        else
        {
            SetMusicVolume(musicSlider.value);
        }
    }

    public void ToggleMuteSFX(bool isMuted)
    {
        isSFXMuted = isMuted;

        if (isMuted)
        {
            audioMixer.SetFloat("SFXVolume", -80f);
        }
        else
        {
            SetSFXVolume(sfxSlider.value);
        }
    }
}
