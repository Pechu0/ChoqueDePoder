using UnityEngine;
using System.Collections;

public class EnemigoMilitar : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 3f;

    [Header("Ataque")]
    [SerializeField] private float rangoAtaque = 1.2f;
    [Tooltip("Diferencia vertical maxima para que el ataque conecte (evita pegar al jugador en el aire)")]
    [SerializeField] private float rangoAtaqueVertical = 1.2f;
    [SerializeField] private float danio = 10f;
    [SerializeField] private float cooldownAtaque = 1.2f;
    [Tooltip("Tiempo desde que inicia la animacion de ataque hasta que se aplica el dano")]
    [SerializeField] private float delayDanio = 0.35f;
    [Tooltip("Duracion total estimada de la animacion de ataque")]
    [SerializeField] private float duracionAtaque = 0.7f;
    [Tooltip("Tiempo en idle despues de atacar antes de volver a moverse")]
    [SerializeField] private float pausaPostAtaque = 0.5f;

    [Header("Vida")]
    [SerializeField] private int golpesParaMorir = 3;
    [SerializeField] private float cooldownRecibirGolpe = 0.3f;
    [Tooltip("Distancia horizontal maxima a la que el golpe del jugador afecta al enemigo")]
    [SerializeField] private float rangoRecibirGolpe = 1.8f;
    [Tooltip("Diferencia vertical maxima para recibir golpe (evita matar desde arriba saltando)")]
    [SerializeField] private float rangoRecibirGolpeVertical = 1.2f;

    [Header("Orientacion")]
    [Tooltip("Marcar si el sprite base (escala positiva) mira a la derecha")]
    [SerializeField] private bool miraDerechaEnBase = true;

    [Header("Audio")]
    [SerializeField] private AudioClip sonidoAtaque;
    [Range(0f, 4f)] [SerializeField] private float volumenAtaque = 1f;
    [SerializeField] private AudioClip sonidoMuerte;
    [Range(0f, 4f)] [SerializeField] private float volumenMuerte = 1f;

    [Header("Destruccion")]
    [Tooltip("Tiempo despues de morir para destruir el GameObject (debe cubrir la animacion Die)")]
    [SerializeField] private float tiempoDestruirTrasMorir = 1.2f;

    private Transform jugador;
    private Animator anim;
    private Rigidbody2D rb;

    private int golpesRecibidos = 0;
    private float timerCooldownAtaque = 0f;
    private float timerCooldownGolpe = 0f;
    private bool atacando = false;
    private bool muerto = false;
    private bool activo = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) jugador = p.transform;
    }

    public void Activar()
    {
        activo = true;
    }

    void Update()
    {
        if (muerto || !activo) return;

        timerCooldownAtaque -= Time.deltaTime;
        timerCooldownGolpe -= Time.deltaTime;

        if (jugador == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) jugador = p.transform;
            return;
        }

        DetectarGolpeJugador();

        float dx = jugador.position.x - transform.position.x;
        float distancia = Mathf.Abs(dx);

        Girar(dx);

        if (atacando)
        {
            Detener();
            return;
        }

        if (distancia <= rangoAtaque)
        {
            Detener();
            if (timerCooldownAtaque <= 0f)
                StartCoroutine(Atacar());
        }
        else
        {
            Caminar(Mathf.Sign(dx));
        }
    }

    void Caminar(float direccion)
    {
        if (rb != null)
            rb.velocity = new Vector2(direccion * velocidad, rb.velocity.y);
        else
            transform.position += new Vector3(direccion * velocidad * Time.deltaTime, 0f, 0f);

        SetBoolSeguro("Walking", true);
    }

    void Detener()
    {
        if (rb != null) rb.velocity = new Vector2(0f, rb.velocity.y);
        SetBoolSeguro("Walking", false);
    }

    void SetBoolSeguro(string nombre, bool valor)
    {
        if (anim == null) return;
        foreach (AnimatorControllerParameter p in anim.parameters)
        {
            if (p.name == nombre && p.type == AnimatorControllerParameterType.Bool)
            {
                anim.SetBool(nombre, valor);
                return;
            }
        }
    }

    void Girar(float dx)
    {
        if (Mathf.Abs(dx) < 0.01f) return;

        float escalaX = Mathf.Abs(transform.localScale.x);
        bool jugadorDerecha = dx > 0f;
        bool mirarDerecha = jugadorDerecha;
        float signo = (mirarDerecha == miraDerechaEnBase) ? 1f : -1f;

        transform.localScale = new Vector3(signo * escalaX, transform.localScale.y, transform.localScale.z);
    }

    IEnumerator Atacar()
    {
        atacando = true;
        timerCooldownAtaque = cooldownAtaque;

        SetBoolSeguro("Walking", false);
        SetBoolSeguro("Attacking", true);
        AudioUtil.Reproducir2D(sonidoAtaque, volumenAtaque);

        yield return new WaitForSeconds(delayDanio);

        if (!muerto) AplicarDanio();

        float resto = Mathf.Max(0f, duracionAtaque - delayDanio);
        if (resto > 0f) yield return new WaitForSeconds(resto);

        SetBoolSeguro("Attacking", false);

        // Pausa en idle antes de volver a perseguir
        if (pausaPostAtaque > 0f) yield return new WaitForSeconds(pausaPostAtaque);

        atacando = false;
    }

    void AplicarDanio()
    {
        if (jugador == null) return;
        float distanciaX = Mathf.Abs(jugador.position.x - transform.position.x);
        float distanciaY = Mathf.Abs(jugador.position.y - transform.position.y);
        if (distanciaX > rangoAtaque) return;
        if (distanciaY > rangoAtaqueVertical) return;

        VidaJugador vida = jugador.GetComponent<VidaJugador>();
        if (vida != null) vida.RecibirDanio(danio);
    }

    void DetectarGolpeJugador()
    {
        if (timerCooldownGolpe > 0f) return;

        float distanciaX = Mathf.Abs(jugador.position.x - transform.position.x);
        float distanciaY = Mathf.Abs(jugador.position.y - transform.position.y);
        if (distanciaX > rangoRecibirGolpe) return;
        if (distanciaY > rangoRecibirGolpeVertical) return;

        TrompasController p = jugador.GetComponent<TrompasController>();
        if (p != null && p.EstaGolpeando())
        {
            timerCooldownGolpe = cooldownRecibirGolpe;
            RecibirGolpe();
        }
    }

    public void RecibirGolpe()
    {
        if (muerto) return;

        golpesRecibidos++;
        if (golpesRecibidos >= golpesParaMorir)
            Morir();
    }

    void Morir()
    {
        muerto = true;

        if (anim != null) anim.SetTrigger("Die");
        AudioUtil.Reproducir2D(sonidoMuerte, volumenMuerte);

        // Desactivar solo los colliders que son triggers (detectar golpes del player)
        // para no perder la colision con el suelo
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            if (c.isTrigger) c.enabled = false;
        }

        // Congelar el rigidbody para que no atraviese el suelo ni se mueva
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Destroy(gameObject, tiempoDestruirTrasMorir);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoRecibirGolpe);
    }
}
