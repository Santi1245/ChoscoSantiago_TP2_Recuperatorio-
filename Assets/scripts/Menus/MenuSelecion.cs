using UnityEngine;
using UnityEngine.UI;

public class MenuSelecion : UIPanel
{
    [SerializeField] private GestorUI gestor; 

    public override void Mostrar()
    {
        gameObject.SetActive(true);

        if (gestor == null)
        {
            Debug.LogError("El GestorUI no se asigno a este panel");
            return;
        }
    }

    public override void Ocultar()
    {
        gameObject.SetActive(false);
    }
}
