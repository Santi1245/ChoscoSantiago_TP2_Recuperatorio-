using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monedarotacion : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0f, 30f * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(1);
            }
            Destroy(gameObject);
        }
    }
}
