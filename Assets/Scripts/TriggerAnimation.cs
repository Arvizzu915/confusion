using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    private void PlayAnimation()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayAnimation();
        }
    }
}
