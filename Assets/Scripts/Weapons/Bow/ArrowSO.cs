using UnityEngine;

[CreateAssetMenu(fileName = "NormalArrow", menuName = "Arrows/NormalArrow")]
public class ArrowSO : ScriptableObject
{
    public int damage = 1;

    public virtual void HitObject(Collision collision, ArrowManager flyingArrow)
    {
        ContactPoint contact = collision.GetContact(0);

        Rigidbody hitRb = collision.collider.attachedRigidbody;

        StuckArrow stuckArrow = ArrowPool.Instance.GetStuckArrow(
            contact.point,
            flyingArrow.transform.rotation
        );

        stuckArrow.StickTo(hitRb);

        if (collision.gameObject.TryGetComponent(out IShootable shootable))
        {
            shootable.GetShot(damage);
        }
        else if (hitRb != null && hitRb.TryGetComponent(out IShootable rbShootable))
        {
            rbShootable.GetShot(damage);
        }

        flyingArrow.ReturnToPool();
    }
}