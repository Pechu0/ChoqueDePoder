using UnityEngine;
using UnityEngine.Events;

public class VidaJugador : MonoBehaviour
{
    [Header("Vida")]
    public float vidaMaxima = 100f;
    public float vidaActual = 100f;

    [Header("Escudo")]
    public float duracionEscudo = 5f;
    private bool escudoActivo = false;
    private float timerEscudo = 0f;

    // Eventos para notificar a la UI
    public UnityEvent<float, float> onVidaCambia;   // (vidaActual, vidaMaxima)
    public UnityEvent onMuerte;
    public UnityEvent<bool> onEscudoCambia;         // (activo)

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
        vidaActual = Mathf.Clamp(vidaActual, 0f, vidaMaxima);
        onVidaCambia?.Invoke(vidaActual, vidaMaxima);

        if (vidaActual <= 0f)
            onMuerte?.Invoke();
    }

    public void Curar(float cantidad)
    {
        vidaActual += cantidad;
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
