using UnityEngine;

public class Huevo : MonoBehaviour
{
    [Header("Caida")]
    [SerializeField] private float velocidadCaida = 6f;

    [Header("Deteccion de suelo")]
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float distanciaDeteccion = 0.3f;

    [Header("Explosion al tocar el suelo")]
    [SerializeField] private GameObject prefabMiniExplosion;

    [Header("Bonificadores (se elige uno al azar)")]
    [SerializeField] private GameObject[] bonificadores;

    private bool reventado = false;

    void Update()
    {
        if (reventado) return;

        transform.position += Vector3.down * velocidadCaida * Time.deltaTime;

        // Raycast hacia abajo para detectar el suelo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanciaDeteccion, capaSuelo);
        if (hit.collider != null)
        {
            Reventar();
        }
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (reventado) return;

        if (otro.CompareTag("Suelo") || otro.CompareTag("Desnivel"))
            Reventar();
    }

    void OnTriggerStay2D(Collider2D otro)
    {
        if (reventado) return;

        if (otro.CompareTag("Suelo") || otro.CompareTag("Desnivel"))
            Reventar();
    }

    void Reventar()
    {
        reventado = true;

        if (prefabMiniExplosion != null)
            Instantiate(prefabMiniExplosion, transform.position, Quaternion.identity);

        if (bonificadores != null && bonificadores.Length > 0)
        {
            int indice = Random.Range(0, bonificadores.Length);
            if (bonificadores[indice] != null)
                Instantiate(bonificadores[indice], transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
