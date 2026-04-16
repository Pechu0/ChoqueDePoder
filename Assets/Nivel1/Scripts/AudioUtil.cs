using UnityEngine;

public static class AudioUtil
{
    public const float VolumenMaximo = 4f;

    public static float VolumenMaestro()
    {
        if (PlayerPrefs.GetInt("isMuted", 0) == 1) return 0f;
        return PlayerPrefs.GetFloat("volumen", 1f);
    }

    public static void Reproducir2D(AudioClip clip, float volumen = 1f)
    {
        if (clip == null) return;

        float total = Mathf.Clamp(volumen, 0f, VolumenMaximo) * VolumenMaestro();
        if (total <= 0f) return;

        // Como AudioSource.volume se limita a 1, repartimos el volumen
        // en varias fuentes superpuestas para poder superar el 100%.
        while (total > 0f)
        {
            float parcial = Mathf.Min(total, 1f);
            total -= parcial;

            GameObject go = new GameObject("SonidoTemp");
            AudioSource src = go.AddComponent<AudioSource>();
            src.clip = clip;
            src.volume = parcial;
            src.spatialBlend = 0f;
            src.playOnAwake = false;
            src.Play();

            Object.Destroy(go, clip.length + 0.1f);
        }
    }
}
