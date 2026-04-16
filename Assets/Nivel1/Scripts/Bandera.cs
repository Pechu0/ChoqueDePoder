using UnityEngine;
using UnityEngine.Events;

public class Bandera : MonoBehaviour
{
    public const string KeyNivel1Completado = "nivel1_completado";

    [Header("Efecto (opcional)")]
    [SerializeField] private GameObject efectoCaptura;

    [Header("Eventos")]
    public UnityEvent onCaptura;

    private bool capturada = false;

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (capturada) return;
        if (!otro.CompareTag("Player")) return;

        capturada = true;

        PlayerPrefs.SetInt(KeyNivel1Completado, 1);
        PlayerPrefs.Save();

        if (efectoCaptura != null)
            Instantiate(efectoCaptura, transform.position, Quaternion.identity);

        onCaptura?.Invoke();

        Destroy(gameObject);
    }
}
