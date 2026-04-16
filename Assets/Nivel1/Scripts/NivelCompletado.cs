using UnityEngine;
using UnityEngine.SceneManagement;

public class NivelCompletado : MonoBehaviour
{
    public const string KeyAbrirNiveles = "abrirNiveles";

    [Header("Panel")]
    [SerializeField] private GameObject panelNivelCompletado;

    [Header("Nombre de la escena del menu")]
    [SerializeField] private string escenaMenu = "Menu";

    void Start()
    {
        if (panelNivelCompletado != null)
            panelNivelCompletado.SetActive(false);
    }

    // Conecta este metodo al evento onCaptura de la Bandera
    public void MostrarPanel()
    {
        if (panelNivelCompletado != null)
            panelNivelCompletado.SetActive(true);
    }

    // Asigna este metodo al boton "Continuar" del panel
    public void IrAlMenuConNiveles()
    {
        PlayerPrefs.SetInt(KeyAbrirNiveles, 1);
        PlayerPrefs.Save();
        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaMenu);
    }
}
