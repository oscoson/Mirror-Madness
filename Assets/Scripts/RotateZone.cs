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
        rotator = GetComponentInParent<Rotator>();

        touching = new List<Collider2D>();

        // Grab all overlapped objects
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 2);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Reflectable" && collider.gameObject.GetComponent<Reflectable>().reflections < Reflectable.maxReflections && !touching.Contains(collider))
            {
                touching.Add(collider);
                rotator.Rotate(collider.gameObject);
            }
        }

        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(0, 0, 0, 0.25f);

        // Sit on the bottom of the parent
        transform.localPosition = new Vector3(0, 0, 0);
    }

}
