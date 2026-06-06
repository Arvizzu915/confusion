using UnityEngine;

public class FadeAnim : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;

    public void FadeInOut(bool inout)
    {

        if (inout)
        {
            LeanTween.alphaCanvas(panel, 1, 0.1f);
        }
        else
        {
            LeanTween.alphaCanvas(panel, 0, 0.1f);
        }
    }
}
