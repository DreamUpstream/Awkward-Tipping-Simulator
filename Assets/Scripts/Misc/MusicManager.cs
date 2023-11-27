using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public AudioClip backgroundMusic;

    private AudioSource audioSource;

    private void Awake()
    {
        // Ensure only one instance of MusicManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Create and configure the AudioSource
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.volume = 0.5f; // Adjust the volume as needed

            // Load and play the background music
            if (backgroundMusic != null)
            {
                audioSource.clip = backgroundMusic;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Background music AudioClip not set in the MusicManager.");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}