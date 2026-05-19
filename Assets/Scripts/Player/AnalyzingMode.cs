using UnityEngine;
using UnityEngine.InputSystem;

public class AnalyzingMode : MonoBehaviour
{
    public static AnalyzingMode Instance;

    public IInspectionable currentObj;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputManager.Instance.inputs.Analyze.Return.performed += ExitAnalyze;
    }

    private void OnDisable()
    {
        InputManager.Instance.inputs.Analyze.Return.performed -= ExitAnalyze;
    }

    public void EnterAnalyzeMode()
    {
        InputManager.Instance.SwitchToAnalyze();
        PlayerManager.instance.PlayerUIManager.CanInteract(false, "");
        PlayerDetectInteract.instance.analyzing = true;
        PlayerDetectInteract.instance.lantern.GetComponent<Light>().intensity = 3;
        LevelCanvas.instance.ChangeToInspectHUD();
        PlayerDetectInteract.instance.bow.SetActive(false);
        Time.timeScale = 0.0f;
    }

    private void ExitAnalyze(InputAction.CallbackContext context)
    {
        PlayerDetectInteract.instance.checkingObject = false;
        InputManager.Instance.SwitchToGameplay();
        PlayerDetectInteract.instance.lantern.GetComponent<Light>().intensity = 52;
        PlayerDetectInteract.instance.analyzing = false;
        LevelCanvas.instance.ChangeToPlayingHUD();
        PlayerDetectInteract.instance.bow.SetActive(true);
        Time.timeScale = 1.0f;

        currentObj.StopInspecting();
        currentObj = null;
    }
}
