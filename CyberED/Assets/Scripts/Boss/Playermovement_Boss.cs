using UnityEngine;

public class Playermovement_Boss : MonoBehaviour
{
    // public Transform groundCheck;
    // public float groundCheckRadius = 0.1f;
    // public LayerMask groundLayer;

    public float speed = 8f;
    private Rigidbody2D body;
    // private Animator anim;
    public bool hasJumped = false;

    // Ladder variables
    private float vertical;
    private bool isLadder;
    private bool isClimbing;
    public float climbSpeed = 2.5f;
    public float moveDirection = 0f;
    private bool grounded;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        // anim = GetComponent<Animator>();
    }

    // Methods to be called on button press/release
    public void MoveLeft(bool isPressed)
{
    if (isPressed)
    {
        moveDirection = -1f;
    }
    else
    {
        moveDirection = 0f;
    }
}

    public void MoveRight(bool isPressed)
    {
     if (isPressed)
    {
        moveDirection = 1f;
    }
    else
    {
        moveDirection = 0f;
    }   
}
   private void Update()
{
    // if (body == null || anim == null) return; // Skip if not on the Player

    // grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

    // Apply horizontal movement
    body.linearVelocity = new Vector2(moveDirection * speed, body.linearVelocity.y);
    // anim.SetBool("run", moveDirection != 0f);

    // Player flipping
    if (moveDirection > +0.01f)
    {
        // Face right
        transform.localScale = new Vector2(1f, 1f);
    }
    else if (moveDirection < -0.01f)
    {
        // Face left
        transform.localScale = new Vector2(-1f, 1f);
    }

    // Climbing logic
    if (isLadder && Mathf.Abs(vertical) > 0f)
    {
        isClimbing = true;
    }

    // Animation handling
    // anim.SetBool("grounded", grounded);

    // Handle idle animation when the player is not moving or climbing
    // if (grounded && moveDirection == 0 && !isClimbing)
    // {
    //     anim.SetBool("idle", true);
    // }
    // else
    // {
    //     anim.SetBool("idle", false);
    // }
}


    private void FixedUpdate()
    {
        if (body == null) return; // Ensure Rigidbody2D exists

        if (isClimbing)
        {
            body.gravityScale = 0f; // Disable gravity while climbing
            body.linearVelocity = new Vector2(0, vertical * climbSpeed);
        }
        else
        {
            body.gravityScale = 2.7f; // Reset gravity when not climbing
        }
    }


    public void Jump()
    {
        if (!grounded || hasJumped) return;

        // if (isLadder)
        // {
        //     // Climb the ladder when on it
        //     vertical = 1.5f;
        // }
        else if (!hasJumped && grounded)
        {
            // Regular jump
            body.linearVelocity = new Vector2(body.linearVelocity.x, 10f);
            // anim.SetTrigger("jump");
            hasJumped = true;
            grounded = false;
        }
    }

    // public void StopClimbing()
    // {
    //     vertical = 0f;
    //     isClimbing = false;
    // }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasJumped = false; // Allow jumping again when touching the ground
            grounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
}