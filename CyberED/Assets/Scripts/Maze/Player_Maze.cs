using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Maze : MonoBehaviour
{
    public int keys = 0;
    public float speed = 5.0f;
    public GameObject door;
    public Text keyAmount;
    public Text youwin;
    public GameObject nextLevelButton; // Assign in Inspector


    private Vector2 moveDirection = Vector2.zero;
    private bool isButtonPressed = false; // Tracks if on-screen button is pressed
    void Start()
    {
        nextLevelButton.SetActive(false); // Hide the button at the start
    }
    void Update()
    {
        // Keyboard input always available
        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) input += Vector2.up;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) input += Vector2.down;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) input += Vector2.left;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) input += Vector2.right;

        bool keyboardActive = input != Vector2.zero;

        // If keyboard is active, update moveDirection
        if (keyboardActive) {
            moveDirection = input.normalized;
        }
        // If neither keyboard nor on-screen button is active, stop
        if (!keyboardActive && !isButtonPressed) {
            moveDirection = Vector2.zero;
        }

        // Move the player continuously in the current direction
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    // Movement functions for button events
    public void MoveUp()
    {
        moveDirection = Vector2.up;
        isButtonPressed = true;
    }

    public void MoveDown()
    {
        moveDirection = Vector2.down;
        isButtonPressed = true;
    }

    public void MoveLeft()
    {
        moveDirection = Vector2.left;
        isButtonPressed = true;
    }

    public void MoveRight()
    {
        moveDirection = Vector2.right;
        isButtonPressed = true;
    }

    public void StopMovement()
    {
        moveDirection = Vector2.zero;
        isButtonPressed = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Keys")
        {
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            keys++;
            keyAmount.text = "Keys: " + keys;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Gem")
        {
            youwin.text = "You Win!!!!!";

        }
        if (collision.gameObject.tag == "Enemies")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (collision.gameObject.CompareTag("Door") && keys >= 3)
        {
            Destroy(collision.gameObject);
            nextLevelButton.SetActive(true); // Show the button

        }
    }
     public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load next scene
    }
}
