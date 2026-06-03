using UnityEngine;

public class StuckArrow : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider coll;
    [SerializeField] private FixedJoint joint;

    public void ResetArrow()
    {
        joint.connectedBody = null;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = false;

        if (coll != null)
            coll.enabled = false;
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
}