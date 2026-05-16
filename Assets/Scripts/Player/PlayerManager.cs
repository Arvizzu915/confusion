using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public PlayerMovement playerMovement;
    public PlayerUIManager PlayerUIManager;
    public PlayerCombat playerCombat;
    public InputManager inputManager;
    public PlayerAnimManager playerAnim;
    public CameraLook cameraLook;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inputManager.inputs.Playing.ReloadScene.performed += ReloadScene;   
    }

    private void OnDisable()
    {
        inputManager.inputs.Playing.ReloadScene.performed -= ReloadScene;
    }

    public void ReloadScene(InputAction.CallbackContext ctx)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
