using UnityEngine;

public class Chest : MonoBehaviour, IInteractuable
{
    [SerializeField] private Transform spawnPlace;
    [SerializeField] private GameObject obj;

    private bool closed = true;

    [SerializeField] private Animator animator;

    public void OpenChest()
    {
        animator.Play("Open");
        

        GameObject objSpawned = Instantiate(obj, spawnPlace.position, Quaternion.identity);

        // Apply physics on the server — this syncs automatically
        if (objSpawned.TryGetComponent(out Rigidbody rb))
        {
            rb.WakeUp();
            rb.AddForce((-transform.forward * 2) + Vector3.up * 7, ForceMode.Impulse);
        }
    }

    public void Hover(PlayerUIManager UIManager)
    {
        UIManager.CanInteract(closed, "Open");
    }

    public void Interact(PlayerManager player)
    {
        if (!closed)
            return;

        closed = false;

        OpenChest();
    }
}
