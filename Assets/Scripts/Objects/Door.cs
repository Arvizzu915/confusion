using System.Collections;
using UnityEngine;

public class Door : Interactuable
{
    private bool berserkDoor = false;

    private enum DoorState
    {
        Closed,
        Nudged,
        Opening,
        Opened,
        Closing
    }

    [Header("References")]
    [SerializeField] private HingeJoint hinge;
    [SerializeField] private DoorTrigger trigger;

    [Header("Angles")]
    [SerializeField] private float nudgeAngle = 15f;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float closedAngleTolerance = 2f;

    [Header("Spring")]
    [SerializeField] private float springStrength = 40f;
    [SerializeField] private float springDamper = 8f;

    [Header("Auto Close")]
    [SerializeField] private float autoCloseDelay = 2f;

    private DoorState state = DoorState.Closed;

    private Coroutine movementRoutine;
    private bool openPositive;

    public bool locked = false;

    private void Awake()
    {
        if (trigger != null)
        {
           trigger.Initialize(this);
        }
       
        JointSpring spring = hinge.spring;
        spring.spring = springStrength;
        spring.damper = springDamper;
        spring.targetPosition = 0;
        hinge.spring = spring;

        hinge.useSpring = true;
    }

    public override void Interact(PlayerManager player)
    {
        if (state == DoorState.Opening || state == DoorState.Closing)
            return;

        if (locked)
        {
            if (berserkDoor)
            {
                BerserkDetector.Instance.ActivateBerserker();
                berserkDoor = false;
            }
            //make locked sound
            return;
        }

        switch (state)
        {
            case DoorState.Closed:
                Open(false);
                break;

            case DoorState.Nudged:
                Open(true);
                break;

            case DoorState.Opened:
                Close();
                break;
        }
    }

    private void Open(bool fullOpen)
    {
        openPositive = transform.InverseTransformPoint(PlayerManager.instance.transform.position).z >= 0;

        float target = fullOpen ? openAngle : nudgeAngle;

        if (!openPositive)
            target = -target;

        if (movementRoutine != null)
            StopCoroutine(movementRoutine);

        movementRoutine = StartCoroutine(MoveDoor(
            fullOpen ? DoorState.Opening : DoorState.Nudged,
            target));
    }

    public void Close()
    {
        if (movementRoutine != null)
            StopCoroutine(movementRoutine);

        movementRoutine = StartCoroutine(MoveDoor(DoorState.Closing, 0));
    }

    private IEnumerator MoveDoor(DoorState movingState, float targetAngle)
    {
        state = movingState;

        JointSpring spring = hinge.spring;
        spring.targetPosition = targetAngle;
        hinge.spring = spring;
        hinge.useSpring = true;

        while (Mathf.Abs(hinge.angle - targetAngle) > 2f)
            yield return null;

        if (movingState == DoorState.Opening)
        {
            state = DoorState.Opened;

            if (trigger != null)
            {
                while (trigger.PlayerInside)
                    yield return null;
            }
            

            yield return new WaitForSeconds(autoCloseDelay);

            if (!trigger.PlayerInside && trigger != null)
                Close();
        }
        else if (movingState == DoorState.Closing)
        {
            while (Mathf.Abs(hinge.angle) > closedAngleTolerance)
                yield return null;

            state = DoorState.Closed;
        }
        else
        {
            state = DoorState.Nudged;
        }

        movementRoutine = null;
    }

    public override void Hover()
    {
        switch (state)
        {
            case DoorState.Closed:
                PlayerManager.instance.PlayerUIManager.CanInteract(canInteract, "Nudge");
                break;

            case DoorState.Nudged:
                PlayerManager.instance.PlayerUIManager.CanInteract(canInteract, "Open");
                break;

            case DoorState.Opened:
                PlayerManager.instance.PlayerUIManager.CanInteract(canInteract, "Close");
                break;
        }
    }
}