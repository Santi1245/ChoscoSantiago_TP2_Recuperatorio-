using UnityEngine;
using System.Collections;

public class Power : MonoBehaviour
{
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private float duracionEfecto = 3f;

    private PlayerMovement player;
    private float velocidadOriginal;

    void Start()
    {
        player = GetComponent<PlayerMovement>();
        velocidadOriginal = player.Speed;
    }

    void Update()
    {
        if (isEnabled && Input.GetKeyDown(KeyCode.Z))  
        {
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        isEnabled = false;
        Debug.Log("<color=yellow>Poder habilitado</color>");

        player.Speed = velocidadOriginal * 2f;
        Debug.Log("Activando poder especial");

        yield return new WaitForSeconds(duracionEfecto);

        player.Speed = velocidadOriginal;

        yield return new WaitForSeconds(cooldown);

        isEnabled = true;
        Debug.Log("<color=red>Poder desabilitado</color>");
    }
}
