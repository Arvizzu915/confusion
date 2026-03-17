using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public PlayerUIManager PlayerUIManager;
    public PlayerCombat playerCombat;
    public PlayerAnimManager playerAnim;
    public CameraLook cameraLook;

    public GameObject canvas;
    public LevelCanvas levelCanvas;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameObject hud = Instantiate(canvas);
        levelCanvas = hud.GetComponent<LevelCanvas>();

        PlayerUIManager.SetManager(this);
    }
}
