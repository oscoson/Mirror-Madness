using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMira : MonoBehaviour
{
    private Vector2 startPos;
    public GameObject reflectorPrefab;
    public float MIRA_WIDTH = 0.2f;

    void Update()
    {

        // Listen for a mouse down
        if (Input.GetMouseButtonDown(0))
        {
            // Get the mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            startPos = mousePos;
        }

        // Listen for a mouse up
        if (Input.GetMouseButtonUp(0))
        {
            // Get the mouse position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check if the prefab is not null
            if (reflectorPrefab != null)
            {
                // Create a reflector
                GameObject reflector = Instantiate(reflectorPrefab, (startPos + mousePos) / 2, Quaternion.identity) as GameObject;

                // Set the scale of the reflector
                reflector.transform.localScale = new Vector3(Vector2.Distance(startPos, mousePos), MIRA_WIDTH, 1);

                // Set the rotation of the reflector
                reflector.transform.rotation = Quaternion.FromToRotation(Vector2.right, mousePos - startPos);
            }
        }
    }
}
