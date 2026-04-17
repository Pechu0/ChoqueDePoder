using UnityEngine;

public class TriggerJefe : MonoBehaviour
{
    [SerializeField] private Jefe jefe;
    [SerializeField] private CamaraNivel2 camara;
    [Header("Muros de la arena (desactivados por defecto)")]
    [SerializeField] private GameObject muroIzquierdo;
    [SerializeField] private GameObject muroDerecho;
    private bool yaActivado = false;

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (yaActivado) return;
        if (!otro.CompareTag("Player")) return;

        yaActivado = true;

        if (muroIzquierdo != null) muroIzquierdo.SetActive(true);
        if (muroDerecho != null) muroDerecho.SetActive(true);

        if (camara != null)
            camara.BloquearParaJefe();

        if (jefe != null)
        {
            jefe.gameObject.SetActive(true);
            jefe.Activar();
        }
    }
}
