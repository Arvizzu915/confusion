using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public ArrowSO arrowSO;

    public Rigidbody rb;
    public Collider coll;

    private void OnEnable()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        arrowSO.HitObject(collision, this);
    }
}
