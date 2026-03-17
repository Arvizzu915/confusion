using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerUIManager PlayerUIManager;
    public PlayerCombat playerCombat;
    public PlayerAnimManager playerAnim;

    public GameObject canvas;
    public LevelCanvas levelCanvas;

    private void Start()
    {
        GameObject hud = Instantiate(canvas);
        levelCanvas = hud.GetComponent<LevelCanvas>();

        PlayerUIManager.SetManager(this);
    }
}
