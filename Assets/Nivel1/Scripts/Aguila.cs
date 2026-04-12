using UnityEngine;

public class Aguila : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadMin = 3f;
    [SerializeField] private float velocidadMax = 6f;
    private float velocidadActual;

    [Header("Vida")]
    [SerializeField] private int golpesParaMorir = 1;
    private int golpesRecibidos = 0;

    [Header("Huevo")]
    [SerializeField] private GameObject prefabHuevo;

    [Header("Destruccion")]
    [SerializeField] private float distanciaMaxima = 30f; // se destruye al alejarse mucho de su origen

    private Vector3 posicionInicial;
    private bool activa = false;

    void Start()
    {
        velocidadActual = Random.Range(velocidadMin, velocidadMax);
        posicionInicial = transform.position;
    }

    void Update()
    {
        if (!activa) return;

        transform.position += Vector3.right * velocidadActual * Time.deltaTime;

        // Destruir si se alejo demasiado
        if (Mathf.Abs(transform.position.x - posicionInicial.x) > distanciaMaxima)
            Destroy(gameObject);
    }

    public void Activar()
    {
        activa = true;
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        // Si colisiona con el player y el player esta golpeando, recibe dano
        if (otro.CompareTag("Player"))
        {
            TrompasController player = otro.GetComponent<TrompasController>();
            if (player != null && player.EstaGolpeando())
            {
                RecibirGolpe();
            }
        }
    }

    public void RecibirGolpe()
    {
        golpesRecibidos++;
        if (golpesRecibidos >= golpesParaMorir)
        {
            SoltarHuevo();
            Destroy(gameObject);
        }
    }

    void SoltarHuevo()
    {
        if (prefabHuevo != null)
            Instantiate(prefabHuevo, transform.position, Quaternion.identity);
    }
}
