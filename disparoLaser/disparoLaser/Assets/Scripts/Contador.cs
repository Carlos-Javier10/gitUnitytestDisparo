using UnityEngine;
using UnityEngine.UI;

public class Contador : MonoBehaviour
{
    public Text contadorTexto; // Variable para el texto del contador

    void Start()
    {
        // Inicializar el texto del contador con puntuación inicial (0)
        contadorTexto.text = "Cubos Destruidos: 0\nEsferas Destruidas: 0";
    }
}
