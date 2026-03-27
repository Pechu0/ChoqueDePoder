using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;
    public float velocidadCorrer = 8f;
    public float fuerzaSalto = 7f;

    private Rigidbody2D rb;
    private float movimiento;
    private bool enSuelo;

    public Transform puntoSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
void Update()
{
    movimiento = Input.GetAxis("Horizontal");

    enSuelo = rb.IsTouchingLayers(capaSuelo);

    if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
    {
        rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
    }
}

    void FixedUpdate()
    {
        // Correr con Shift
        float velocidadActual = Input.GetKey(KeyCode.LeftShift) ? velocidadCorrer : velocidad;

        rb.velocity = new Vector2(movimiento * velocidadActual, rb.velocity.y);
    }
}
