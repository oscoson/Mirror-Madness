using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class MirrorShadow : MonoBehaviour
{
    
    SpriteRenderer sr;
    
    public List<Collider2D> touching;

    private ReflectMirror reflectMirror;


    void Start()
    {
        touching = new List<Collider2D>();
        reflectMirror = GetComponentInParent<ReflectMirror>();

        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(0, 0, 0, 0.25f);

        // Set the size of the shadow to be a square with length the same as the x scale of the parent
        transform.localScale = new Vector3(1, transform.parent.localScale.x / transform.parent.localScale.y, 1);

        // Sit on the bottom of the parent
        transform.localPosition = new Vector3(0, -transform.localScale.y / 2 - 1, 0);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        // Check if the collider is colliding with the reflectMirror
        Collider2D[] colliders = Physics2D.OverlapAreaAll(collider.bounds.min, collider.bounds.max);
        bool isCollidingWithReflectMirror = false;
        foreach (Collider2D col in colliders)
        {
            if (col.gameObject == reflectMirror.gameObject)
            {
                isCollidingWithReflectMirror = true;
                reflectMirror.RemoveReflectedObject(collider.gameObject);
                break;
            }
        }

        if (!isCollidingWithReflectMirror)
        {
            touching.Add(collider);
            reflectMirror.Reflect(collider.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Mirrorable" && touching.Contains(collider))
        {
            touching.Remove(collider);
            reflectMirror.RemoveReflectedObject(collider.gameObject);
        }
    }

}
