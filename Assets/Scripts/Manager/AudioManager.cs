using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip bgmClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.3f;
        PlayBGM();
    }

    private void PlayBGM()
    {
        if (audioSource.clip != bgmClip)
        {
            audioSource.clip = bgmClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
