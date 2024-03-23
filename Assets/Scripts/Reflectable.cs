using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class Reflectable : MonoBehaviour
{
    public int reflections = 0;
    public static int maxReflections = 2;
    private List<Color> colors = new List<Color> { Color.yellow, Color.red };

    void Start()
    {
        // Set the tag
        gameObject.tag = "Reflectable";
    }

    // When the object is reflected, increment the reflections
    public void UpdateReflect()
    {
        reflections++;

        // Change the color of the object
        GetComponent<SpriteRenderer>().color = colors[reflections - 1];
    }
}
