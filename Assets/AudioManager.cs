using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip deathMusic;

    [Header("Crossfade Settings")]
    public float crossfadeDuration = 1.0f; // Duration of the crossfade in seconds

    private Coroutine crossfadeCoroutine; // Coroutine reference for crossfading

    private void Start()
    {
        // Start playing the initial background music
        PlayMusic(backgroundMusic);
    }

    public void PlayMusic(AudioClip music)
    {
        if (music == null)
            return;

        if (crossfadeCoroutine != null)
        {
            // If a crossfade coroutine is already running, stop it
            StopCoroutine(crossfadeCoroutine);
        }

        // Start crossfade to the new music
        crossfadeCoroutine = StartCoroutine(CrossfadeMusicCoroutine(music));
    }

    private IEnumerator CrossfadeMusicCoroutine(AudioClip newMusic)
    {
        if (musicSource.isPlaying)
        {
            // Fade out the current music
            float startVolume = musicSource.volume;
            float timer = 0.0f;

            while (timer < crossfadeDuration)
            {
                timer += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0.0f, timer / crossfadeDuration);
                yield return null;
            }

            musicSource.Stop();
        }

        // Start playing the new music
        musicSource.clip = newMusic;
        musicSource.Play();

        // Fade in the new music
        float targetVolume = 1.0f;
        float fadeInTimer = 0.0f;

        while (fadeInTimer < crossfadeDuration)
        {
            fadeInTimer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0.0f, targetVolume, fadeInTimer / crossfadeDuration);
            yield return null;
        }

        musicSource.volume = targetVolume; // Ensure volume is set to full at the end
        crossfadeCoroutine = null; // Reset coroutine reference
    }
}
