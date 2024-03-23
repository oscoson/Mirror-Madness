using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectMirror : MonoBehaviour
{
    public Transform objectToReflect;
    public GameObject reflectedObject;

    void Update()
    {
        // If the object to reflect is null, kill the reflected object
        if (!objectToReflect)
        {
            print("Object to reflect is null");
            if (reflectedObject)
            {
                Destroy(reflectedObject);
            }
            return;
        }

        // If the object to reflect is null, define a new object to reflect
        if (!reflectedObject) {
            reflectedObject = Instantiate(objectToReflect.gameObject);
            SpriteRenderer sr = reflectedObject.GetComponent<SpriteRenderer>();
            sr.color = new Color(1, 0, 0, 0.5f);
            sr.flipY = true;
        }

        // Get all objects in a square 
        
        // Get the angle relative to the x-axis
        Vector2 axis = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z) * Vector2.right;

        // Get the positions
        Vector2 mirrorPos = transform.position;
        Vector2 objPosition = objectToReflect.transform.position;

        // Get the reflected vector (based at the origin)
        Vector2 reflectVec = ReflectAgainstAxis(objPosition - mirrorPos, axis);

        // Move the reflected object to the reflected position
        reflectedObject.transform.position = reflectVec + mirrorPos;

        // Rotate the reflected object (black magic)
        Quaternion newRot = Quaternion.FromToRotation(objectToReflect.transform.right, ReflectAgainstAxis(objectToReflect.transform.right, axis));

        // Set the rotation of the reflected object
        reflectedObject.transform.rotation = objectToReflect.transform.rotation;
        reflectedObject.transform.rotation *= newRot;

    }

    Vector2 ReflectAgainstAxis(Vector2 vec, Vector2 axis)
    {
        Vector2 normAxis = axis.normalized;
        Vector2 projVec = Vector2.Dot(vec, normAxis) * normAxis;
        return 2f * projVec - vec;
    }
}

