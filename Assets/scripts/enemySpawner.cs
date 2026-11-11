using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float tiempoInicial = 2f;
    [SerializeField] private float intervalo = 3f;
    [SerializeField] private float tiempoDeVida = 5f; 

    void Start()
    {
        InvokeRepeating("CrearEnemigo", tiempoInicial, intervalo);
    }

    private void CrearEnemigo()
    {
        GameObject enemigo = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        
        Destroy(enemigo, tiempoDeVida);
    }
}
