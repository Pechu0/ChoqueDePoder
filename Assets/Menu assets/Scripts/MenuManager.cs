using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject optionsPanel;
    public GameObject levelsPanel;

    [Header("Audio")]
    public AudioSource musicSource;
    public Slider volumeSlider;
    public Sprite volumeSprite;
    public Sprite muteSprite;
    public Image muteButtonImage;
    private float volumenAntes = 1f;
    private bool isMuted = false;

    [Header("Brillo")]
    public Slider brightnessSlider;
    public Image brightnessOverlay;

    [Header("Camara")]
    public Camera camaraMenu;
    public float tamanoOrtografico = 5f;

    void Start()
    {
        // Asegurar que la camara este en su estado correcto al entrar al menu
        if (camaraMenu == null)
            camaraMenu = Camera.main;
        if (camaraMenu != null)
        {
            camaraMenu.orthographic = true;
            camaraMenu.orthographicSize = tamanoOrtografico;
        }

        // Obtener la Image del botón BotonMute si no está asignada
        if (muteButtonImage == null)
        {
            Button muteButton = GameObject.Find("BotonMute")?.GetComponent<Button>();
            if (muteButton != null)
            {
                muteButtonImage = muteButton.GetComponent<Image>();
            }
        }

        // Cargar volumen guardado (por defecto 1 si no existe)
        float volumenGuardado = PlayerPrefs.GetFloat("volumen", 1f);
        musicSource.volume = volumenGuardado;
        volumeSlider.value = volumenGuardado;
        volumenAntes = volumenGuardado;

        // Cargar estado del mute
        isMuted = PlayerPrefs.GetInt("isMuted", 0) == 1;
        if (isMuted)
        {
            musicSource.volume = 0;
            if (muteButtonImage != null)
                muteButtonImage.sprite = muteSprite;
        }
        else
        {
            if (muteButtonImage != null)
                muteButtonImage.sprite = volumeSprite;
        }

        // Cargar brillo guardado (por defecto 1 si no existe)
        float briloGuardado = PlayerPrefs.GetFloat("brillo", 1f);
        brightnessSlider.value = briloGuardado;
        CambiarBrillo(briloGuardado); // Aplica el brillo al iniciar
    }

    // ===== OPCIONES =====
    public void AbrirOpciones()
    {
        optionsPanel.SetActive(true);
    }

    public void CerrarOpciones()
    {
        optionsPanel.SetActive(false);
    }

    public void AbrirNiveles()
    {
        levelsPanel.SetActive(true);
    }

    public void CerrarNiveles()
    {
        levelsPanel.SetActive(false);
    }

    // ===== VOLUMEN =====
    public void CambiarVolumen(float valor)
    {
        musicSource.volume = valor;
        PlayerPrefs.SetFloat("volumen", valor);
        volumenAntes = valor;

        // Si estaba muteado, lo desmutea automáticamente al ajustar volumen
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
            // Desmuteando
            isMuted = false;
            musicSource.volume = volumenAntes;
            muteButtonImage.sprite = volumeSprite;
        }
        else
        {
            // Muteando
            isMuted = true;
            volumenAntes = musicSource.volume;
            musicSource.volume = 0;
            muteButtonImage.sprite = muteSprite;
        }

        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
    }

    // ===== BRILLO =====
    public void CambiarBrillo(float valor)
    {
        // valor va de 0 a 1
        // Lo invertimos: si valor es 0, alfa es 1 (oscuro); si valor es 1, alfa es 0 (claro)
        float alfa = 1f - valor;

        Color color = brightnessOverlay.color;
        color.a = alfa;
        brightnessOverlay.color = color;

        PlayerPrefs.SetFloat("brillo", valor);
    }

    // ===== NIVELES =====
    public void CargarNivel1()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void CargarNivel2()
    {
        SceneManager.LoadScene("Nivel2");
    }
    // ===== SALIR =====
    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
