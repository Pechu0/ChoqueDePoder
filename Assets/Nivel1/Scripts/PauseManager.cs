using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Panel de Pausa")]
    public GameObject pausePanel;

    [Header("Audio")]
    public AudioSource musicSource;
    public Slider volumeSlider;
    public Sprite volumeSprite;
    public Sprite muteSprite;
    public Image muteButtonImage;

    [Header("Brillo")]
    public Slider brightnessSlider;
    public Image brightnessOverlay;

    private bool pausado = false;
    private float volumenAntes = 1f;
    private bool isMuted = false;

    void Start()
    {
        pausePanel.SetActive(false);

        // Cargar valores guardados del menu
        float volumen = PlayerPrefs.GetFloat("volumen", 1f);
        volumenAntes = volumen;
        isMuted = PlayerPrefs.GetInt("isMuted", 0) == 1;

        if (musicSource != null)
        {
            musicSource.volume = isMuted ? 0f : volumen;
            if (volumeSlider != null) volumeSlider.value = volumen;
        }

        if (muteButtonImage != null)
            muteButtonImage.sprite = isMuted ? muteSprite : volumeSprite;

        float brillo = PlayerPrefs.GetFloat("brillo", 1f);
        if (brightnessSlider != null) brightnessSlider.value = brillo;
        if (brightnessOverlay != null) CambiarBrillo(brillo);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausado) Reanudar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        pausado = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void Reanudar()
    {
        pausado = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void CambiarVolumen(float valor)
    {
        if (musicSource != null) musicSource.volume = valor;
        PlayerPrefs.SetFloat("volumen", valor);
        volumenAntes = valor;

        // Si estaba muteado, lo desmutea al ajustar el slider
        if (isMuted && muteButtonImage != null)
        {
            isMuted = false;
            muteButtonImage.sprite = volumeSprite;
            PlayerPrefs.SetInt("isMuted", 0);
        }
    }

    public void ToggleMute()
    {
        if (muteButtonImage == null) return;

        if (isMuted)
        {
            isMuted = false;
            musicSource.volume = volumenAntes;
            muteButtonImage.sprite = volumeSprite;
        }
        else
        {
            isMuted = true;
            volumenAntes = musicSource.volume;
            musicSource.volume = 0f;
            muteButtonImage.sprite = muteSprite;
        }

        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
    }

    public void CambiarBrillo(float valor)
    {
        if (brightnessOverlay == null) return;
        float alfa = 1f - valor;
        Color color = brightnessOverlay.color;
        color.a = alfa;
        brightnessOverlay.color = color;
        PlayerPrefs.SetFloat("brillo", valor);
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
