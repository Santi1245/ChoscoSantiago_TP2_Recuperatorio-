using UnityEngine;

public class GoalZone : MonoBehaviour
{
    [SerializeField] private Transform goal;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickItem pickItem = other.GetComponent<PickItem>();

            if (pickItem != null)
            {
                GameObject item = pickItem.DropItem();

                if (item != null)
                {
                    ItemInZone(item);

                    Debug.Log("completaste el nivel");
                }
                else
                {
                    Debug.Log("Derrota o Llego a la meta pero sin el item");
                }
            }
        }
    }

    private void ItemInZone(GameObject item)
    {
        item.transform.SetParent(goal);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Collider col = item.GetComponent<Collider>();
        Rigidbody rb = item.GetComponent<Rigidbody>();

        if (col!= null) col.enabled = false;
        if (rb!= null) rb.isKinematic = false;





    }
}