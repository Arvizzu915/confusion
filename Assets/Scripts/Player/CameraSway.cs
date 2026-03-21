using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CameraSway : MonoBehaviour
{
    [SerializeField] float maxAmplitude = 0.05f;
    [SerializeField] float maxFrequency = 6f;

    [SerializeField] float swayMultiplier = 0.01f, frequencyMultiplayer = 2f;

    float timer;
    Vector3 initialLocalPos;

    void Start()
    {
        initialLocalPos = transform.localPosition;
    }

    void Update()
    {
        

        float speed = PlayerManager.instance.playerMovement.controller.velocity.magnitude;

        float amplitude = speed * swayMultiplier;
        float frequency = speed * frequencyMultiplayer;

        amplitude = Mathf.Clamp(amplitude, 0, maxAmplitude);
        frequency = Mathf.Clamp(frequency, 0, maxFrequency);


        timer += Time.deltaTime * frequency;

        float x = Mathf.Sin(timer) * amplitude;
        float y = Mathf.Cos(timer * 2f) * amplitude/2f;

        Vector3 offset = new(x, y, 0);

        transform.localPosition = initialLocalPos + offset;
    }
}
