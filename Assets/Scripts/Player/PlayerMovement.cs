using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public PlayerManager PlayerManager;
    public InputManager inputManager;

    public CharacterController controller;

    [Header("Speed")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private float targetSpeed;
    [SerializeField] private float normalSpeed = 3f;
    [SerializeField] private float runningSpeed = 7f;
    [SerializeField] private float crouchingSpeed = 1.5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 14f;

    [SerializeField] private float gravity = -10f;

    private bool canMove = true;
    private bool running = false;

    public bool isGrounded = true;
    public Vector3 playerVelocity;

    [Header("Ground Check Settings")]
    [SerializeField] private float sphereRadius = 0.3f;
    [SerializeField] private float sphereDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer, objectMask;
    [SerializeField] private Transform groundCheckOrigin;

    [Header("Crouching Settings")]
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float crouchingHeight = 1f;

    [SerializeField] private Vector3 standingCenter = new(0, 1f, 0);
    [SerializeField] private Vector3 crouchingCenter = new(0, 0.5f, 0);

    private float targetHeight = 2f;
    private Vector3 targetCenter = new(0, 1f, 0);

    private bool isCrouching = false;

    public PlayerInput inputs;

    private InputAction move;

    public Vector3 moveDirection;
    private Vector3 movementDirection;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        targetHeight = standingHeight;
        targetCenter = standingCenter;

        currentSpeed = 0f;
        targetSpeed = 0f;

        move = inputManager.inputs.Playing.Move;

        inputManager.inputs.Playing.Jump.started += Jump;
        inputManager.inputs.Playing.Run.started += Run;
        inputManager.inputs.Playing.Crouch.started += ctx => SetCrouch(true);
        inputManager.inputs.Playing.Crouch.canceled += ctx => SetCrouch(false);
    }

    private void OnDisable()
    {
        inputManager.inputs.Playing.Jump.started -= Jump;
        inputManager.inputs.Playing.Run.started -= Run;
    }

    private void Update()
    {
        UpdateControllerHeight();
        ReadMovementInput();
        UpdateMovementState();
        UpdateSpeed();
        ApplyGravity();
        MovePlayer();
    }

    private void UpdateControllerHeight()
    {
        controller.height = Mathf.Lerp(
            controller.height,
            targetHeight,
            Time.deltaTime * 10f
        );

        controller.center = Vector3.Lerp(
            controller.center,
            targetCenter,
            Time.deltaTime * 10f
        );
    }

    private void ReadMovementInput()
    {
        Vector2 input = canMove ? move.ReadValue<Vector2>() : Vector2.zero;

        moveDirection = new Vector3(input.x, 0f, input.y);

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            movementDirection = transform.TransformDirection(moveDirection);
        }
    }

    private void UpdateMovementState()
    {
        bool hasInput = moveDirection.sqrMagnitude > 0.01f;

        if (!hasInput)
        {
            running = false;
            targetSpeed = 0f;
            PlayerManager.playerAnim.SetTrigger("Running", false);
            return;
        }

        if (moveDirection.z <= 0.2f && !isCrouching)
        {
            running = false;
            PlayerManager.playerAnim.SetTrigger("Running", false);
        }

        if (isCrouching)
        {
            targetSpeed = crouchingSpeed;
        }
        else if (running)
        {
            targetSpeed = runningSpeed;
        }
        else
        {
            targetSpeed = normalSpeed;
        }
    }

    private void UpdateSpeed()
    {
        float speedChangeRate = targetSpeed > currentSpeed
            ? acceleration
            : deceleration;

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            speedChangeRate * Time.deltaTime
        );

        if (currentSpeed < 0.01f)
        {
            currentSpeed = 0f;
        }
    }

    private void ApplyGravity()
    {
        CheckGround();

        if (playerVelocity.y > -50f)
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }
        else
        {
            playerVelocity.y = -50f;
        }

        if (isGrounded && playerVelocity.y < 0f)
        {
            playerVelocity.y = -2f;
        }
    }

    private void MovePlayer()
    {
        Vector3 horizontalMovement = movementDirection * currentSpeed;
        Vector3 finalMovement = horizontalMovement + playerVelocity;

        controller.Move(finalMovement * Time.deltaTime);
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        /*
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -gravity);
        }
        */
    }

    private void Run(InputAction.CallbackContext ctx)
    {
        if (!isGrounded || isCrouching || PlayerCombat.Instance.aiming) return;

        running = true;
        PlayerManager.playerAnim.SetTrigger("Running", true);
    }

    private void CheckGround()
    {
        if (groundCheckOrigin == null)
            groundCheckOrigin = transform;

        isGrounded = Physics.SphereCast(
            groundCheckOrigin.position,
            sphereRadius,
            Vector3.down,
            out _,
            sphereDistance,
            groundLayer + objectMask
        );
    }

    private void SetCrouch(bool crouch)
    {
        if (crouch)
        {
            targetHeight = crouchingHeight;
            targetCenter = crouchingCenter;

            isCrouching = true;
            running = false;
        }
        else
        {
            if (!CanStand()) return;

            targetHeight = standingHeight;
            targetCenter = standingCenter;

            isCrouching = false;
        }

        PlayerManager.instance.cameraLook.ChangePosition(isCrouching);
    }

    public void StopRunning()
    {
        running = false;
    }

    private bool CanStand()
    {
        float checkDistance = standingHeight - crouchingHeight;

        return !Physics.SphereCast(
            transform.position,
            controller.radius,
            Vector3.up,
            out _,
            checkDistance,
            groundLayer
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckOrigin == null)
            groundCheckOrigin = transform;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(
            groundCheckOrigin.position + Vector3.down * sphereDistance,
            sphereRadius
        );
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic) return;
        if (hit.moveDirection.y < -0.3f) return;

        Vector3 pushDir = new(hit.moveDirection.x, 0f, hit.moveDirection.z);

        float pushForce = .5f;

        rb.AddForce(pushDir * pushForce, ForceMode.Impulse);
    }
}