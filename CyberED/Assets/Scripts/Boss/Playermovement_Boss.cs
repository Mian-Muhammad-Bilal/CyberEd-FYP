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
    private bool isButtonPressed = false; // Tracks if on-screen button is pressed

    public GameObject bulletPrefab; // Assign in Inspector
    public Transform firePoint;    // Assign in Inspector (where the bullet spawns)

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
            isButtonPressed = true;
        }
        else
        {
            moveDirection = 0f;
            isButtonPressed = false;
        }
    }

    public void MoveRight(bool isPressed)
    {
        if (isPressed)
        {
            moveDirection = 1f;
            isButtonPressed = true;
        }
        else
        {
            moveDirection = 0f;
            isButtonPressed = false;
        }
    }
   private void Update()
{
    // Keyboard input always available
    float input = 0f;
    if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) input -= 1f;
    if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) input += 1f;

    bool keyboardActive = input != 0f;

    // If keyboard is active, update moveDirection
    if (keyboardActive) {
        moveDirection = input;
    }
    // If neither keyboard nor on-screen button is active, stop
    if (!keyboardActive && !isButtonPressed) {
        moveDirection = 0f;
    }

    // Jump with spacebar
    if (Input.GetKeyDown(KeyCode.Space)) {
        Jump();
    }
    // Fire with Z
    if (Input.GetKeyDown(KeyCode.Z)) {
        Fire();
    }

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
            body.linearVelocity = new Vector2(body.linearVelocity.x, 6f); // Lowered jump force
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

    public void Fire()
    {
        Debug.Log("Fire() called");
        // If a PrefabWeapon component is present, use it
        var weapon = GetComponent<PrefabWeapon>();
        if (weapon != null)
        {
            weapon.Shoot();
            return;
        }
        // Otherwise, fallback to direct instantiation
        if (bulletPrefab != null && firePoint != null)
        {
            // Make sure firePoint's right vector points in the direction you want the bullet to go
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}