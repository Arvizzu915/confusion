using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public PlayerManager playerManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(5);
        }

        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce((other.transform.position - playerManager.transform.position).normalized * 7, ForceMode.Impulse);
        }
    }
}
