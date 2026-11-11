using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    public void Nivel2()
    {
        SceneManager.LoadScene("Nivel2");
    }

    public void Jugar()
    {
        SceneManager.LoadScene("SelectorDeNiveles");
    }

    public void Nivel1()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void retroceder()
    {
        SceneManager.LoadScene("menu");
    }
}
