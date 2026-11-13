using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Configuracion Salud")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    [SerializeField] private bool isDead;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;
        Debug.Log($"Salud actual: {currentHealth}");
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        if (amount <= 0f) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"Salud actual: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Dead();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        if (amount <= 0f) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"Salud actual: {currentHealth}");
    }

    private void Dead()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} eliminado");

        if (GameManager.Instance != null)
        {
            if (CompareTag("Player"))
            {
                GameManager.Instance.NotifyPlayerDead();
            }
            else
            {
                GameManager.Instance.NotifyEnemyDefeat();
            }
        }

        if (!CompareTag("Player"))
        {
            Transform target = transform.parent != null ? transform.parent : transform;
            Destroy(target.gameObject);
        }
    }
}
