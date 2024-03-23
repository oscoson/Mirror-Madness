using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZone : MonoBehaviour
{
    
    SpriteRenderer sr;
    
    public List<Collider2D> touching;

    private Rotator rotator;


    void Start()
    {
        touching = new List<Collider2D>();
        rotator = GetComponentInParent<Rotator>();

        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(0, 0, 0, 0.25f);

        // Set the size of the shadow to be a square with length the same as the x scale of the parent
        transform.localScale = 5f * Vector3.one;

        // Sit on the bottom of the parent
        transform.localPosition = new Vector3(0, 0, 0);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Reflectable" &&
            collider.gameObject.GetComponent<Rotatable>().rotations < Rotatable.maxRotations &&
            !touching.Contains(collider))
        {
            touching.Add(collider);
            rotator.Rotate(collider.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Reflectable" && touching.Contains(collider))
        {
            touching.Remove(collider);
            rotator.RemoveReflectedObject(collider.gameObject);
        }
    }

}
