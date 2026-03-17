using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerInput inputs;

    private void Awake()
    {
        inputs = new PlayerInput();

        inputs.Playing.Enable();
    }
}
