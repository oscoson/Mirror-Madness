using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Laser : MonoBehaviour
{
    [SerializeField]
    float laserLength = 1000.0f;
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var rotVec = Quaternion.Euler(0, 0, transform.eulerAngles.z) * (laserLength * Vector3.right);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rotVec, laserLength, ~LayerMask.GetMask("Reflector"));
        lineRenderer.SetPosition(0, transform.position);

        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);

            // player layer
            if (hit.collider.gameObject.layer == 6)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + rotVec);
        }
    }
}
