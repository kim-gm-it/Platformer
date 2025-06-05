using UnityEngine;

public class SpikesSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip spikeAudioClip;
    public void playSpikeSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(spikeAudioClip);
        }
    }
    
}
