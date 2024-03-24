using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MiraManager : MonoBehaviour
{
    [SerializeField] private int maxReflectorCount = 2;
    [SerializeField] private int maxRotatorCount = 2;
    [SerializeField] AudioClip place1AudioClip;
    [SerializeField] AudioClip place2AudioClip;
    [SerializeField] AudioSource audioSource;

    private int reflectorCount = 0;
    private int rotatorCount = 0;

    public int MaxReflectorCount { get { return maxReflectorCount; } }
    public int MaxRotatorCount { get { return maxRotatorCount; } }

    public int ReflectorCount { get { return reflectorCount; } }
    public int RotatorCount { get { return rotatorCount; } }

    private float minReflectLen = 0.05f;

    private GameObject reflectorPreview;
    private GameObject rotatorPreview;


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

        reflectorPreview = transform.GetComponentInChildren<ReflectorPreview>().gameObject;
        rotatorPreview = transform.GetComponentInChildren<RotatorPreview>().gameObject;

        SpriteRenderer refsr = reflectorPreview.transform.GetChild(0).GetComponent<SpriteRenderer>();
        refsr.color = new Color(0, 0, 0, 0.25f);

        SpriteRenderer rotsr = rotatorPreview.transform.GetChild(0).GetComponent<SpriteRenderer>();
        rotsr.color = new Color(0, 0, 0, 0.25f);

        reflectorPreview.SetActive(false);
        rotatorPreview.SetActive(false);
    }

    private void Start()
    {
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
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

            placing = !rotating;

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
                            audioSource.PlayOneShot(place1AudioClip);
                            // Place the reflector
                            PlaceReflector(startPos, mousePos);                            
                        }
                    }
                    else if (currentTool == "Rotator")
                    {
                        if (rotatorCount < maxRotatorCount)
                        {
                            audioSource.PlayOneShot(place2AudioClip);
                            // Place the rotator
                            PlaceRotator(startPos, mousePos);
                        }
                    }
                }

                placing = false;
            }
        }

        // Listen for a right mouse down
        if (Input.GetMouseButtonDown(1))
        {
            // Get the mouse position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            startPos = mousePos;
            placing = true;
        }

        // Listen for a right mouse up
        if (Input.GetMouseButtonUp(1) && placing)
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
            placing = false;
        }


        if (!placing)
        {
            reflectorPreview.SetActive(false);
            rotatorPreview.SetActive(false);
        }
        else
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (currentTool == "Reflector")
            {
                reflectorPreview.SetActive(true);
                rotatorPreview.SetActive(false);

                reflectorPreview.transform.position = (mousePos - startPos) * 0.5f + startPos;
                // Set the scale of the reflector
                reflectorPreview.transform.localScale = new Vector3(Vector2.Distance(startPos, mousePos), REFLECTOR_WIDTH, 1);

                int inverse = Input.GetMouseButton(0) ? 1 : -1;
                // Set the rotation of the reflector
                reflectorPreview.transform.rotation = Quaternion.FromToRotation(Vector2.right, inverse * (mousePos - startPos));



                Transform shadow = reflectorPreview.transform.GetChild(0);

                // Set the size of the shadow to be a square with length the same as the x scale of the parent
                shadow.transform.localScale = new Vector3(1, reflectorPreview.transform.localScale.x / reflectorPreview.transform.localScale.y, 1);

                // Sit on the bottom of the parent
                shadow.transform.localPosition = new Vector3(0, -shadow.transform.localScale.y / 2 - 1, 0);


            }
            else if (currentTool == "Rotator")
            {
                rotatorPreview.SetActive(true);
                reflectorPreview.SetActive(false);

                rotatorPreview.transform.position = startPos;


                // Set the scale of the rotator
                Vector2 dist = (mousePos - startPos) * 2;
                float mag = dist.magnitude;

                // Get the transform of the child
                Transform shadow = rotatorPreview.transform.GetChild(0);

                // Set the scale of the child
                shadow.localScale = new Vector3(mag, mag, mag);
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
