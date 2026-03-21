using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private int currentHealth = 0, maxHealth = 100;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            //die
        }
    }

    public void Heal(int health)
    {
        currentHealth += health;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
