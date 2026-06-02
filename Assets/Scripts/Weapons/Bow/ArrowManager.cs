    using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public ArrowSO arrowSO;

    public Rigidbody rb;
    public Collider coll;
    public FixedJoint joint;

    private bool stuck;

    private void OnEnable()
    {
        stuck = false;

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.detectCollisions = true;

        coll.enabled = true;
        coll.isTrigger = false;

        DisableJoint();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

        arrowSO.HitObject(collision, this);
    }

    public void StickWithJoint(Collision collision)
    {
        if (stuck) return;

        stuck = true;

        Rigidbody hitRb = collision.collider.attachedRigidbody;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = false;
        rb.useGravity = false;

        coll.enabled = false;

        if (hitRb != null)
        {
            joint.connectedBody = hitRb;
            joint.enableCollision = false;
            joint.breakForce = Mathf.Infinity;
            joint.breakTorque = Mathf.Infinity;
        }
        else
        {
            rb.isKinematic = true;
        }
    }

    private void DisableJoint()
    {
        joint.connectedBody = null;
        joint.breakForce = 0f;
        joint.breakTorque = 0f;
    }
}
