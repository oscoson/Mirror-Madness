using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiraManager : MonoBehaviour
{
    private Vector2 startPos;
    public GameObject reflectorPrefab;
    public GameObject rotatorPrefab;
    public float REFLECTOR_WIDTH = 0.2f;
    public bool placing = false;

    public string currentTool = "Reflector";

    void Update()
    {

    // ----------KEYBOARD INPUT----------

        // Listen for a number 1 key down
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentTool = "Reflector";
            
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentTool = "Rotator";
        }

    // ----------MOUSE INPUT----------

        // Listen for a left mouse down
        if (Input.GetMouseButtonDown(0))
        {
            placing = true;

            // Get the mouse position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            startPos = mousePos;

            // Listen for a shift key down
            if (Input.GetKey(KeyCode.LeftShift))
            {
                placing = false;

                // Delete the reflector if it is tagged as a reflector
                Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, 0.1f);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.tag == "Reflector")
                    {
                        Destroy(collider.gameObject);
                    }
                }
            }
        }

        // Listen for a left mouse up
        if (Input.GetMouseButtonUp(0) && placing)
        {
            // Get the mouse position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (currentTool == "Reflector")
            {
                // Place the reflector
                PlaceReflector(startPos, mousePos);
            }
            else if (currentTool == "Rotator")
            {
                // Place the rotator
                PlaceRotator(startPos, mousePos);
            }
        }

        // Listen for a right mouse down
        if (Input.GetMouseButtonDown(1))
        {
            // Get the mouse position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            startPos = mousePos;
        }

        // Listen for a right mouse up
        if (Input.GetMouseButtonUp(1))
        {
            // Get the mouse position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Place the reflector
            if (currentTool == "Reflector")
            {
                PlaceReflector(startPos, mousePos, -1);
            }
        }
    }

    void PlaceReflector(Vector2 startPos, Vector2 endPos, int inverse = 1)
    {
        // Check if the prefab is not null
        if (reflectorPrefab != null)
        {
            // Create a reflector
            GameObject reflector = Instantiate(reflectorPrefab, (startPos + endPos) / 2, Quaternion.identity) as GameObject;

            // Set the scale of the reflector
            reflector.transform.localScale = new Vector3(Vector2.Distance(startPos, endPos), REFLECTOR_WIDTH, 1);

            // Set the rotation of the reflector
            reflector.transform.rotation = Quaternion.FromToRotation(Vector2.right, inverse * (endPos - startPos));
        }
    }

    void PlaceRotator(Vector2 startPos, Vector2 endPos)
    {
        // Check if the prefab is not null
        if (rotatorPrefab != null)
        {
            // Create a rotator
            GameObject rotator = Instantiate(rotatorPrefab, startPos, Quaternion.identity) as GameObject;

            // Set the scale of the rotator
            Vector2 dist = (endPos - startPos) * 2;
            float mag = dist.magnitude;

            // Get the transform of the child
            Transform shadow = rotator.transform.GetChild(0);

            // Set the scale of the child
            shadow.localScale = new Vector3(mag, mag, mag);
        }
    }
}
