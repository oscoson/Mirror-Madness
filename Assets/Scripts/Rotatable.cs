using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatable : MonoBehaviour
{
    public int rotations = 0;
    public static int maxRotations = 1;
    private List<Color> colors = new List<Color> { Color.yellow, Color.red };

    void Start()
    {
        // Set the tag
        gameObject.tag = "Reflectable";
    }

    // When the object is reflected, increment the reflections
    public void UpdateRotate()
    {
        rotations++;

        // Change the color of the object
        GetComponent<SpriteRenderer>().color = colors[rotations - 1];
    }
}
