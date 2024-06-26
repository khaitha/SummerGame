using UnityEngine;

public class VictoryMusic : MonoBehaviour
{
    public AudioClip victoryAudioClip; // Assign the victory audio clip in the Inspector
    private AudioManager audioManager;
    private bool hasPlayedVictoryMusic = false;
    private BossHealth bossHealth;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>(); // Or assign through Inspector
        bossHealth = FindObjectOfType<BossHealth>(); // Find BossHealth component
    }

    void Update()
    {
        // Check if boss has died and play victory music once
        if (!hasPlayedVictoryMusic && IsBossDead())
        {
            audioManager.PlayMusic(victoryAudioClip);
            hasPlayedVictoryMusic = true;
        }
    }

    bool IsBossDead()
    {
        // Check if the boss's health is zero or less
        return bossHealth != null && bossHealth.CurrentHealth <= 0;
    }
}
