using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Dictionary<GameObject, GameObject> rotatedObjects;
    public static List<GameObject> rotators;

    void Start()
    {
        if (rotators == null)
        {
            rotators = new List<GameObject>();
        }

        rotators.Add(gameObject);
        rotatedObjects = new Dictionary<GameObject, GameObject>();
    }

    void Update()
    {
        // Remove any objects that have been destroyed
        List<GameObject> toRemove = new List<GameObject>();
        foreach (GameObject obj in rotatedObjects.Keys)
        {
            if (obj == null)
            {
                toRemove.Add(obj);
            }
        }

        foreach (GameObject obj in toRemove)
        {
            RemoveReflectedObject(obj);
        }

        // Update the reflected objects
        foreach (GameObject obj in rotatedObjects.Keys)
        {
            Rotate(obj);
        }
    }

    public void Rotate(GameObject obj)
    {

        // Get the positions
        Vector2 reflectorPos = transform.position;
        Vector2 objPosition = obj.transform.position;

        // Get the reflected vector (based at the origin)
        Vector3 reflectorToObj = objPosition - reflectorPos;
        Vector2 reflectPos = (Vector2) (transform.rotation * reflectorToObj) + reflectorPos;

        // Set the rotation of the reflected object
        //Quaternion reflectRot = obj.transform.rotation * newRot;
        Quaternion reflectRot = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + obj.transform.rotation.eulerAngles.z);

        if (rotatedObjects.ContainsKey(obj))
        {
            rotatedObjects[obj].transform.position = reflectPos;
            rotatedObjects[obj].transform.rotation = reflectRot;
        } else {
            CreateRotatedObject(obj, reflectPos, reflectRot);
        }
    }

    public void CreateRotatedObject(GameObject obj, Vector2 pos, Quaternion rot)
    {
        GameObject rotatedObject = Instantiate(obj, pos, rot);
        rotatedObjects.Add(obj, rotatedObject);
        rotatedObject.GetComponent<Reflectable>().UpdateReflect();
    }

    public void RemoveReflectedObject(GameObject obj)
    {
        if (rotatedObjects.ContainsKey(obj))
        {
            Destroy(rotatedObjects[obj]);
            rotatedObjects.Remove(obj);
        }
    }

    void OnDestroy()
    {
        // Delete all reflected objects
        foreach (GameObject obj in rotatedObjects.Values)
        {
            Destroy(obj);
        }

        // Remove the reflector from the list of reflectors
        rotators.Remove(gameObject);

        // Destroy the reflector
        Destroy(gameObject);
    }
}

