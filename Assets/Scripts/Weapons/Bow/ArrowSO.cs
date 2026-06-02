using UnityEngine;

[CreateAssetMenu(fileName = "NormalArrow", menuName = "Arrows/NormalArrow")]
public class ArrowSO : ScriptableObject
{
    public int damage = 1;


    public virtual void HitObject(Collision collision, ArrowManager manager)
    {
        manager.StickWithJoint(collision);

        if (collision.gameObject.TryGetComponent(out IShootable shootable))
        {
            shootable.GetShot(damage);
        }
    }
}
