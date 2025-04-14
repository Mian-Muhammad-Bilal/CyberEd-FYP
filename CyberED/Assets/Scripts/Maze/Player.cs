using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int keys = 0;
    public float speed = 5.0f;
    public GameObject door;
    public Text keyAmount;
    public Text youwin;

    private Vector2 moveDirection = Vector2.zero;

    void Update()
    {
        // Move the player continuously in the current direction
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    // Movement functions for button events
    public void MoveUp()
    {
        moveDirection = Vector2.up;
    }

    public void MoveDown()
    {
        moveDirection = Vector2.down;
    }

    public void MoveLeft()
    {
        moveDirection = Vector2.left;
    }

    public void MoveRight()
    {
        moveDirection = Vector2.right;
    }

    public void StopMovement()
    {
        moveDirection = Vector2.zero;
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
        if (collision.gameObject.tag == "Server")
        {
            youwin.text = "You Hacked into the Server!!";
        }
        if (collision.gameObject.tag == "Enemies")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (collision.gameObject.CompareTag("Door") && keys >= 3)
        {
            Destroy(collision.gameObject);
        }
    }
}
