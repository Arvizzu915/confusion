using UnityEngine;

[CreateAssetMenu(fileName = "NormalArrow", menuName = "Arrows/NormalArrow")]
public class ArrowSO : ScriptableObject
{
    public int damage = 1;
    ProjectileType projectileType = ProjectileType.Pierce;

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
            shootable.GetShot(damage, projectileType);
        }
        else if (hitRb != null && hitRb.TryGetComponent(out IShootable rbShootable))
        {
            rbShootable.GetShot(damage, projectileType);
        }

        flyingArrow.ReturnToPool();
    }
}