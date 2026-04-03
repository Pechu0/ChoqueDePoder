using UnityEngine;

public class ParallaxNivel1 : MonoBehaviour
{
    [SerializeField] private Camera camaraJuego;
    [SerializeField] private float velocidadParallax = 0.05f;

    [Header("Offset respecto a la camara")]
    [SerializeField] private Vector2 offsetCamara = Vector2.zero;

    private MeshRenderer meshRenderer;
    private float posAnteriorCamaraX;

    void Start()
    {
        if (camaraJuego == null)
            camaraJuego = Camera.main;

        meshRenderer = GetComponent<MeshRenderer>();
        posAnteriorCamaraX = camaraJuego.transform.position.x;

        // Seguir la camara desde el inicio
        SeguirCamara();
    }

    void LateUpdate()
    {
        // Mover la textura segun el movimiento de la camara
        float deltaCamara = camaraJuego.transform.position.x - posAnteriorCamaraX;
        posAnteriorCamaraX = camaraJuego.transform.position.x;

        Vector2 offset = meshRenderer.material.mainTextureOffset;
        offset.x += deltaCamara * velocidadParallax;
        meshRenderer.material.mainTextureOffset = offset;

        // El objeto sigue la posicion de la camara
        SeguirCamara();
    }

    void SeguirCamara()
    {
        transform.position = new Vector3(
            camaraJuego.transform.position.x + offsetCamara.x,
            camaraJuego.transform.position.y + offsetCamara.y,
            transform.position.z
        );
    }
}
