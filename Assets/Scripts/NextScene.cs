using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;

public class NextScene : MonoBehaviour
{

    [SerializeField] GameObject transition;
    [SerializeField] float transitionTime;
    private void Start()
    {
        if (PlayerPrefs.GetInt("NoTransition") == 1)
        {
            transition.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);
        // Check if the colliding object has a tag "Player"
        if (collision.gameObject.name == "Player")
        {
            Destroy(collision.gameObject);
            // Get the index of the current scene
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Load the next scene in the build settings
            StartCoroutine(LoadLevel(currentSceneIndex));
        }
    }

    IEnumerator LoadLevel(int currentSceneIndex)
    {
        transition.SetActive(true);
        PlayerPrefs.SetInt("NoTransition", 0);
        transition.GetComponent<Animator>().SetTrigger("EndLevel");

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadSceneAsync(currentSceneIndex + 1);
    }
}
