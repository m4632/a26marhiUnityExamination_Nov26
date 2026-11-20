
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 10f;
    public float acceleration = 50f;

    [Header("Jump")]
    public float jumpForce = 17f;
    public float gravityScale = 2f;
    public float fallMultiplier = 3f;

    [Header("Dash")]
    public float dashSpeed = 22f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.6f;

    private Rigidbody2D rb;

    private float moveInput;
    private int facingDirection = 1;

    private bool isGrounded;
    private bool isDashing;
    private bool canDash = true;
    private bool jumpPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set gravity through script
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // MOVEMENT INPUT
        moveInput = 0;

        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            moveInput = -1;
        }

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput = 1;
        }

        // FACING DIRECTION
        if (moveInput != 0)
        {
            facingDirection = moveInput > 0 ? 1 : -1;
        }

        // JUMP
        if (Keyboard.current.spaceKey.wasPressedThisFrame &&
            isGrounded && !isDashing)
        {
            jumpPressed = true;
        }

        // DASH
        if (Keyboard.current.leftCtrlKey.wasPressedThisFrame &&
            canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        // WALK AND SPRINT
        bool sprinting = Keyboard.current != null &&
            (Keyboard.current.leftShiftKey.isPressed ||
             Keyboard.current.rightShiftKey.isPressed);

        float speed = sprinting ? sprintSpeed : walkSpeed;

        float targetSpeed = moveInput * speed;

        float newSpeed = Mathf.MoveTowards(
            rb.linearVelocity.x,
            targetSpeed,
            acceleration * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector2(
            newSpeed,
            rb.linearVelocity.y
        );

        // JUMP
        if (jumpPressed)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            isGrounded = false;
            jumpPressed = false;
        }

        // STRONGER FALLING GRAVITY
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up *
                Physics2D.gravity.y *
                (fallMultiplier - 1) *
                Time.fixedDeltaTime;
        }
    }

    // DASH
    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        float originalGravity = rb.gravityScale;

        rb.gravityScale = 0;

        rb.linearVelocity = new Vector2(
            facingDirection * dashSpeed,
            0
        );

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    // GROUND DETECTION
    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}


