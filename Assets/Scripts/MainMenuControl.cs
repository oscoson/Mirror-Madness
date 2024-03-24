using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuControl : MonoBehaviour
{
    [SerializeField] float transitionTime;

    public void StartGame()
    {
        StartCoroutine(LoadLevel("Level 1")); // Load Level 1 scene
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
        StartCoroutine(LoadLevel("Level 1")); // Load Level 1 scene
    }

    public void OpenLevel2()
    {
        StartCoroutine(LoadLevel("Level 2")); // Load Level 2 scene
    }

    public void OpenLevel3()
    {
        StartCoroutine(LoadLevel("Level 3")); // Load Level 3 scene
    }

    public void OpenLevel4()
    {
        StartCoroutine(LoadLevel("Level 4")); // Load Level 4 scene
    }

    public void OpenLevel5()
    {
        StartCoroutine(LoadLevel("Level 5")); // Load Level 5 scene
    }

    public void OpenLevel6()
    {
        StartCoroutine(LoadLevel("Level 6")); // Load Level 6 scene
    }

        public void OpenLevel7()
    {
        StartCoroutine(LoadLevel("Level 7")); // Load Level 3 scene
    }

    public void OpenLevel8()
    {
        StartCoroutine(LoadLevel("Level 8")); // Load Level 4 scene
    }

    public void OpenLevel9()
    {
        StartCoroutine(LoadLevel("Level 9")); // Load Level 5 scene
    }

    public void OpenLevel10()
    {
        StartCoroutine(LoadLevel("Level 10")); // Load Level 6 scene
    }


    IEnumerator LoadLevel(string level)
    {
        PlayerPrefs.SetInt("NoTransition", 0);
        GameObject.FindWithTag("Transition").GetComponent<Animator>().SetTrigger("EndLevel");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadSceneAsync(level);
    }

}
