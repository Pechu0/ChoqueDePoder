using UnityEngine;
using UnityEngine.UI;

public class Nivel2Bloqueado : MonoBehaviour
{
    [Header("Referencias")]
    public Button botonNivel2;
    public RawImage iconoCandado;

    void OnEnable()
    {
        Refrescar();
    }

    void Start()
    {
        Refrescar();
    }

    void Refrescar()
    {
        bool desbloqueado = PlayerPrefs.GetInt(Bandera.KeyNivel1Completado, 0) == 1;

        if (botonNivel2 != null)
            botonNivel2.interactable = desbloqueado;

        if (iconoCandado != null)
            iconoCandado.gameObject.SetActive(!desbloqueado);
    }

    [ContextMenu("Desbloquear Nivel 2 (pruebas)")]
    public void DesbloquearNivel2()
    {
        PlayerPrefs.SetInt(Bandera.KeyNivel1Completado, 1);
        PlayerPrefs.Save();
        Refrescar();
    }

    [ContextMenu("Bloquear Nivel 2 (pruebas)")]
    public void BloquearNivel2()
    {
        PlayerPrefs.DeleteKey(Bandera.KeyNivel1Completado);
        PlayerPrefs.Save();
        Refrescar();
    }
}
