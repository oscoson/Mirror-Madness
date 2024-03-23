using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class Reflector : MonoBehaviour
{
    public Dictionary<GameObject, GameObject> reflectedObjects;
    private static List<GameObject> reflectors;

    void Start()
    {
        if (reflectors == null)
        {
            reflectors = new List<GameObject>();
        }

        reflectors.Add(gameObject);
        reflectedObjects = new Dictionary<GameObject, GameObject>();
    }

    void Update()
    {
        // Remove any objects that have been destroyed
        List<GameObject> toRemove = new List<GameObject>();
        foreach (GameObject obj in reflectedObjects.Keys)
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
        foreach (GameObject obj in reflectedObjects.Keys)
        {
            Reflect(obj);
        }
    }

    public void Reflect(GameObject obj)
    {
        // Get the angle relative to the x-axis
        Vector2 axis = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z) * Vector2.right;

        // Get the positions
        Vector2 reflectorPos = transform.position;
        Vector2 objPosition = obj.transform.position;

        // Get the reflected vector (based at the origin)
        Vector2 reflectVec = ReflectAgainstAxis(objPosition - reflectorPos, axis);

        // Move the reflected object to the reflected position
        Vector2 reflectPos = reflectVec + reflectorPos;

        // Rotate the reflected object (black magic)
        Quaternion newRot = Quaternion.FromToRotation(obj.transform.right, ReflectAgainstAxis(obj.transform.right, axis));

        // Set the rotation of the reflected object
        Quaternion reflectRot = obj.transform.rotation * newRot;

        if (reflectedObjects.ContainsKey(obj))
        {
            reflectedObjects[obj].transform.position = reflectPos;
            reflectedObjects[obj].transform.rotation = reflectRot;
        } else {
            CreateReflectedObject(obj, reflectPos, reflectRot);
        }
    }

    public void CreateReflectedObject(GameObject obj, Vector2 pos, Quaternion rot)
    {
        GameObject reflectedObject = Instantiate(obj, pos, rot);
        reflectedObjects.Add(obj, reflectedObject);
        reflectedObject.GetComponent<Reflectable>().UpdateReflect();
    }

    Vector2 ReflectAgainstAxis(Vector2 vec, Vector2 axis)
    {
        Vector2 normAxis = axis.normalized;
        Vector2 projVec = Vector2.Dot(vec, normAxis) * normAxis;
        return 2f * projVec - vec;
    }

    public void RemoveReflectedObject(GameObject obj)
    {
        if (reflectedObjects.ContainsKey(obj))
        {
            Destroy(reflectedObjects[obj]);
            reflectedObjects.Remove(obj);
        }
    }
}

