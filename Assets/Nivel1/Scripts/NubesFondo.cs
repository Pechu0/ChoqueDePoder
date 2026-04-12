using UnityEngine;

public class NubesFondo : MonoBehaviour
{
    [SerializeField] private Camera camaraJuego;

    [Header("Movimiento automatico")]
    [SerializeField] private float velocidadX = 0.02f;
    [SerializeField] private float velocidadY = 0f;

    [Header("Offset respecto a la camara")]
    [SerializeField] private Vector2 offsetCamara = Vector2.zero;

    private MeshRenderer meshRenderer;

    void Start()
    {
        if (camaraJuego == null)
            camaraJuego = Camera.main;

        meshRenderer = GetComponent<MeshRenderer>();
    }

    void LateUpdate()
    {
        // Mover textura automaticamente con el tiempo
        Vector2 offset = meshRenderer.material.mainTextureOffset;
        offset.x += velocidadX * Time.deltaTime;
        offset.y += velocidadY * Time.deltaTime;
        meshRenderer.material.mainTextureOffset = offset;

        // Seguir la camara solo en X, Y fijo (las nubes no se mueven al saltar)
        transform.position = new Vector3(
            camaraJuego.transform.position.x + offsetCamara.x,
            transform.position.y,
            transform.position.z
        );
    }
}
