using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public PlayerMovement playerMovement;
    public PlayerUIManager PlayerUIManager;
    public PlayerCombat playerCombat;
    public PlayerAnimManager playerAnim;
    public CameraLook cameraLook;

    private void Awake()
    {
        instance = this;
    }

}
