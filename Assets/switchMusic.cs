using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchMusic : MonoBehaviour
{
    public AudioClip newTrack;
    private AudioManager audioManager;
    private bool hasTriggered = false; // Flag to track if the trigger has been activated

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            if (newTrack != null)
            {
                // Use AudioManager's PlayMusic method to switch music with crossfade
                audioManager.PlayMusic(newTrack);
                hasTriggered = true; // Mark trigger as activated
            }
        }
    }
}
