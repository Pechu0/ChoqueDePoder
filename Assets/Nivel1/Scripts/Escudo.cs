using UnityEngine;

public class Escudo : MonoBehaviour
{
    [Header("Efecto visual (opcional)")]
    public GameObject efectoRecoleccion;

    void OnTriggerEnter2D(Collider2D otro)
    {
        VidaJugador vida = otro.GetComponent<VidaJugador>();
        if (vida == null) return;

        vida.ActivarEscudo();

        if (efectoRecoleccion != null)
            Instantiate(efectoRecoleccion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
