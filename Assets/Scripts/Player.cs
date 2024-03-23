using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D playerCollider;
    bool isGrounded = true;
    [SerializeField] Collider2D groundTrigger;
    [SerializeField] Collider2D leftSideTrigger;
    [SerializeField] Collider2D rightSideTrigger;

    bool pressedJump = false;
    float horizontalSpeedCap = 5.0f;
    float verticalWallJumpSpeed = 4.0f;
    float horizontalWallJumpSpeed = 4.0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            pressedJump = true;
        }
    }

    private void FixedUpdate()
    {
        bool isGrounded = IsGrounded();
        bool isTouchingLeft = IsTouchingLeft();
        bool isTouchingRight = IsTouchingRight();

        Vector2 vel = rb.velocity;

        float horizontalDirection = 0.0f;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            horizontalDirection += -1f;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            horizontalDirection += 1f;
        }

        if (isGrounded)
        {
            float coefficient = 0.5f;

            if (horizontalDirection >= 0f && vel.x < 0f)
            {
                if (horizontalDirection == 0f)
                {
                    vel.x = Mathf.MoveTowards(vel.x, 0f, coefficient);
                }
                else
                {
                    vel.x += coefficient;
                }
            }
            else if (horizontalDirection <= 0f && vel.x > 0f)
            {
                if (horizontalDirection == 0f)
                {
                    vel.x = Mathf.MoveTowards(vel.x, 0f, coefficient);
                }
                else
                {
                    vel.x += -coefficient;
                }
            }
            else
            {
                vel.x += coefficient * horizontalDirection;
            }

            if (pressedJump)
            {
                vel.y = verticalWallJumpSpeed;
            }
        }
        else
        {
            float coefficient = 0.1f;
            if (horizontalDirection > 0f)
            {
                vel.x += coefficient;
            }
            else if (horizontalDirection < 0f)
            {
                vel.x += -coefficient;
            }

            if (isTouchingLeft)
            {
                if (pressedJump)
                {
                    vel.y = verticalWallJumpSpeed;
                    vel.x = horizontalWallJumpSpeed;
                }
            }

            if (isTouchingRight)
            {
                if (pressedJump)
                {
                    vel.y = verticalWallJumpSpeed;
                    vel.x = -horizontalWallJumpSpeed;
                }
            }

        }

        vel.x = Mathf.Clamp(vel.x, -horizontalSpeedCap, horizontalSpeedCap);

        rb.velocity = vel;

        pressedJump = false;
    }
    private bool IsTouchingLeft()
    {
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();
        if (leftSideTrigger.OverlapCollider(filter, results) > 0)
        {
            foreach (Collider2D col in results)
            {
                if (col != playerCollider && col.gameObject.tag == "Reflectable")
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
                if (col != playerCollider && col.gameObject.tag == "Reflectable")
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
                if (col != playerCollider && col.gameObject.tag == "Reflectable")
                {
                    return true;
                }
            }
        }

        return false;
    }

}
