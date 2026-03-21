using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public PlayerManager PlayerManager;
    public InputManager inputManager;

    [SerializeField] public CharacterController controller;
    [SerializeField] private float currentSpeed, normalSpeed = 3, runningSpeed = 7, crouchingSpeed = 1.5f;
    [SerializeField] private float gravity = -10f;

    private bool canMove = true, running = false;

    public bool isGrounded = true;
    [SerializeField] private float jumpHeight = 3;

    public Vector3 playerVelocity;

    [Header("Ground Check Settings")]
    [SerializeField] private float sphereRadius = 0.3f;
    [SerializeField] private float sphereDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer, objectMask;
    [SerializeField] private Transform groundCheckOrigin;

    [Header("Crouching Settings")]
    [SerializeField] float standingHeight = 2f;
    [SerializeField] float crouchingHeight = 1f;


    [SerializeField] Vector3 standingCenter = new(0, 1f, 0);
    [SerializeField] Vector3 crouchingCenter = new(0, 0.5f, 0);

    [SerializeField] float crouchSpeed = 1.5f;

    float targetHeight = 2;
    Vector3 targetCenter = new(0, 1f, 0);

    bool isCrouching = false;

    //Inputs
    public PlayerInput inputs;

    private InputAction move;
    public Vector3 moveDirection;


    private void Start()
    {
        targetHeight = standingHeight;
        targetCenter = standingCenter;

        currentSpeed = normalSpeed;

        move = inputManager.inputs.Playing.Move;

        inputManager.inputs.Playing.Jump.started += Jump;

        inputManager.inputs.Playing.Run.started += Run;

        inputManager.inputs.Playing.Crouch.started += ctx => SetCrouch(true);
        inputManager.inputs.Playing.Crouch.canceled += ctx => SetCrouch(false);
    }


    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        inputManager.inputs.Playing.Jump.started -= Jump;

        inputManager.inputs.Playing.Run.started -= Run;
    }


    private void Update()
    {
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * 10f);
        controller.center = Vector3.Lerp(controller.center, targetCenter, Time.deltaTime * 10f);


        if (running)
        {
            currentSpeed = runningSpeed;
        }

        if (move.ReadValue<Vector2>().y <= .2)
        {
            running = false;
            currentSpeed = normalSpeed;

            PlayerManager.playerAnim.SetTrigger("Running", false);
        }

        if (canMove)
        {
            moveDirection.x = move.ReadValue<Vector2>().x;
            moveDirection.z = move.ReadValue<Vector2>().y;
        }

        CheckGround();

        if (playerVelocity.y > -50)
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }
        else
        {
            playerVelocity.y = -50;
        }

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2;
        }

        controller.Move((currentSpeed * transform.TransformDirection(moveDirection) + playerVelocity) * Time.deltaTime);

    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -gravity);
        }
    }

    private void Run(InputAction.CallbackContext ctx)
    {
        if (!isGrounded || isCrouching) return;

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
            currentSpeed = crouchingSpeed;
            targetHeight = crouchingHeight;
            targetCenter = crouchingCenter;
            currentSpeed = crouchSpeed;
            isCrouching = true;
            running = false;
        }
        else
        {
            if (!CanStand()) return;

            currentSpeed = normalSpeed;
            targetHeight = standingHeight;
            targetCenter = standingCenter;
            currentSpeed = normalSpeed;
            isCrouching = false;
        }

        PlayerManager.instance.cameraLook.ChangePosition(isCrouching);
    }

    bool CanStand()
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
        Gizmos.DrawWireSphere(groundCheckOrigin.position + Vector3.down * sphereDistance, sphereRadius);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic) return;

        // Optional: don't push downward
        if (hit.moveDirection.y < -0.3f) return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        float pushForce = .5f;

        rb.AddForce(pushDir * pushForce, ForceMode.Impulse);
    }
}
