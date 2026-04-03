using UnityEngine;

public class CamaraNivel1 : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    [Header("Limites de la camara")]
    public float minX = 490f;
    public float maxX = 14700f;
    public float minY = 170f;
    public float maxY = 500f;

    void LateUpdate()
    {
        Vector3 objetivo = new Vector3(
            player.position.x + offset.x,
            player.position.y + offset.y,
            -10f
        );

        Vector3 suavizado = Vector3.Lerp(transform.position, objetivo, smoothSpeed * Time.deltaTime);

        suavizado.x = Mathf.Clamp(suavizado.x, minX, maxX);
        suavizado.y = Mathf.Clamp(suavizado.y, minY, maxY);
        suavizado.z = -10f;

        transform.position = suavizado;
    }
}
