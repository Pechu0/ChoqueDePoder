using UnityEngine;

public class Motociclista : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadMin = 5f;
    [SerializeField] private float velocidadMax = 8f;
    private float velocidadActual;

    [Header("Dano")]
    [SerializeField] private float danio = 20f;

    [Header("Explosion")]
    [SerializeField] private GameObject prefabExplosion;

    [Header("Destruccion")]
    [SerializeField] private float distanciaMaxima = 40f;

    [Header("Sonido")]
    [SerializeField] private AudioClip sonidoAparicion;
    [SerializeField] [Range(0f, 4f)] private float volumenAparicion = 1f;
    [SerializeField] private AudioClip sonidoExplosion;
    [SerializeField] [Range(0f, 4f)] private float volumenExplosion = 1f;

    private Vector3 posicionInicial;
    private bool activo = false;

    void Start()
    {
        velocidadActual = Random.Range(velocidadMin, velocidadMax);
        posicionInicial = transform.position;

        AudioUtil.Reproducir2D(sonidoAparicion, volumenAparicion);
    }

    void Update()
    {
        if (!activo) return;

        // Se mueve de derecha a izquierda
        transform.position += Vector3.left * velocidadActual * Time.deltaTime;

        if (Mathf.Abs(transform.position.x - posicionInicial.x) > distanciaMaxima)
            Destroy(gameObject);
    }

    public void Activar()
    {
        activo = true;
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            VidaJugador vida = otro.GetComponent<VidaJugador>();
            if (vida != null)
                vida.RecibirDanio(danio);

            Explotar();
        }
        else if (otro.CompareTag("Desnivel"))
        {
            Explotar();
        }
    }

    void Explotar()
    {
        if (prefabExplosion != null)
            Instantiate(prefabExplosion, transform.position, Quaternion.identity);

        AudioUtil.Reproducir2D(sonidoExplosion, volumenExplosion);

        Destroy(gameObject);
    }
}
