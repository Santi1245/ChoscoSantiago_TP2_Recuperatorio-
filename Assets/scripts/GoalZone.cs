using UnityEngine;

public class GoalZone : MonoBehaviour
{
    [SerializeField] private Transform goal;

    private bool yaCompletado = false;   // evita que se dispare dos veces

    private void OnTriggerEnter(Collider other)
    {
        if (yaCompletado) return;
        if (!other.CompareTag("Player")) return;

        PickItem pickItem = other.GetComponent<PickItem>();

        if (pickItem == null) return;

        GameObject item = pickItem.DropItem();

        if (item != null)
        {
            yaCompletado = true;
            ItemInZone(item);
            Debug.Log("completaste el nivel");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.NotifyGoalZone();
            }
        }
        else
        {
            Debug.Log("Derrota o Llego a la meta pero sin el item");
        }
    }

    private void ItemInZone(GameObject item)
    {
        item.transform.SetParent(goal);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Collider col = item.GetComponent<Collider>();
        Rigidbody rb = item.GetComponent<Rigidbody>();

        if (col != null) col.enabled = false;
        if (rb != null) rb.isKinematic = true;   // ojo: estaba en false
    }
}
