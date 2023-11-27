using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public string sceneToLoad;
    public AudioClip typingSound;

    private int index;
    private AudioSource audioSource;

    void Start()
    {
        textComponent.text = string.Empty;
        audioSource = gameObject.AddComponent<AudioSource>();
        StartDialogue();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (textComponent.text == lines[index])
            {
                textComponent.text = string.Empty;
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            PlayTypingSound();
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text += string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    void PlayTypingSound()
    {
        if (typingSound != null)
        {
            audioSource.clip = typingSound;
            audioSource.Play();
        }
    }
}
