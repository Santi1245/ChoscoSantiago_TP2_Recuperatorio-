using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monedarotacion : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0f, 30f * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("+1 punto");
            Destroy(gameObject);
        }
    }
}
