using UnityEngine;

[CreateAssetMenu(fileName = "NormalArrow", menuName = "Arrows/NormalArrow")]
public class ArrowSO : ScriptableObject
{
    public int damage = 1;


    public virtual void HitObject(Collision collision, ArrowManager manager)
    {
        manager.transform.SetParent(collision.transform, true);

        manager.coll.isTrigger = true;
        manager.rb.isKinematic = true;

        if (collision.gameObject.TryGetComponent(out IShootable shootable))
        {
            shootable.GetShot(damage);
        }
    }
}
