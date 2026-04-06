using UnityEngine;

public sealed class VibracionMotor : MonoBehaviour
{
    [Header("Configuración de Vibración")]
    public float intensidad = 0.05f; // Qué tanto se mueve (distancia)
    public float rapidez = 20f;     // Qué tan rápido vibra

    private Vector3 posicionOriginal;

    void Start()
    {
        // Guardamos la posición local relativa al padre (la Moto)
        posicionOriginal = transform.localPosition;
    }

    void Update()
    {
        // Generamos un desfase aleatorio usando Mathf.Sin para que sea fluido
        // o Random para que sea más errático. Usaremos una mezcla suave:
        float offsetX = (Mathf.PerlinNoise(Time.time * rapidez, 0) - 0.5f) * intensidad;
        float offsetY = (Mathf.PerlinNoise(0, Time.time * rapidez) - 0.5f) * intensidad;

        // Aplicamos el movimiento sin perder la posición original del chasis
        transform.localPosition = posicionOriginal + new Vector3(offsetX, offsetY, 0);
    }
}