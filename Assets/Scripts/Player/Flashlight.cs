using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSmoothTime = 0.12f;

    private Vector3 angularVelocity;

    private void LateUpdate()
    {
        // Position follows instantly
        transform.position = target.position;

        // Rotation follows with delay
        Vector3 currentEuler = transform.rotation.eulerAngles;
        Vector3 targetEuler = target.rotation.eulerAngles;

        Vector3 smoothEuler = new(
            Mathf.SmoothDampAngle(currentEuler.x, targetEuler.x, ref angularVelocity.x, rotationSmoothTime),
            Mathf.SmoothDampAngle(currentEuler.y, targetEuler.y, ref angularVelocity.y, rotationSmoothTime),
            Mathf.SmoothDampAngle(currentEuler.z, targetEuler.z, ref angularVelocity.z, rotationSmoothTime)
        );

        transform.rotation = Quaternion.Euler(smoothEuler);
    }
}
