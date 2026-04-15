using UnityEngine;

public class Helicoptero : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadMin = 3f;
    [SerializeField] private float velocidadMax = 5f;
    private float velocidadActual;

    [Header("Bombas")]
    [SerializeField] private GameObject prefabBomba;
    [SerializeField] private float intervaloBombasMin = 2f;
    [SerializeField] private float intervaloBombasMax = 4f;
    [SerializeField] private Transform puntoLanzamiento;

    [Header("Destruccion")]
    [SerializeField] private float distanciaMaxima = 40f;

    [Header("Sonido")]
    [SerializeField] private AudioClip sonidoAparicion;
    [SerializeField] [Range(0f, 4f)] private float volumenAparicion = 1f;

    private Vector3 posicionInicial;
    private bool activo = false;
    private float timerBomba;

    void Start()
    {
        velocidadActual = Random.Range(velocidadMin, velocidadMax);
        posicionInicial = transform.position;
        ReiniciarTimer();

        AudioUtil.Reproducir2D(sonidoAparicion, volumenAparicion);
    }

    void Update()
    {
        if (!activo) return;

        // Se mueve de derecha a izquierda
        transform.position += Vector3.left * velocidadActual * Time.deltaTime;

        // Contador para lanzar bombas
        timerBomba -= Time.deltaTime;
        if (timerBomba <= 0f)
        {
            LanzarBomba();
            ReiniciarTimer();
        }

        if (Mathf.Abs(transform.position.x - posicionInicial.x) > distanciaMaxima)
            Destroy(gameObject);
    }

    public void Activar()
    {
        activo = true;
    }

    void ReiniciarTimer()
    {
        timerBomba = Random.Range(intervaloBombasMin, intervaloBombasMax);
    }

    void LanzarBomba()
    {
        if (prefabBomba == null) return;

        Vector3 posLanzamiento = puntoLanzamiento != null ? puntoLanzamiento.position : transform.position;
        Instantiate(prefabBomba, posLanzamiento, Quaternion.identity);
    }
}
