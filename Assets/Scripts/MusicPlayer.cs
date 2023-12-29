using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] musicClips;

    private int currentClipIndex = 0;

    private void Start()
    {
        PlayNextClip();
    }

    private void PlayNextClip()
    {
        audioSource.clip = musicClips[currentClipIndex];
        audioSource.Play();
        currentClipIndex++;
        if (currentClipIndex >= musicClips.Length)
        {
            currentClipIndex = 0;
        }
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextClip();
        }
    }
}