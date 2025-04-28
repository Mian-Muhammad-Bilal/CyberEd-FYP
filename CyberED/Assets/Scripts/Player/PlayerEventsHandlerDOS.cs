using UnityEngine;
using DOS;

namespace DOS
{
    public class PlayerEventsHandlerDOS : MonoBehaviour
    {
        public void OnLand()
        {
            Debug.Log("Player has landed!");
            // You can trigger animation or sound here
        }

        public void OnCrouch(bool isCrouching)
        {
            Debug.Log("Crouch Status: " + isCrouching);
            // You can trigger crouch animation or logic here
        }
    }
}