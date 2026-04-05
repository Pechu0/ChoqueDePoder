using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    [Header("Referencias UI")]
    public Slider sliderVida;
    public Image rellenoVida;
    public Image iconoEscudo;

    [Header("Colores")]
    public Color colorAlto = Color.green;
    public Color colorMedio = Color.yellow;
    public Color colorBajo = Color.red;
    public Color colorEscudo = new Color(0.2f, 0.5f, 1f, 1f);

    [Header("Jugador")]
    public VidaJugador vidaJugador;

    private bool escudoActivo = false;

    void Start()
    {
        if (vidaJugador == null)
            vidaJugador = FindObjectOfType<VidaJugador>();

        // Suscribirse a los eventos
        vidaJugador.onVidaCambia.AddListener(ActualizarBarra);
        vidaJugador.onEscudoCambia.AddListener(ActualizarEscudo);

        // Inicializar
        ActualizarBarra(vidaJugador.vidaActual, vidaJugador.vidaMaxima);
        if (iconoEscudo != null) iconoEscudo.gameObject.SetActive(false);
    }

    void ActualizarBarra(float actual, float maximo)
    {
        if (sliderVida == null) return;

        sliderVida.value = actual / maximo;

        if (rellenoVida == null) return;

        // Si el escudo esta activo, mantener color azul independientemente de la vida
        if (escudoActivo)
        {
            rellenoVida.color = colorEscudo;
            return;
        }

        float porcentaje = actual / maximo;
        if (porcentaje > 0.6f)
            rellenoVida.color = colorAlto;
        else if (porcentaje > 0.3f)
            rellenoVida.color = colorMedio;
        else
            rellenoVida.color = colorBajo;
    }

    void ActualizarEscudo(bool activo)
    {
        escudoActivo = activo;

        if (iconoEscudo != null)
            iconoEscudo.gameObject.SetActive(activo);

        // Refrescar color de la barra
        ActualizarBarra(vidaJugador.vidaActual, vidaJugador.vidaMaxima);
    }
}
