using UnityEngine;

public class TrompasController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float velocidadCorrer = 8f;

    [Header("Salto")]
    public float fuerzaSalto = 7f;
    public float multiplicadorSaltoBajo = 2f;   // gravedad extra al soltar espacio temprano
    public float multiplicadorCaida = 2.5f;     // gravedad extra al caer

    [Header("Suelo")]
    public Transform puntoSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    [Header("Golpe")]
    public float velocidadAnimGolpe = 1.5f;
    public float duracionEstimadaGolpe = 0.35f; // tiempo aprox que dura el golpe (en segundos)

    private Rigidbody2D rb;
    private Animator anim;

    private float movimiento;
    private bool enSuelo;
    private bool estaCorriendo;
    private bool estaGolpeando;
    private int golpeActual = 1;
    private bool espacioPresionado;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (estaGolpeando)
        {
            // Permitir encadenar golpe si se presiona F pasado el 40% de la animacion
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            if (Input.GetKeyDown(KeyCode.F) && state.normalizedTime >= 0.4f)
            {
                Golpear();
                return;
            }
            if (state.normalizedTime >= 0.9f && !anim.IsInTransition(0))
                FinGolpe();
            return;
        }

        movimiento = Input.GetAxis("Horizontal");
        estaCorriendo = Input.GetKey(KeyCode.LeftShift);

        enSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioSuelo, capaSuelo);

        // Leer input de espacio para el salto variable (en Update, no en FixedUpdate)
        espacioPresionado = Input.GetKey(KeyCode.Space);

        if (movimiento > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (movimiento < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }

        // Golpear en el suelo o en el aire si hay tiempo suficiente para terminar la animacion
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (enSuelo || TieneTiempoParaGolpearEnAire())
            {
                Golpear();
                return; // No actualizar parametros del animator este frame (evita que Any State -> Jumping pise al golpe)
            }
        }

        ActualizarAnimaciones();
    }

    void FixedUpdate()
    {
        if (estaGolpeando)
        {
            // Permitir gravedad natural si esta en el aire, frenar horizontal
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else
        {
            float velocidadActual = estaCorriendo ? velocidadCorrer : velocidad;
            rb.velocity = new Vector2(movimiento * velocidadActual, rb.velocity.y);
        }

        // Salto variable: mas gravedad al caer, o al soltar espacio en la subida
        // Multiplicamos por rb.gravityScale para que el efecto sea proporcional a la gravedad configurada
        if (rb.velocity.y < 0f)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * rb.gravityScale * (multiplicadorCaida - 1f) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0f && !espacioPresionado)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * rb.gravityScale * (multiplicadorSaltoBajo - 1f) * Time.fixedDeltaTime;
        }
    }

    bool TieneTiempoParaGolpearEnAire()
    {
        // Estima cuanto tiempo queda en el aire segun velocidad vertical y gravedad
        float g = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        float vy = rb.velocity.y;

        // Tiempo restante hasta llegar al suelo (aproximacion: tiempo hasta apex + tiempo de caida razonable)
        // Si sube, tiene tiempo de subida + caida; si cae, solo el tiempo de caida restante
        float tiempoRestante;
        if (vy > 0f)
            tiempoRestante = (vy / g) + 0.3f; // tiempo hasta apex + colchon
        else
            tiempoRestante = 0.2f; // ya cayendo, poco tiempo

        return tiempoRestante >= duracionEstimadaGolpe / velocidadAnimGolpe;
    }

    void ActualizarAnimaciones()
    {
        // Considera "en suelo" para animacion si toca suelo O tiene velocidad vertical casi cero (evita rebote)
        bool enSueloAnim = enSuelo || Mathf.Abs(rb.velocity.y) < 0.5f;

        anim.SetFloat("Speed", enSueloAnim ? Mathf.Abs(movimiento) : 0f);
        anim.SetBool("isRunning", enSueloAnim && estaCorriendo && Mathf.Abs(movimiento) > 0.01f);
        anim.SetBool("isGrounded", enSueloAnim);
    }

    void Golpear()
    {
        estaGolpeando = true;
        movimiento = 0f;
        anim.speed = velocidadAnimGolpe;

        anim.ResetTrigger("Golpe1");
        anim.ResetTrigger("Golpe2");

        if (golpeActual == 1)
        {
            anim.SetTrigger("Golpe1");
            golpeActual = 2;
        }
        else
        {
            anim.SetTrigger("Golpe2");
            golpeActual = 1;
        }
    }

    public void FinGolpe()
    {
        estaGolpeando = false;
        anim.speed = 1f;
        anim.CrossFade("Idle", 0.1f);
    }

    public bool EstaGolpeando() => estaGolpeando;

    void OnDrawGizmosSelected()
    {
        if (puntoSuelo == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoSuelo.position, radioSuelo);
    }
}
