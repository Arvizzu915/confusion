using System.Collections;
using UnityEngine;

public class Door : Interactuable
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private HingeJoint hinge;

    Quaternion initialLocalRotation;
    private bool isOpen = false;
    [SerializeField] float openTorque = 5f;

    [Header("Closed Settings")]
    [SerializeField] float autoCloseAngleThreshold = 5f;
    float openTimer = 0f;
    [SerializeField] float autoCloseDelay = 0.5f;

    private void Awake()
    {
        initialLocalRotation = transform.localRotation;

        SetClosedStateImmediate();
    }

    void Update()
    {
        if (!isOpen) return;

        openTimer += Time.deltaTime;

        if (openTimer < autoCloseDelay) return;

        float angle = Quaternion.Angle(transform.localRotation, initialLocalRotation);

        if (angle < autoCloseAngleThreshold && rb.angularVelocity.magnitude < 0.1f)
        {
            CloseDoor();
        }
    }

    public override void Interact(PlayerManager player)
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        openTimer = 0f;

        
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;

        // Disable spring so it's free
        var spring = hinge.spring;
        spring.spring = 0f;
        hinge.spring = spring;
        hinge.useSpring = false;

        // Determine push direction relative to door
        Vector3 toPlayer = (PlayerManager.instance.transform.position - transform.position).normalized;

        // Decide rotation direction (left/right)
        float direction = Vector3.Dot(transform.right, toPlayer) > 0 ? 1f : -1f;

        // Apply torque around hinge axis (usually Y)
        rb.AddTorque(Vector3.up * openTorque, ForceMode.Impulse);
    }

    private void CloseDoor()
    {
        if (!isOpen) return;

        isOpen = false;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = false; // keep physics active

        var spring = hinge.spring;
        spring.spring = 50f;        // strength
        spring.damper = 5f;         // smoothness
        spring.targetPosition = 0f; // closed angle

        hinge.spring = spring;
        hinge.useSpring = true;
    }

    private void SetClosedStateImmediate()
    {
        rb.isKinematic = true;
        transform.localRotation = initialLocalRotation;
    }

    public override void Hover()
    {
        if (isOpen)
        {
            PlayerManager.instance.PlayerUIManager.CanInteract(canInteract, "Close");
        }
        else
        {
            PlayerManager.instance.PlayerUIManager.CanInteract(canInteract, "Open");
        }
    }
}
