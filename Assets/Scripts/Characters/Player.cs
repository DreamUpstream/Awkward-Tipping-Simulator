using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    public EquipController equipCtrl;
    public static Player Instance;

    [Header("Audio")]
    [SerializeField] private AudioClip deathSFX1;

    [Header("Audio")]
    [SerializeField] private AudioClip deathSFX2;

    [Header("Audio")]
    [SerializeField] private AudioClip deathSFX3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
        }
    }

    public override void Die()
    {

        audioSource.PlayOneShot(deathSFX1);
        audioSource.PlayOneShot(deathSFX2);
        audioSource.PlayOneShot(deathSFX3, 0.5f);
        StartCoroutine(LoadLosingScene());
    }

    IEnumerator LoadLosingScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Losing_cinematic");
    }
}
