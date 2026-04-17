using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Jefe : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 3.5f;
    [SerializeField] private float fuerzaSalto = 8f;

    [Header("Ataque")]
    [SerializeField] private float rangoAtaque = 1.4f;
    [SerializeField] private float rangoAtaqueVertical = 1.4f;
    [SerializeField] private float danio = 15f;
    [SerializeField] private float cooldownAtaque = 1.5f;
    [SerializeField] private float delayDanio = 0.4f;
    [SerializeField] private float duracionAtaque = 0.8f;
    [SerializeField] private float pausaPostAtaque = 0.6f;

    [Header("Vida (3 barras)")]
    [SerializeField] private int golpesPorBarra = 5;
    [SerializeField] private int vidasTotales = 3;
    [SerializeField] private float cooldownRecibirGolpe = 0.3f;
    [SerializeField] private float rangoRecibirGolpe = 2f;

    [Header("Buff por fase")]
    [Tooltip("Multiplicador aplicado a velocidad y dano al perder una barra")]
    [SerializeField] private float multiplicadorBuff = 1.4f;

    [Header("Orientacion")]
    [SerializeField] private bool miraDerechaEnBase = true;

    [Header("Audio")]
    [SerializeField] private AudioClip sonidoAtaque;
    [Range(0f, 4f)] [SerializeField] private float volumenAtaque = 1f;
    [SerializeField] private AudioClip sonidoMuerte;
    [Range(0f, 4f)] [SerializeField] private float volumenMuerte = 1f;
    [SerializeField] private AudioClip sonidoBuff;
    [Range(0f, 4f)] [SerializeField] private float volumenBuff = 1f;

    [Header("Muerte")]
    [SerializeField] private GameObject prefabCarta;
    [SerializeField] private float offsetYCarta = 2f;
    [SerializeField] private float tiempoDestruirTrasMorir = 1.5f;

    [Header("Eventos")]
    public UnityEvent onAparecer;
    public UnityEvent<int, int> onVidaCambia;       // (hitsActuales, golpesPorBarra)
    public UnityEvent<int> onCambioNivelVida;       // (nivelVida)
    public UnityEvent onMuerte;

    private Transform jugador;
    private Animator anim;
    private Rigidbody2D rb;

    private int hitsActuales;
    private int nivelVida = 0;
    private float timerCooldownAtaque = 0f;
    private float timerCooldownGolpe = 0f;
    private bool atacando = false;
    private bool muerto = false;
    private bool activo = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Activar()
    {
        hitsActuales = golpesPorBarra;
        activo = true;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) jugador = p.transform;

        onAparecer?.Invoke();
        onCambioNivelVida?.Invoke(nivelVida);
        onVidaCambia?.Invoke(hitsActuales, golpesPorBarra);
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

        if (atacando) { Detener(); return; }

        if (distancia <= rangoAtaque)
        {
            Detener();
            if (timerCooldownAtaque <= 0f) StartCoroutine(Atacar());
        }
        else
        {
            Caminar(Mathf.Sign(dx));
        }
    }

    void Caminar(float dir)
    {
        if (rb != null) rb.velocity = new Vector2(dir * velocidad, rb.velocity.y);
        else transform.position += new Vector3(dir * velocidad * Time.deltaTime, 0f, 0f);
        SetBoolSeguro("Walking", true);
    }

    void Detener()
    {
        if (rb != null) rb.velocity = new Vector2(0f, rb.velocity.y);
        SetBoolSeguro("Walking", false);
    }

    void Girar(float dx)
    {
        if (Mathf.Abs(dx) < 0.01f) return;
        float escalaX = Mathf.Abs(transform.localScale.x);
        bool jugadorDerecha = dx > 0f;
        float signo = (jugadorDerecha == miraDerechaEnBase) ? 1f : -1f;
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

        if (pausaPostAtaque > 0f) yield return new WaitForSeconds(pausaPostAtaque);
        atacando = false;
    }

    void AplicarDanio()
    {
        if (jugador == null) return;
        float dx = Mathf.Abs(jugador.position.x - transform.position.x);
        float dy = Mathf.Abs(jugador.position.y - transform.position.y);
        if (dx > rangoAtaque || dy > rangoAtaqueVertical) return;

        VidaJugador vida = jugador.GetComponent<VidaJugador>();
        if (vida != null) vida.RecibirDanio(danio);
    }

    void DetectarGolpeJugador()
    {
        if (timerCooldownGolpe > 0f) return;
        float distancia = Mathf.Abs(jugador.position.x - transform.position.x);
        if (distancia > rangoRecibirGolpe) return;

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

        hitsActuales--;
        onVidaCambia?.Invoke(hitsActuales, golpesPorBarra);

        if (hitsActuales <= 0)
        {
            if (nivelVida < vidasTotales - 1)
            {
                nivelVida++;
                hitsActuales = golpesPorBarra;
                onCambioNivelVida?.Invoke(nivelVida);
                onVidaCambia?.Invoke(hitsActuales, golpesPorBarra);
                AplicarBuff();
            }
            else
            {
                Morir();
            }
        }
    }

    void AplicarBuff()
    {
        velocidad *= multiplicadorBuff;
        cooldownAtaque /= multiplicadorBuff;
        duracionAtaque /= multiplicadorBuff;
        pausaPostAtaque /= multiplicadorBuff;
        AudioUtil.Reproducir2D(sonidoBuff, volumenBuff);

        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            SetTriggerSeguro("Jump");
        }
    }

    void Morir()
    {
        muerto = true;
        SetTriggerSeguro("Die");
        AudioUtil.Reproducir2D(sonidoMuerte, volumenMuerte);

        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols) if (c.isTrigger) c.enabled = false;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        if (prefabCarta != null)
        {
            Vector3 posCarta = new Vector3(transform.position.x, transform.position.y + offsetYCarta, transform.position.z);
            Instantiate(prefabCarta, posCarta, Quaternion.identity);
        }

        onMuerte?.Invoke();
        Destroy(gameObject, tiempoDestruirTrasMorir);
    }

    void SetBoolSeguro(string nombre, bool valor)
    {
        if (anim == null) return;
        foreach (AnimatorControllerParameter p in anim.parameters)
            if (p.name == nombre && p.type == AnimatorControllerParameterType.Bool)
            { anim.SetBool(nombre, valor); return; }
    }

    void SetTriggerSeguro(string nombre)
    {
        if (anim == null) return;
        foreach (AnimatorControllerParameter p in anim.parameters)
            if (p.name == nombre && p.type == AnimatorControllerParameterType.Trigger)
            { anim.SetTrigger(nombre); return; }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoRecibirGolpe);
    }
}
