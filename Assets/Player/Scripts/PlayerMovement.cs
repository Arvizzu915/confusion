using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public PlayerManager PlayerManager;
    public InputManager inputManager;

    [SerializeField] public CharacterController controller;
    [SerializeField] private float currentSpeed, normalSpeed = 3;
    [SerializeField] private float gravity = -10f;

    private bool canMove = true, running = false;

    public bool isGrounded = true;
    [SerializeField] private float jumpHeight = 3;

    Vector3 playerVelocity;

    [Header("Ground Check Settings")]
    [SerializeField] private float sphereRadius = 0.3f;
    [SerializeField] private float sphereDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer, objectMask;
    [SerializeField] private Transform groundCheckOrigin;

    //Inputs
    public PlayerInput inputs;

    private InputAction move;
    private Vector3 moveDirection;


    private void Start()
    {
        currentSpeed = normalSpeed;

        move = inputManager.inputs.Playing.Move;

        inputManager.inputs.Playing.Jump.started += Jump;

        inputManager.inputs.Playing.Run.started += Run;
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
        if (running)
        {
            currentSpeed = normalSpeed * 2.5f;
        }
        else
        {
            currentSpeed = normalSpeed;
        }

        if (move.ReadValue<Vector2>().y <= .2)
        {
            running = false;
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

    private void OnDrawGizmosSelected()
    {
        if (groundCheckOrigin == null)
            groundCheckOrigin = transform;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheckOrigin.position + Vector3.down * sphereDistance, sphereRadius);
    }
}
