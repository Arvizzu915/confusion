using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] private Transform cameraHolder;

    private void OnEnable()
    {
        if (Camera.main == null)
            return;

        Camera.main.transform.SetPositionAndRotation(cameraHolder.position, cameraHolder.rotation);
        Camera.main.transform.SetParent(cameraHolder);
    }
}
