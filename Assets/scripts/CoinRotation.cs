using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    private float velocidadRotacion;

    void Start()
    {
        velocidadRotacion = 90f;
    }

    // Update is called once per frame
    void Update()
    {
    float giro = Input.GetAxisRaw("Horizontal");
    
    transform.Rotate(0f, giro * velocidadRotacion * Time.deltaTime, 0f, Space.World);
    }

}