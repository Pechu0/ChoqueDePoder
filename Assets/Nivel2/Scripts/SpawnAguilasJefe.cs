using UnityEngine;
using System.Collections;

public class SpawnAguilasJefe : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject prefabAguila;

    [Header("Spawn")]
    [SerializeField] private int cantidadPorOleada = 1;
    [SerializeField] private float intervaloMin = 10f;
    [SerializeField] private float intervaloMax = 20f;

    [Header("Posicion")]
    [SerializeField] private float offsetXAparicion = 15f;
    [SerializeField] private bool usarYFijo = false;
    [SerializeField] private float posicionYFija = 5f;
    [SerializeField] private float yMin = 3f;
    [SerializeField] private float yMax = 6f;

    [Header("Jefe")]
    [SerializeField] private Jefe jefe;

    private Camera cam;
    private Coroutine bucle;

    void Start()
    {
        cam = Camera.main;

        if (jefe != null)
        {
            jefe.onAparecer.AddListener(IniciarSpawn);
            jefe.onMuerte.AddListener(DetenerSpawn);
        }
    }

    public void IniciarSpawn()
    {
        if (bucle != null) return;
        bucle = StartCoroutine(BucleSpawn());
    }

    public void DetenerSpawn()
    {
        if (bucle != null)
        {
            StopCoroutine(bucle);
            bucle = null;
        }
    }

    IEnumerator BucleSpawn()
    {
        while (true)
        {
            float duracion = Random.Range(intervaloMin, intervaloMax);

            // Generar tiempos aleatorios dentro de la duracion
            float[] tiempos = new float[cantidadPorOleada];
            for (int i = 0; i < cantidadPorOleada; i++)
                tiempos[i] = Random.Range(0f, duracion);

            System.Array.Sort(tiempos);

            float tiempoAnterior = 0f;
            for (int i = 0; i < cantidadPorOleada; i++)
            {
                float espera = tiempos[i] - tiempoAnterior;
                if (espera > 0f) yield return new WaitForSeconds(espera);
                tiempoAnterior = tiempos[i];
                SpawnearAguila();
            }

            // Esperar el resto de la duracion
            float restante = duracion - tiempoAnterior;
            if (restante > 0f) yield return new WaitForSeconds(restante);
        }
    }

    void SpawnearAguila()
    {
        if (prefabAguila == null) return;

        float xCam = cam != null ? cam.transform.position.x : transform.position.x;
        float ySpawn = usarYFijo ? posicionYFija : Random.Range(yMin, yMax);

        Vector3 pos = new Vector3(xCam + offsetXAparicion, ySpawn, 0f);
        GameObject obj = Instantiate(prefabAguila, pos, Quaternion.identity);

        Aguila aguila = obj.GetComponent<Aguila>();
        if (aguila != null) aguila.Activar();
    }
}
