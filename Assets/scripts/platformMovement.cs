using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;
    [SerializeField] private float velocidad = 2f;

    private Transform objetivoActual;

    void Start()
    {
        objetivoActual = puntoA;
        Invoke("CambiarDireccion", 2f);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            objetivoActual.position,
            velocidad * Time.deltaTime);
    }

    private void CambiarDireccion()
    {
        if (objetivoActual == puntoA)
            objetivoActual = puntoB;
        else
            objetivoActual = puntoA;

        Invoke("CambiarDireccion", 2f);
    }
}
