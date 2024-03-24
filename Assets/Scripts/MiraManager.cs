using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiraManager : MonoBehaviour
{
    [SerializeField] private int maxReflectorCount = 2;
    [SerializeField] private int maxRotatorCount = 2;

    private int reflectorCount = 0;
    private int rotatorCount = 0;

    public int MaxReflectorCount { get { return maxReflectorCount; } }
    public int MaxRotatorCount { get { return maxRotatorCount; } }

    public int ReflectorCount { get { return reflectorCount; } }
    public int RotatorCount { get { return rotatorCount; } }

    private float minReflectLen = 0.05f;

    private Vector2 startPos;
    public GameObject reflectorPrefab;
    public GameObject rotatorPrefab;
    public float REFLECTOR_WIDTH = 0.2f;
    public bool placing = false;
    private bool rotating = false;

    public string currentTool = "Reflector";

    List<Rotator> rotators;

    private void Awake()
    {
        rotators = new();
    }

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
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check if handle clicked
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, 0);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    Handle handle = hit.collider.gameObject.GetComponent<Handle>();
                    if (handle != null)
                    {
                        rotating = true;
                        rotators.Add(hit.collider.gameObject.GetComponentInParent<Rotator>());
                    }
                }
            }

            placing = true;

            // Get the mouse position

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
                        if (collider.gameObject.GetComponent<Reflector>() != null)
                        {
                            reflectorCount--;
                        }
                        else if (collider.gameObject.GetComponent<Rotator>() != null)
                        {
                            rotatorCount--;
                        }
                        Destroy(collider.gameObject);
                    }
                }
            }
        }

        if (rotating)
        {
            foreach (Rotator rotator in rotators)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (rotator != null)
                {
                    rotator?.RotateTowards(mousePos);
                }
            }
        }

        // Listen for a left mouse up
        if (Input.GetMouseButtonUp(0))
        {
            if (rotating)
            {
                rotators.Clear();
                rotating = false;
            }
            else if (placing)
            {
                // Get the mouse position
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if ((startPos - mousePos).magnitude >= minReflectLen)
                {
                    if (currentTool == "Reflector")
                    {
                        if (reflectorCount < maxReflectorCount)
                        {
                            // Place the reflector
                            PlaceReflector(startPos, mousePos);
                        }
                    }
                    else if (currentTool == "Rotator")
                    {
                        if (rotatorCount < maxRotatorCount)
                        {
                            // Place the rotator
                            PlaceRotator(startPos, mousePos);
                        }
                    }
                }
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

            if ((startPos - mousePos).magnitude >= minReflectLen)
            {
                // Place the reflector
                if (currentTool == "Reflector")
                {
                    if (reflectorCount < maxReflectorCount)
                    {
                        PlaceReflector(startPos, mousePos, -1);
                    }
                }
            }
        }
    }

    void PlaceReflector(Vector2 startPos, Vector2 endPos, int inverse = 1)
    {
        // Check if the prefab is not null
        if (reflectorPrefab != null)
        {
            reflectorCount++;
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
            rotatorCount++;
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
