using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


using UnityEngine.UI; // For handling UI (color changes)

public class DragDrop : MonoBehaviour
{
    public GameObject ObjectToDrag;
    public GameObject ObjectDragToPos;
    public float DropDistance;
    public bool isLocked;
    public Button submitButton; // Reference to the submit button

    // public Color correctColor = Color.green; // Color for correct placement
    // public Color wrongColor = Color.red;    // Color for wrong placement
    // private Color originalColor;             // To store original color of the object

    Vector2 ObjectInitPos;
    // public void level1()
    // {
    //     SceneManager.LoadSceneAsync(1);
    // }
//  public void level2()
//     {
//         SceneManager.LoadSceneAsync(2);
//     }
//     public void level3()
//     {
//         SceneManager.LoadSceneAsync(3);
//     }
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ObjectInitPos = ObjectToDrag.transform.position;
        // originalColor = ObjectToDrag.GetComponent<SpriteRenderer>().color; // Save original color
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(OnSubmit); // Add listener for submit button click
        }
    }

    // This method handles dragging the object
    public void DragObject()
    {
        if (!isLocked)
        {
            ObjectToDrag.transform.position = Input.mousePosition;
        }
    }

    // This method handles dropping the object
    public void DropObject()
    {
        float Distance = Vector3.Distance(ObjectToDrag.transform.position, ObjectDragToPos.transform.position);
        if (Distance < DropDistance)
        {
            isLocked = true;
            ObjectToDrag.transform.position = ObjectDragToPos.transform.position;
        }
        else
        {
            ObjectToDrag.transform.position = ObjectInitPos;
        }
    }

    // This method is called when the submit button is clicked
   void OnSubmit()
{
    DragDrop[] allDragDrops = Object.FindObjectsByType<DragDrop>(FindObjectsSortMode.None);// Find all draggable objects in scene

    foreach (DragDrop item in allDragDrops)
    {
        if (!item.isLocked) // If any object is not placed correctly, stop
        {
            Debug.Log("Complete the game first!");
            return;
        }
    }

    // If all are placed correctly, load next level
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
}

}
