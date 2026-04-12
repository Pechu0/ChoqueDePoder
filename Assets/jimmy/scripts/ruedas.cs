using UnityEngine;

public class GiroObjeto2D : MonoBehaviour
{
    public float velocidadRotacion = 200f; // Velocidad de giro

    private float ultimaPosicionX;

    void Start()
    {
        ultimaPosicionX = transform.position.x;
    }

    void Update()
    {
        float posicionActualX = transform.position.x;

        // Si X cambia (aumenta o disminuye), gira siempre hacia la derecha
        if (posicionActualX != ultimaPosicionX)
        {
            transform.Rotate(0, 0, -velocidadRotacion * Time.deltaTime);
        }

        // Guardar posición actual
        ultimaPosicionX = posicionActualX;
    }
}