using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public ArrowSO arrowSO;

    public Rigidbody rb;
    public Collider coll;

    private bool hasHit;

    public void ResetArrow()
    {
        hasHit = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.detectCollisions = true;

        coll.enabled = true;
        coll.isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        hasHit = true;

        arrowSO.HitObject(collision, this);
    }

    public void ReturnToPool()
    {
        ArrowPool.Instance.ReturnFlyingArrow(this);
    }
}