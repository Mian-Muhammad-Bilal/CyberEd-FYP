using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] public Transform prevRoom;
    [SerializeField] public Transform nextRoom;
    [SerializeField] public CameraControl cam;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(collision.transform.position.x < transform.position.x)
                {
                    cam.MovToNewRoom(nextRoom);
                }
        }
    }
}
