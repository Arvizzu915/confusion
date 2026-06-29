using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public bool PlayerInside { get; private set; }

    private Door door;

    public void Initialize(Door d)
    {
        door = d;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerInside = false;
    }
}