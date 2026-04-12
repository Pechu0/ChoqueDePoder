using UnityEngine;

public class Bomba : MonoBehaviour
{
    [Header("Dano")]
    [SerializeField] private float danio = 25f;

    [Header("Caida")]
    [SerializeField] private float velocidadCaida = 5f;
    [SerializeField] private bool usarGravedadRigidbody = false;

    [Header("Explosion")]
    [SerializeField] private GameObject prefabExplosion;

    private bool explotada = false;

    void Update()
    {
        if (explotada) return;

        if (!usarGravedadRigidbody)
            transform.position += Vector3.down * velocidadCaida * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (explotada) return;

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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (explotada) return;

        if (col.gameObject.CompareTag("Player"))
        {
            VidaJugador vida = col.gameObject.GetComponent<VidaJugador>();
            if (vida != null)
                vida.RecibirDanio(danio);
        }

        Explotar();
    }

    void Explotar()
    {
        explotada = true;

        if (prefabExplosion != null)
            Instantiate(prefabExplosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
