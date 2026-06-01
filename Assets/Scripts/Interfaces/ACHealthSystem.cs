using UnityEngine;

public class ACHealthSystem : MonoBehaviour
{
    public EnemyManager enemyManager;
    private Animator animator;

    [SerializeField] private int currentHealth = 100, maxHealth = 100;


    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
    }

    public void TakeDamage(int damage)
    {
        animator.Play("Damage");

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
