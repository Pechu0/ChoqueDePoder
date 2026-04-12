using UnityEngine;

public class Aguila : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadMin = 3f;
    [SerializeField] private float velocidadMax = 6f;
    private float velocidadActual;

    [Header("Huevo")]
    [SerializeField] private GameObject prefabHuevo;
    private bool yaSoltoHuevo = false;

    [Header("Destruccion")]
    [SerializeField] private float distanciaMaxima = 30f;

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

        if (Mathf.Abs(transform.position.x - posicionInicial.x) > distanciaMaxima)
            Destroy(gameObject);
    }

    public void Activar()
    {
        activa = true;
    }

    void OnTriggerStay2D(Collider2D otro)
    {
        if (yaSoltoHuevo) return;

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
        if (yaSoltoHuevo) return;

        yaSoltoHuevo = true;
        SoltarHuevo();
    }

    void SoltarHuevo()
    {
        if (prefabHuevo != null)
            Instantiate(prefabHuevo, transform.position, Quaternion.identity);
    }
}
