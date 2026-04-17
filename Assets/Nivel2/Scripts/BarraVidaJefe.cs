using UnityEngine;
using UnityEngine.UI;

public class BarraVidaJefe : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject contenedor; // panel que se muestra/oculta
    [SerializeField] private Slider slider;
    [SerializeField] private Image relleno;
    [SerializeField] private Jefe jefe;

    [Header("Colores por nivel")]
    public Color colorVerde = Color.green;
    public Color colorAmarillo = Color.yellow;
    public Color colorRojo = Color.red;

    void Start()
    {
        if (contenedor != null) contenedor.SetActive(false);

        if (jefe != null)
        {
            jefe.onAparecer.AddListener(Mostrar);
            jefe.onVidaCambia.AddListener(ActualizarBarra);
            jefe.onCambioNivelVida.AddListener(ActualizarColor);
            jefe.onMuerte.AddListener(Ocultar);
        }
    }

    void Mostrar()
    {
        if (contenedor != null) contenedor.SetActive(true);
    }

    void Ocultar()
    {
        if (contenedor != null) contenedor.SetActive(false);
    }

    void ActualizarBarra(int actual, int maximo)
    {
        if (slider != null && maximo > 0)
            slider.value = (float)actual / maximo;
    }

    void ActualizarColor(int nivel)
    {
        if (relleno == null) return;
        switch (nivel)
        {
            case 0: relleno.color = colorVerde; break;
            case 1: relleno.color = colorAmarillo; break;
            default: relleno.color = colorRojo; break;
        }
    }

    public void SetJefe(Jefe j) { jefe = j; Start(); }
}
