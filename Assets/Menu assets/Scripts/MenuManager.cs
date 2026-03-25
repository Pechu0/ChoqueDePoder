using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject optionsPanel;

    [Header("Audio")]
    public AudioSource musicSource;
    public Slider volumeSlider;

    void Start()
    {
        // Cargar volumen guardado (por defecto 1 si no existe)
        float volumenGuardado = PlayerPrefs.GetFloat("volumen", 1f);

        // Aplicarlo
        musicSource.volume = volumenGuardado;
        volumeSlider.value = volumenGuardado;
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

    // ===== VOLUMEN =====
    public void CambiarVolumen(float valor)
    {
        musicSource.volume = valor;
        PlayerPrefs.SetFloat("volumen", valor);
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