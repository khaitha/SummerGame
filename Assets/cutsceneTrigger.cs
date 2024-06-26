using UnityEngine;
using UnityEngine.Playables;

public class cutsceneTrigger : MonoBehaviour
{
    public PlayableDirector playableDirector;
    private bool hasTriggered = false; // To ensure the timeline only plays once

    void Start()
    {
        // Optionally, you can find the PlayableDirector component if not assigned
        if (playableDirector == null)
        {
            playableDirector = GetComponent<PlayableDirector>();
        }
    }

    // This method can be called to play the timeline
    public void PlayTimeline()
    {
        if (playableDirector != null && !hasTriggered)
        {
            playableDirector.Play();
            hasTriggered = true; // Mark that the timeline has been triggered
        }
    }

    // Example of a trigger event
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Replace "Player" with the appropriate tag
        {
            PlayTimeline();
        }
    }
}
