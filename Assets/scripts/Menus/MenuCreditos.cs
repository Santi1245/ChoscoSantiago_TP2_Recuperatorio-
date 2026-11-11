using UnityEngine;
using UnityEngine.UI;

public class MenuCreditos : UIPanel
{
    [SerializeField] private Button btVolver;
    [SerializeField] private GestorUI gestor;

    public override void Mostrar()
    {
        gameObject.SetActive(true); 

        if(gestor == null)
        {
         Debug.LogError("El GestorUI no fue asignado a "+gestor.name);
         return;
        }

        btVolver.onClick.RemoveAllListeners();
        btVolver.onClick.AddListener(()=>gestor.MostrarPaneles(0));
    }

    public override void Ocultar()
    {
        gameObject.SetActive(false);
    }
}