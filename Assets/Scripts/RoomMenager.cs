using UnityEngine;
using UnityEngine.InputSystem;
public class RoomMenager : MonoBehaviour
{
    public Collider collider;
    public bool isPlayerInRoom = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRoom = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRoom = false;
        }
    }
   
    private void Update()
    {
    }
}
