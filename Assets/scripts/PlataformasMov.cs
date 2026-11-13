using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlataformaMov : MonoBehaviour
{
    [Header("Recorrido")]
    [SerializeField] private Vector3 desplazamiento = new Vector3(6f, 0f, 0f);
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private float pausaEnExtremos = 0.5f;

    private Rigidbody cuerpo;
    private Collider volumen;
    private Vector3 puntoInicial;
    private Vector3 puntoFinal;
    private bool yendoAlFinal;
    private float temporizadorPausa;

    private readonly HashSet<Rigidbody> pasajeros = new HashSet<Rigidbody>();

    private void Awake()
    {
        cuerpo = GetComponent<Rigidbody>();
        cuerpo.isKinematic = true;
        volumen = GetComponent<Collider>();

        puntoInicial = transform.position;
        puntoFinal = puntoInicial + desplazamiento;
        yendoAlFinal = true;
    }

    private void FixedUpdate()
    {
        if (temporizadorPausa > 0f)
        {
            temporizadorPausa -= Time.fixedDeltaTime;
            pasajeros.Clear();
            return;
        }

        Vector3 destino = yendoAlFinal ? puntoFinal : puntoInicial;
        Vector3 siguiente = Vector3.MoveTowards(cuerpo.position, destino, velocidad * Time.fixedDeltaTime);
        Vector3 avance = siguiente - cuerpo.position;

        cuerpo.MovePosition(siguiente);

        foreach (Rigidbody pasajero in pasajeros)
        {
            if (pasajero != null) pasajero.position += avance;
        }
        pasajeros.Clear();

        if (siguiente == destino)
        {
            yendoAlFinal = !yendoAlFinal;
            temporizadorPausa = pausaEnExtremos;
        }
    }

    private void OnCollisionStay(Collision colision)
    {
        Rigidbody otro = colision.rigidbody;
        if (otro == null) return;

        if (colision.collider.GetComponentInParent<PlayerMovement>() == null) return;

        bool estaArriba = colision.collider.bounds.min.y >= volumen.bounds.max.y - 0.2f;
        if (estaArriba) pasajeros.Add(otro);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origen = Application.isPlaying ? puntoInicial : transform.position;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(origen, origen + desplazamiento);
        Gizmos.DrawWireCube(origen + desplazamiento, transform.localScale);
    }
}
