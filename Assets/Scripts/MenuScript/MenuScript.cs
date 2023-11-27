using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadScene("Main_menu");
    }
    public void LoadAbout()
    {
        SceneManager.LoadScene("About_menu");
    }

    public void LoadControls()
    {
        SceneManager.LoadScene("Controls_menu");
    }

    public void LoadIntro()
    {
        SceneManager.LoadScene("Intro_cinematic");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
