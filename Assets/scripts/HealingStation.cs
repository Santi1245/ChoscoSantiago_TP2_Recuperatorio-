using System.Collections;
using UnityEngine;

public class HealingStation : MonoBehaviour
{
    [Header("Configuracion de curacion")]
    [SerializeField] private float healPerTick = 10f;
    [SerializeField] private float tickInterval = 1.5f;
    [SerializeField] private float maxHealAmount = 50f;

    [Header("Cooldown")]
    [SerializeField] private float cooldownTime = 5f;

    [Header("Referencias visuales")]
    [SerializeField] private Light stationLight;
    [SerializeField] private ParticleSystem stationParticles;
    [SerializeField] private Renderer stationRenderer;

    [Header("Colores de estado")]
    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private Color cooldownColor = Color.red;

    private bool isPlayerInside = false;
    private bool isOnCooldown = false;
    private float healedSoFar = 0f;
    private HealthSystem playerHealth;
    private Coroutine healRoutine;

    private void Start()
    {
        SetVisualState(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerHealth = other.GetComponent<HealthSystem>();
        if (playerHealth == null) return;

        isPlayerInside = true;

        if (isOnCooldown)
        {
            Debug.Log("Estacion en Recarga");
            return;
        }

        if (healRoutine == null)
            healRoutine = StartCoroutine(HealRoutine());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerInside = false;

        if (healRoutine != null)
        {
            StopCoroutine(healRoutine);
            healRoutine = null;
        }
    }

    private IEnumerator HealRoutine()
    {
        Debug.Log("Estacion activa: curando...");

        while (isPlayerInside && healedSoFar < maxHealAmount)
        {
            if (playerHealth == null || playerHealth.IsDead)
                break;

            if (playerHealth.CurrentHealth >= playerHealth.MaxHealth)
            {
                Debug.Log("Vida al maximo, la estacion no consume carga");
                yield return new WaitForSeconds(tickInterval);
                continue;
            }

            playerHealth.Heal(healPerTick);
            healedSoFar += healPerTick;
            Debug.Log($"Curado +{healPerTick} HP | Carga usada: {healedSoFar}/{maxHealAmount}");

            yield return new WaitForSeconds(tickInterval);
        }

        healRoutine = null;

        if (healedSoFar >= maxHealAmount)
            StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        SetVisualState(false);
        Debug.Log("Estacion en Recarga");

        yield return new WaitForSeconds(cooldownTime);

        healedSoFar = 0f;
        isOnCooldown = false;
        SetVisualState(true);
        Debug.Log("Estacion lista de nuevo");

        if (isPlayerInside && healRoutine == null)
            healRoutine = StartCoroutine(HealRoutine());
    }

    private void SetVisualState(bool active)
    {
        if (stationLight != null)
            stationLight.enabled = active;

        if (stationParticles != null)
        {
            if (active) stationParticles.Play();
            else stationParticles.Stop();
        }

        if (stationRenderer != null)
            stationRenderer.material.color = active ? activeColor : cooldownColor;
    }
}
