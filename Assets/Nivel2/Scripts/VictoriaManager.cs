using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoriaManager : MonoBehaviour
{
    public const string KeyAbrirNiveles = "abrirNiveles";
    public const string KeyNivel2Completado = "nivel2_completado";

    [Header("Panel de victoria")]
    [SerializeField] private GameObject panelVictoria;

    [Header("Escena menu")]
    [SerializeField] private string escenaMenu = "Menu";

    void Start()
    {
        if (panelVictoria != null) panelVictoria.SetActive(false);
    }

    // Llamado por CartaFinal tras expandirse y recibir input
    public void MostrarVictoria()
    {
        if (panelVictoria != null) panelVictoria.SetActive(true);
    }

    // Conectar al boton "Menu" del panel de victoria
    public void IrAlMenu()
    {
        PlayerPrefs.SetInt(KeyNivel2Completado, 1);
        PlayerPrefs.SetInt(KeyAbrirNiveles, 1);
        PlayerPrefs.Save();
        // Restaurar timeScale solo al cambiar de escena
        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaMenu);
    }
}
