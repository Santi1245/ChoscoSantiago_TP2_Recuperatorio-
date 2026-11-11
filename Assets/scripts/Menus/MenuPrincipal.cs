using UnityEngine;
using UnityEngine.UI;

public class MenuPrincipal : UIPanel
{
    [SerializeField] private Button btCreditos;
    [SerializeField] private Button btSalir;
    [SerializeField] private GestorUI gestor;

public override void Mostrar ( )
{
    gameObject.SetActive(true);
    
    if(gestor == null)
    {
       Debug.LogError("El GestorUI no se asigno a este panel ->"+ gestor.name);
       return;
    }

   btCreditos.onClick.RemoveAllListeners();
   btCreditos.onClick.AddListener(()=>gestor.MostrarPaneles(1));

   btSalir.onClick.RemoveAllListeners();
   btSalir.onClick.AddListener(()=>gestor.Salir());
}

public override void Ocultar()
{
     gameObject.SetActive(false);
}

}