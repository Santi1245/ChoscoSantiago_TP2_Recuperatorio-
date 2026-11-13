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
            if (hit.collider.CompareTag("Door"))
            {
                InteractDoor(hit.collider.transform);
            }
        }
    }

    void InteractDoor(Transform door)
    {
        if (!hasClosedState)
        {
            closedPosition = door.position;
            closedRotation = door.rotation;
            hasClosedState = true;
        }

        door.position = closedPosition;
        door.rotation = closedRotation;

        isOpen = !isOpen;

        if (isOpen)
        {
            Vector3 hingeWorld = door.TransformPoint(new Vector3(-hingeOffsetX, 0f, 0f));
            door.RotateAround(hingeWorld, Vector3.up, -openAngle);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = transform.forward;

        Gizmos.DrawRay(origin, direction * interactDistance);
    }
}
