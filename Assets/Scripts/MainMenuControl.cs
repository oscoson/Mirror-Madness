using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuControl : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1"); // Load Level 1 scene
    }

    // Method called when the Levels button is clicked
    public void OpenLevels()
    {
        SceneManager.LoadScene("LevelSelector"); // Load Levels scene
    }

    // Method called when the Settings button is clicked
    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings"); // Load Settings scene
    }

    // Method called when the Exit button is clicked
    public void ExitGame()
    {
        // Exit the game (only works in standalone builds)
        Debug.Log("Game is exiting...");
        Application.Quit();
    }

    public void OpenLevel1()
    {
        SceneManager.LoadScene("Level 1"); // Load Level 1 scene
    }

    public void OpenLevel2()
    {
        SceneManager.LoadScene("Level 2"); // Load Level 2 scene
    }

    public void OpenLevel3()
    {
        SceneManager.LoadScene("Level 3"); // Load Level 3 scene
    }

    public void OpenLevel4()
    {
        SceneManager.LoadScene("Level 4"); // Load Level 4 scene
    }

    public void OpenLevel5()
    {
        SceneManager.LoadScene("Level 5"); // Load Level 5 scene
    }

    public void OpenLevel6()
    {
        SceneManager.LoadScene("Level 6"); // Load Level 6 scene
    }


}
