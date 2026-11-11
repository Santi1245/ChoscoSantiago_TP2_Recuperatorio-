using UnityEngine;

public class MovimientoAleatorio : MonoBehaviour
{
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private float tiempoCambio = 2f;

    private Vector3 direccion;

    void Start()
    {
        ElegirDireccion();
        InvokeRepeating("ElegirDireccion", tiempoCambio, tiempoCambio);
    }

    void Update()
    {
        transform.position += direccion * velocidad * Time.deltaTime;
    }

    private void ElegirDireccion()
    {
        float angulo = Random.Range(0f, 360f);
        direccion = new Vector3(Mathf.Sin(angulo), 0, Mathf.Cos(angulo)).normalized;
    }
}
