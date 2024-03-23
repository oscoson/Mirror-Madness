using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMira : MonoBehaviour
{
    private Vector2 startPos;
    public GameObject reflectorPrefab;
    public float MIRA_WIDTH = 0.2f;
    public bool placing = false;

    void Update()
    {

        
        // Listen for a left mouse down
        if (Input.GetMouseButtonDown(0))
        {
            placing = true;

            // Get the mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

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

            // Place the reflector
            PlaceReflector(startPos, mousePos);
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
            reflector.transform.localScale = new Vector3(Vector2.Distance(startPos, endPos), MIRA_WIDTH, 1);

            // Set the rotation of the reflector
            reflector.transform.rotation = Quaternion.FromToRotation(Vector2.right, inverse * (endPos - startPos));
        }
    }
}
