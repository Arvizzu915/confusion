using UnityEngine;

public abstract class ArrowSO : ScriptableObject
{
    public int damage = 1;

    public virtual void HitObject(Collision collision, ArrowManager manager)
    {
        manager.coll.isTrigger = true;
        manager.rb.isKinematic = true;
    }
}
