using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Configuracion Salud")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public float CurrentHealth => currentHealth;

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
        if (amount <= 0f) return; // NUEVO: evita daño negativo

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"Salud actual: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Dead();
        }
    }

    private void Dead()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} eliminado");

        // NUEVO: avisar al GameManager según quién murió
        if (GameManager.Instance == null) return;

        if (CompareTag("Player"))
        {
            GameManager.Instance.NotifyPlayerDead();
        }
        else
        {
            GameManager.Instance.NotifyEnemyDefeat();
        }
    }
}
