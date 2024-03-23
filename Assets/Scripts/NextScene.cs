using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);
        // Check if the colliding object has a tag "Player"
        if (collision.gameObject.name == "Player")
        {
            // Get the index of the current scene
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Load the next scene in the build settings
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
