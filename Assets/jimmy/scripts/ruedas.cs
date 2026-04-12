using UnityEngine;

public sealed class GirarRuedas : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidadGiro = 500f; // Ajusta qué tan rápido giran visualmente
    
    private float lastXPosition;

    void Start()
    {
        lastXPosition = transform.parent.position.x; // Usamos la posición del padre (la Moto)
    }

    void Update()
    {
        // Obtenemos la posición actual del objeto padre (la Moto)
        float currentX = transform.parent.position.x;
        float diferencia = currentX - lastXPosition;

        // Si hay movimiento, aplicamos la rotación
        if (Mathf.Abs(diferencia) > 0.001f)
        {
            // Si diferencia > 0 (derecha), rotación negativa (sentido horario)
            // Si diferencia < 0 (izquierda), rotación positiva (sentido antihorario)
            float direccion = (diferencia > 0) ? -1f : 1f;
            
            // Aplicamos la rotación en el eje Z
            transform.Rotate(0, 0, direccion * velocidadGiro * Time.deltaTime);
        }

        lastXPosition = currentX;
    }
}
