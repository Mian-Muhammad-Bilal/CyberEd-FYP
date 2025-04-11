using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float speed = 10f;
    public float CurPosX;
    private Vector3 velocity = Vector3.zero;
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(CurPosX, transform.position.y, transform.position.z),ref velocity, speed);
    }
    public void MovToNewRoom(Transform _newRoom)
    {
        CurPosX = _newRoom.position.x;
    }
}
