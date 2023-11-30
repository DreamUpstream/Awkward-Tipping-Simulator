using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public AudioClip backgroundMusic;
    public string[] scenesWithoutMusic;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.volume = 0.5f;

            if (backgroundMusic != null)
            {
                audioSource.clip = backgroundMusic;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Background music AudioClip not set in the MusicManager.");
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (ShouldPlayMusic(scene.name))
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            StopMusic();
        }
    }

    private bool ShouldPlayMusic(string sceneName)
    {
        foreach (string scene in scenesWithoutMusic)
        {
            if (scene == sceneName)
                return false;
        }
        return true;
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
