using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public PlayerInput inputs;

    private void Awake()
    {
        Instance = this;

        inputs = new PlayerInput();

        inputs.Playing.Enable();

    }

    public void SwitchToInspect()
    {
        inputs.Playing.Disable();
        inputs.Analyze.Disable();
        inputs.Inspecting.Enable();
    }

    public void SwitchToGameplay()
    {
        inputs.Inspecting.Disable();
        inputs.Analyze.Disable();
        inputs.Playing.Enable();
    }

    public void SwitchToAnalyze()
    {
        inputs.Playing.Disable();
        inputs.Inspecting.Disable();
        inputs.Analyze.Enable();
    }
}
