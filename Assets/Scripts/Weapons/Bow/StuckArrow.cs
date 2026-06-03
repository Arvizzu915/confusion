using UnityEngine;

public class StuckArrow : PickableObject
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private FixedJoint joint;

    public void ResetArrow()
    {
        joint.connectedBody = null;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = false;
    }

    public void StickTo(Rigidbody targetRb)
    {
        if (targetRb != null)
        {
            rb.isKinematic = false;
            joint.connectedBody = targetRb;
        }
        else
        {
            rb.isKinematic = true;
            joint.connectedBody = null;
        }
    }

    public void ReturnToPool()
    {
        ArrowPool.Instance.ReturnStuckArrow(this);
    }

    public override void Interact(PlayerManager player)
    {
        ObjectsInventory.instance.AddItemAutomatic(this);
    }
}