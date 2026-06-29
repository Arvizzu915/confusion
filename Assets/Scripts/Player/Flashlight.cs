using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Flashlight : MonoBehaviour
{
    public static Flashlight instance;

    [SerializeField] private Light lightComponent;

    [SerializeField] private Transform target;
    [SerializeField] private float rotationSmoothTime = 0.12f;

    private Vector3 angularVelocity;

    private void Awake()
    {
        instance = this;
    }

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

    public void ChangeToInspectingLight(bool turnOnOff)
    {
        if (turnOnOff)
        {
            lightComponent.intensity = 47;
        }
        else
        {
            lightComponent.intensity = 3;
        }
        
    }


}
