using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CartaFinal : MonoBehaviour
{
    [Header("Expansion al tocarla")]
    [SerializeField] private float escalaFinal = 3f;
    [SerializeField] private float duracionExpansion = 0.5f;

    [Header("Efecto (opcional)")]
    [SerializeField] private GameObject efectoRecoleccion;

    [Header("Eventos")]
    public UnityEvent onRecoger;

    private bool recogida = false;
    private bool esperandoInput = false;

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (recogida) return;
        if (!otro.CompareTag("Player")) return;

        recogida = true;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (efectoRecoleccion != null)
            Instantiate(efectoRecoleccion, transform.position, Quaternion.identity);

        onRecoger?.Invoke();
        StartCoroutine(ExpandirYEsperar());
    }

    IEnumerator ExpandirYEsperar()
    {
        // Mover la carta al centro de la camara
        Camera cam = Camera.main;
        if (cam != null)
            transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);

        // Pausar el juego — la carta usa unscaledDeltaTime para seguir animandose
        Time.timeScale = 0f;

        Vector3 escalaInicial = transform.localScale;
        Vector3 escalaObjetivo = escalaInicial * escalaFinal;

        float t = 0f;
        while (t < duracionExpansion)
        {
            t += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(escalaInicial, escalaObjetivo, t / duracionExpansion);
            yield return null;
        }
        transform.localScale = escalaObjetivo;

        esperandoInput = true;
    }

    void Update()
    {
        if (!esperandoInput) return;

        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            esperandoInput = false;

            VictoriaManager vm = FindObjectOfType<VictoriaManager>();
            if (vm != null) vm.MostrarVictoria();

            Destroy(gameObject);
        }
    }
}
