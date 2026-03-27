using UnityEngine;

public class FondoSeguidor : MonoBehaviour
{
    public Transform player; // Arrastra aquí tu Player en el Inspector

    private Vector3 offset;

    void Start()
    {
        // Calcula la diferencia inicial en posición (offset)
        offset = transform.position - player.position;
        // Ignorar el eje Y
        offset.y = 0;
    }

    void LateUpdate()
    {
        // Solo actualizar X y Z, mantener Y fijo
        Vector3 nuevaPos = player.position + offset;
        nuevaPos.y = transform.position.y; // Mantiene la Y del fondo
        transform.position = nuevaPos;
    }
}