using UnityEngine;

public class WeaponCamera : MonoBehaviour
{
    [SerializeField] private Camera sourceCamera;

    [SerializeField] private Camera weaponCamera;

    private void LateUpdate()
    {
        weaponCamera.fieldOfView = sourceCamera.fieldOfView;
        weaponCamera.nearClipPlane = sourceCamera.nearClipPlane;
        weaponCamera.farClipPlane = sourceCamera.farClipPlane;
    }
}
