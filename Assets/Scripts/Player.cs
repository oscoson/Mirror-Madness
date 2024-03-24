using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D playerCollider;
    bool isGrounded = true;
    [SerializeField] Collider2D groundTrigger;
    [SerializeField] Collider2D leftSideTrigger;
    [SerializeField] Collider2D rightSideTrigger;
    [SerializeField] GameObject reflector;
    [SerializeField] GameObject rotator;

    Animator playerAnimator;

    bool pressedJump = false;
    float horizontalSpeedCap = 5.0f;
    float verticalWallJumpSpeed = 6.0f;
    float horizontalWallJumpSpeed = 4.0f;

    string currentPlayerScene;

    // Access the MiraManager to find what the active gadget is
    



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            pressedJump = true;
        }

        GameObject miraManagerObject = GameObject.Find("CreateMira");
        MiraManager miraManagerScript = miraManagerObject.GetComponent<MiraManager>();
        string currentTool = miraManagerScript.currentTool;

        if (currentTool == "Reflector")
        {
            SetTextColor(reflector, Color.white);
            SetTextColor(rotator, Color.gray);
        }
        else if (currentTool == "Rotator")
        {
            SetTextColor(reflector, Color.gray);
            SetTextColor(rotator, Color.white);
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
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            horizontalDirection += -1f;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            horizontalDirection += 1f;
        }
        if (Input.GetKey(KeyCode.R))
        {
            PlayerPrefs.SetInt("NoTransition", 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (isGrounded)
        {
            playerAnimator.SetBool("isJumping", false);
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
            playerAnimator.SetBool("isJumping", true);
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
        playerAnimator.SetBool("isRunning", Mathf.Abs(horizontalDirection) > Mathf.Epsilon);
        
        vel.x = Mathf.Clamp(vel.x, -horizontalSpeedCap, horizontalSpeedCap);
        rb.velocity = vel;   
        
        pressedJump = false;
    }


    void SetTextColor(GameObject target, Color color)
    {
        Transform textTransform = target.transform.Find("Text");
        
        if (textTransform != null)
        {
            TextMeshProUGUI textMeshPro = textTransform.GetComponent<TextMeshProUGUI>();

            if (textMeshPro != null)
            {
                textMeshPro.color = color;
            }
            else
            {
                Debug.LogWarning("TextMeshPro component not found on the 'Text' GameObject of " + target.name);
            }
        }
        else
        {
            Debug.LogWarning("Child GameObject named 'Text' not found under the " + target.name + " GameObject");
        }
    }


    private bool IsTouchingLeft()
    {
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();
        if (leftSideTrigger.OverlapCollider(filter, results) > 0)
        {
            foreach (Collider2D col in results)
            {
                if (col != playerCollider && (col.gameObject.tag == "Reflectable" ||  col.gameObject.tag == "UnReflectable"))
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
                if (col != playerCollider && (col.gameObject.tag == "Reflectable" ||  col.gameObject.tag == "UnReflectable"))
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
                if (col != playerCollider && (col.gameObject.tag == "Reflectable" ||  col.gameObject.tag == "UnReflectable"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.CompareTag("Hazard"))
        {
            transform.position = GameObject.FindWithTag("Spawn").transform.position;
        }
    }

}
