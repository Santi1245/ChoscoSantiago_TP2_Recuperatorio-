using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float interactDistance = 6f;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Puerta")]
    [Tooltip("Distancia del centro del cubo al borde donde va la bisagra.")]
    [SerializeField] private float hingeOffsetX = 2.375f;
    [SerializeField] private float openAngle = 90f;

    private bool isOpen;
    private Vector3 closedPosition;
    private Quaternion closedRotation;
    private bool hasClosedState;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, interactDistance, interactableLayers))
        {
            Debug.Log($"[Interaction] Golpeo: {hit.collider.name} | Tag: {hit.collider.tag} | Distancia: {hit.distance:F2}");

            if (hit.collider.CompareTag("Door"))
            {
                InteractDoor(hit.collider.transform);
            }
            else
            {
                Debug.Log($"[Interaction] El objeto no tiene tag Door (tiene '{hit.collider.tag}').");
            }
        }
        else
        {
            Debug.Log("[Interaction] El rayo no golpeo nada.");
        }
    }

    void InteractDoor(Transform door)
    {
        // Guardamos el estado cerrado una sola vez
        if (!hasClosedState)
        {
            closedPosition = door.position;
            closedRotation = door.rotation;
            hasClosedState = true;
        }

        // Restauramos SIEMPRE el estado cerrado antes de girar
        door.position = closedPosition;
        door.rotation = closedRotation;

        isOpen = !isOpen;

        if (isOpen)
        {
            // La bisagra se calcula desde el estado cerrado, no desde el actual
            Vector3 hingeWorld = door.TransformPoint(new Vector3(-hingeOffsetX, 0f, 0f));
            door.RotateAround(hingeWorld, Vector3.up, -openAngle);
        }

        Debug.Log($"[Door] {(isOpen ? "Abierta" : "Cerrada")}");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = transform.forward;

        Gizmos.DrawRay(origin, direction * interactDistance);
    }
}
