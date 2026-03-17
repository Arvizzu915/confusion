using UnityEngine;

public class ACHealthSystem : MonoBehaviour, IDamageable
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
        animator = enemyManager.animator;
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
