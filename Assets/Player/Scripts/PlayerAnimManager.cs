
using UnityEngine;

public class PlayerAnimManager : MonoBehaviour
{
    public Animator animator;

    public void Play(string clip)
    {
        animator.Play(clip);
    }

    public void SetTrigger(string trigger, bool boolean)
    {
        animator.SetBool(trigger, boolean);
    }
}
