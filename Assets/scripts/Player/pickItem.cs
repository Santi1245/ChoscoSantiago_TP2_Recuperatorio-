using UnityEngine;
using System;

public class PickItem : MonoBehaviour
{
    [SerializeField] private Transform player;   
    [SerializeField] private Vector3 holdOffset = new Vector3(0.4f, 0.3f, 0f);

    private GameObject currentItem = null;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item") && currentItem == null)
        {
            if (Input.GetKeyDown(KeyCode.E)) Pick(other.gameObject);
        }
    }

    private void Pick(GameObject item)
    {
        currentItem = item;
        item.transform.SetParent(player);
        item.transform.localPosition = holdOffset;
        item.transform.localRotation = Quaternion.identity;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        Collider collider = item.GetComponent<Collider>();
        if (rb != null) rb.isKinematic = true;
        if (collider != null) collider.enabled = false;
    }

    public GameObject DropItem()
    {
        GameObject temp = currentItem;
        currentItem = null;
        return temp;
    }
}
