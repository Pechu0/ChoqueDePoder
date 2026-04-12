using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject gameOverPanel;

    [Header("Jugador")]
    public VidaJugador vidaJugador;

    void Start()
    {
        gameOverPanel.SetActive(false);

        if (vidaJugador == null)
            vidaJugador = FindObjectOfType<VidaJugador>();

        vidaJugador.onMuerte.AddListener(MostrarGameOver);
    }

    void MostrarGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Reintentar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
