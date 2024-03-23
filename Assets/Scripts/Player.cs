using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D playerCollider;
    bool isGrounded = true;
    [SerializeField] Collider2D groundTrigger;
    [SerializeField] Collider2D leftSideTrigger;
    [SerializeField] Collider2D rightSideTrigger;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        bool isGrounded = IsGrounded();
        bool isTouchingLeft = IsTouchingLeft();
        bool isTouchingRight = IsTouchingRight();

        Vector2 vel = rb.velocity;

        if (isGrounded)
        {
            Vector2 horizontalDirection = Vector2.zero;

            float coefficient = 1.0f;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalDirection += Vector2.left;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontalDirection += Vector2.right;
            }

            if (math.sign(horizontalDirection.x) != math.sign(vel.x))
            {
                coefficient = 2.0f;
            }

            rb.AddForce(coefficient * horizontalDirection * 20.0f);

            if (vel.x < -5.0f)
            {
                vel.x = -5.0f;
            }
            if (vel.x > 5.0f)
            {
                vel.x = 5.0f;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                vel.y = 4.0f;
                Debug.Log("jump");
            }
        }

        if (isTouchingLeft)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                vel.y = 4.0f;
                vel.x = 4.0f;
            }
        }

        if (isTouchingRight)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                vel.y = 4.0f;
                vel.x = -4.0f;
            }
        }
        rb.velocity = vel;

    }
    private bool IsTouchingLeft()
    {
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();
        if (leftSideTrigger.OverlapCollider(filter, results) > 0)
        {
            foreach (Collider2D col in results)
            {
                if (col != playerCollider)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsTouchingRight()
    {
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();
        if (rightSideTrigger.OverlapCollider(filter, results) > 0)
        {
            foreach (Collider2D col in results)
            {
                if (col != playerCollider)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsGrounded()
    {
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();
        if (groundTrigger.OverlapCollider(filter, results) > 0)
        {
            foreach (Collider2D col in results)
            {
                if (col != playerCollider)
                {
                    return true;
                }
            }
        }

        return false;
    }

}
