using UnityEngine;

public class Bomba : MonoBehaviour
{
    [Header("Dano")]
    [SerializeField] private float danio = 25f;

    [Header("Caida")]
    [SerializeField] private float velocidadCaida = 5f;

    [Header("Deteccion de suelo")]
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float distanciaDeteccion = 0.3f;

    [Header("Explosion")]
    [SerializeField] private GameObject prefabExplosion;

    [Header("Sonido")]
    [SerializeField] private AudioClip sonidoExplosion;
    [SerializeField] [Range(0f, 4f)] private float volumenExplosion = 1f;

    private bool explotada = false;

    void Update()
    {
        if (explotada) return;

        transform.position += Vector3.down * velocidadCaida * Time.deltaTime;

        // Raycast hacia abajo para detectar el suelo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanciaDeteccion, capaSuelo);
        if (hit.collider != null)
        {
            Explotar();
        }
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (explotada) return;
        Verificar(otro);
    }

    void OnTriggerStay2D(Collider2D otro)
    {
        if (explotada) return;
        Verificar(otro);
    }

    void Verificar(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            VidaJugador vida = otro.GetComponent<VidaJugador>();
            if (vida != null)
                vida.RecibirDanio(danio);

            Explotar();
        }
        else if (otro.CompareTag("Suelo") || otro.CompareTag("Desnivel"))
        {
            Explotar();
        }
    }

    void Explotar()
    {
        explotada = true;

        if (prefabExplosion != null)
            Instantiate(prefabExplosion, transform.position, Quaternion.identity);

        AudioUtil.Reproducir2D(sonidoExplosion, volumenExplosion);

        Destroy(gameObject);
    }
}
