using UnityEngine;

public class TrompasController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float velocidadCorrer = 8f;
    public float fuerzaSalto = 7f;

    [Header("Suelo")]
    public Transform puntoSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    [Header("Golpe")]
    public float velocidadAnimGolpe = 1.5f;

    private Rigidbody2D rb;
    private Animator anim;

    private float movimiento;
    private bool enSuelo;
    private bool estaCorriendo;
    private bool estaGolpeando;
    private int golpeActual = 1;

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

        if (movimiento > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (movimiento < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }

        if (Input.GetKeyDown(KeyCode.F) && enSuelo)
        {
            Golpear();
        }

        ActualizarAnimaciones();
    }

    void FixedUpdate()
    {
        if (estaGolpeando)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        float velocidadActual = estaCorriendo ? velocidadCorrer : velocidad;
        rb.velocity = new Vector2(movimiento * velocidadActual, rb.velocity.y);
    }

    void ActualizarAnimaciones()
    {
        anim.SetFloat("Speed", enSuelo ? Mathf.Abs(movimiento) : 0f);
        anim.SetBool("isRunning", enSuelo && estaCorriendo && Mathf.Abs(movimiento) > 0.01f);
        anim.SetBool("isGrounded", enSuelo);
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

    void OnDrawGizmosSelected()
    {
        if (puntoSuelo == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoSuelo.position, radioSuelo);
    }
}
