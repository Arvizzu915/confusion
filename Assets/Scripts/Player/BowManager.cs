using UnityEngine;

public class BowManager : MonoBehaviour
{
    public GameObject currentArrow;

    public Transform arrowSpwnPos;

    public Animator bowAnimator;

    [SerializeField] PlayerCombat combat;

    public void RechargeBow()
    {
        combat.canShoot = true;
    }

    public void PlayAnimClip(string Trigger)
    {
        bowAnimator.Play(Trigger);
    }
}
