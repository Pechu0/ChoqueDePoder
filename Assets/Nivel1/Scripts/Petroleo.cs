using UnityEngine;

public class Petroleo : MonoBehaviour
{
    [Header("Curacion")]
    public float cantidadCuracion = 30f;

    [Header("Efecto visual (opcional)")]
    public GameObject efectoRecoleccion;

    void OnTriggerEnter2D(Collider2D otro)
    {
        VidaJugador vida = otro.GetComponent<VidaJugador>();
        if (vida == null) return;

        vida.Curar(cantidadCuracion);

        if (efectoRecoleccion != null)
            Instantiate(efectoRecoleccion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
