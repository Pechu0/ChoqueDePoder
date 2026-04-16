using UnityEngine;
using UnityEngine.Events;

public class VidaJugador : MonoBehaviour
{
    [Header("Vida")]
    public float vidaMaxima = 100f;
    public float vidaActual = 100f;

    [Header("Niveles de vida (0=verde, 1=amarillo, 2=rojo)")]
    public int vidasTotales = 3;
    public int nivelVida = 0;

    [Header("Escudo")]
    public float duracionEscudo = 5f;
    private bool escudoActivo = false;
    private float timerEscudo = 0f;

    // Eventos para notificar a la UI
    public UnityEvent<float, float> onVidaCambia;   // (vidaActual, vidaMaxima)
    public UnityEvent<int> onCambioNivelVida;       // (nivelVida)
    public UnityEvent onMuerte;
    public UnityEvent<bool> onEscudoCambia;         // (activo)

    void Start()
    {
        onCambioNivelVida?.Invoke(nivelVida);
        onVidaCambia?.Invoke(vidaActual, vidaMaxima);
    }

    void Update()
    {
        if (escudoActivo)
        {
            timerEscudo -= Time.deltaTime;
            if (timerEscudo <= 0f)
                DesactivarEscudo();
        }
    }

    public void RecibirDanio(float cantidad)
    {
        if (escudoActivo) return;

        vidaActual -= cantidad;

        if (vidaActual <= 0f)
        {
            if (nivelVida < vidasTotales - 1)
            {
                // Bajar al siguiente nivel (verde -> amarillo -> rojo)
                nivelVida++;
                vidaActual = vidaMaxima;
                onCambioNivelVida?.Invoke(nivelVida);
                onVidaCambia?.Invoke(vidaActual, vidaMaxima);
            }
            else
            {
                vidaActual = 0f;
                onVidaCambia?.Invoke(vidaActual, vidaMaxima);
                onMuerte?.Invoke();
            }
            return;
        }

        onVidaCambia?.Invoke(vidaActual, vidaMaxima);
    }

    public void Curar(float cantidad)
    {
        vidaActual += cantidad;

        // Si rebasa el maximo y no estamos en la barra mas alta, subir de nivel
        while (vidaActual > vidaMaxima && nivelVida > 0)
        {
            float sobrante = vidaActual - vidaMaxima;
            nivelVida--;
            vidaActual = sobrante;
            onCambioNivelVida?.Invoke(nivelVida);
        }

        vidaActual = Mathf.Clamp(vidaActual, 0f, vidaMaxima);
        onVidaCambia?.Invoke(vidaActual, vidaMaxima);
    }

    public void ActivarEscudo()
    {
        escudoActivo = true;
        timerEscudo = duracionEscudo;
        onEscudoCambia?.Invoke(true);
    }

    private void DesactivarEscudo()
    {
        escudoActivo = false;
        timerEscudo = 0f;
        onEscudoCambia?.Invoke(false);
    }

    public bool EscudoActivo() => escudoActivo;
    public float TiempoEscudoRestante() => timerEscudo;
}
