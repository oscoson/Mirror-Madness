using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class ReflectZone : MonoBehaviour
{
    
    SpriteRenderer sr;
    
    public List<Collider2D> touching;

    private Reflector reflector;


    void Start()
    {
        touching = new List<Collider2D>();
        reflector = GetComponentInParent<Reflector>();

        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(0, 0, 0, 0.25f);

        // Set the size of the shadow to be a square with length the same as the x scale of the parent
        transform.localScale = new Vector3(1, transform.parent.localScale.x / transform.parent.localScale.y, 1);

        // Sit on the bottom of the parent
        transform.localPosition = new Vector3(0, -transform.localScale.y / 2 - 1, 0);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Reflectable" && 
            collider.gameObject.GetComponent<Reflectable>().reflections < Reflectable.maxReflections && 
            !touching.Contains(collider))
        {
            touching.Add(collider);
            reflector.Reflect(collider.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Reflectable" && touching.Contains(collider))
        {
            touching.Remove(collider);
            reflector.RemoveReflectedObject(collider.gameObject);
        }
    }

}
