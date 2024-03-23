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

}
