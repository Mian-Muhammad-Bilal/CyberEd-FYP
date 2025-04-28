using UnityEngine;
using DOS;

namespace DOS
{
    public class PlayerMovementDOS : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float jumpHeight = 20f;
        public Rigidbody2D rb;
        public Animator animator;

        private bool isGrounded = false;
        private bool jumpPressed = false;
        private float horizontalMove = 0f;

        // For tracking the direction the player is facing
        public bool isFacingRight = true; 

        public GameObject bulletPrefab; // Reference to the bullet prefab

        void Update()
        {
            // Update animations
            animator.SetBool("IsJumping", !isGrounded);
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            // Flip the player sprite based on movement direction
            if (horizontalMove < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Flip to left
                isFacingRight = false; // Player is facing left
            }
            else if (horizontalMove > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Flip to right
                isFacingRight = true; // Player is facing right
            }

            // Fire bullet on spacebar (or any other button)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireBullet();
            }
        }

        void FixedUpdate()
        {
            // Move the player
            rb.linearVelocity = new Vector2(horizontalMove * moveSpeed, rb.linearVelocity.y);

            // Handle jump
            if (jumpPressed && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
                jumpPressed = false;
                isGrounded = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }

        // UI button controls
        public void OnLeftDown() { horizontalMove = -1f; }
        public void OnLeftUp() { if (horizontalMove < 0) horizontalMove = 0f; }
        public void OnRightDown() { horizontalMove = 1f; }
        public void OnRightUp() { if (horizontalMove > 0) horizontalMove = 0f; }
        public void OnJumpButtonPressed() { jumpPressed = true; }

        // Fire the bullet
        public void FireBullet()
        {
            // Instantiate the bullet at the player's position
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // Set bullet's velocity based on the player's facing direction
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            // If the player is facing right, fire the bullet to the right
            if (isFacingRight)
            {
                bulletRb.linearVelocity = Vector2.right * 20f; // Bullet moves right
            }
            else
            {
                bulletRb.linearVelocity = Vector2.left * 20f; // Bullet moves left
            }

            // Optional: Rotate the bullet to match the player's facing direction if needed
            bullet.transform.rotation = isFacingRight ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        }
    }
}
