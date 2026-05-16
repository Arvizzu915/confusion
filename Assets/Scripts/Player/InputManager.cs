using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerInput inputs;

    private void Awake()
    {
        inputs = new PlayerInput();

        inputs.Playing.Enable();

    }

    public void SwitchToInspect()
    {
        inputs.Playing.Disable();
        inputs.Inspecting.Enable();
    }

    public void SwitchToGameplay()
    {
        inputs.Inspecting.Disable();
        inputs.Playing.Enable();
    }
}
