using System.Collections;
using UnityEngine;

public class SpeerPowerUp: MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Collider powerUpCollider;

private void Start()
{
    meshRenderer = GetComponent<MeshRenderer>();
    powerUpCollider = GetComponent<Collider>();
}

private void OnTriggerEnter (Collider other)
{
    //Debug.Log("Algo atraves0 el power up: "+other.gameObject.name);
    if (other.CompareTag("Player"))
    {
        PlayerMovement movimiento = other.GetComponent<PlayerMovement>();

        if (movimiento!=null)
        {
            movimiento.ActivarTurbo();
            StartCoroutine(Reaparecer());
        }
    }
}

    private IEnumerator Reaparecer()
    {
       meshRenderer.enabled = false;
       powerUpCollider.enabled = false;

       yield return new WaitForSeconds (10f);

       meshRenderer.enabled = true;
       powerUpCollider.enabled = true;
    }


}