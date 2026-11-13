using UnityEngine;

public class GasToxico : MonoBehaviour
{
    [SerializeField] private float danoPorTick = 5f;
    [SerializeField] private float intervaloTick = 1f;

    private HealthSystem jugador;
    private float temporizador;

    private void OnTriggerEnter(Collider other)
    {
        HealthSystem hs = other.GetComponentInParent<HealthSystem>();
        if (hs == null) return;

        jugador = hs;
        temporizador = 0f;
        Debug.Log("Entro a la zona de gas toxico");
    }

    private void OnTriggerExit(Collider other)
    {
        HealthSystem hs = other.GetComponentInParent<HealthSystem>();
        if (hs == null || hs != jugador) return;

        jugador = null;
        Debug.Log("Salio de la zona de gas toxico");
    }

    private void Update()
    {
        if (jugador == null) return;

        temporizador += Time.deltaTime;
        if (temporizador >= intervaloTick)
        {
            temporizador = 0f;
            jugador.TakeDamage(danoPorTick);
            Debug.Log($"Gas toxico: -{danoPorTick} HP");
        }
    }
}
