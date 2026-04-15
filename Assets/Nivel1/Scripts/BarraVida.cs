using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    [Header("Referencias UI")]
    public Slider sliderVida;
    public Image rellenoVida;
    public Image iconoEscudo;

    [Header("Colores por nivel de vida")]
    public Color colorVerde = Color.green;     // nivel 0 (primera barra)
    public Color colorAmarillo = Color.yellow; // nivel 1 (segunda barra)
    public Color colorRojo = Color.red;        // nivel 2 (tercera barra)
    public Color colorEscudo = new Color(0.2f, 0.5f, 1f, 1f);

    [Header("Jugador")]
    public VidaJugador vidaJugador;

    private bool escudoActivo = false;
    private int nivelVida = 0;

    void Start()
    {
        if (vidaJugador == null)
            vidaJugador = FindObjectOfType<VidaJugador>();

        vidaJugador.onVidaCambia.AddListener(ActualizarBarra);
        vidaJugador.onCambioNivelVida.AddListener(ActualizarNivel);
        vidaJugador.onEscudoCambia.AddListener(ActualizarEscudo);

        nivelVida = vidaJugador.nivelVida;
        ActualizarBarra(vidaJugador.vidaActual, vidaJugador.vidaMaxima);
        if (iconoEscudo != null) iconoEscudo.gameObject.SetActive(false);
    }

    void ActualizarBarra(float actual, float maximo)
    {
        if (sliderVida != null)
            sliderVida.value = actual / maximo;

        AplicarColor();
    }

    void ActualizarNivel(int nivel)
    {
        nivelVida = nivel;
        AplicarColor();
    }

    void ActualizarEscudo(bool activo)
    {
        escudoActivo = activo;

        if (iconoEscudo != null)
            iconoEscudo.gameObject.SetActive(activo);

        AplicarColor();
    }

    void AplicarColor()
    {
        if (rellenoVida == null) return;

        if (escudoActivo)
        {
            rellenoVida.color = colorEscudo;
            return;
        }

        switch (nivelVida)
        {
            case 0: rellenoVida.color = colorVerde; break;
            case 1: rellenoVida.color = colorAmarillo; break;
            default: rellenoVida.color = colorRojo; break;
        }
    }
}
