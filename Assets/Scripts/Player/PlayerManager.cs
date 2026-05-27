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

    public Transform pocketPosition;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inputManager.inputs.Playing.ReloadScene.performed += ReloadScene;
        InputManager.Instance.inputs.Playing.Inventory.performed += OpenInventory;
    }

    private void OnDisable()
    {
        inputManager.inputs.Playing.ReloadScene.performed -= ReloadScene;
        InputManager.Instance.inputs.Playing.Inventory.performed -= OpenInventory;
    }

    public void ReloadScene(InputAction.CallbackContext ctx)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenInventory(InputAction.CallbackContext ctx)
    {
        ObjectsInventory.instance.OpenInventory();
    }
}
