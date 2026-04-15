using UnityEngine;
using System.Collections;

public class SpawnAguila : MonoBehaviour
{
    [Header("Prefabs a spawnear")]
    [SerializeField] private SpawnConfig[] oleadas;

    [Header("Opciones")]
    [SerializeField] private bool soloUnaVez = true;
    [SerializeField] private Camera camaraJuego;
    private bool yaActivado = false;

    [System.Serializable]
    public class SpawnConfig
    {
        public GameObject prefab;
        public int cantidad = 1;

        [Header("Tiempo de aparicion")]
        [Tooltip("Tiempo total en el que se distribuyen los spawns aleatoriamente")]
        public float duracionTotal = 10f;
        [Tooltip("Retraso antes de empezar a spawnear esta oleada")]
        public float retrasoInicial = 0f;

        [Header("Posicion X")]
        public float offsetXAparicion = -15f;

        [Header("Posicion Y")]
        public bool usarYFijo = false;
        public float posicionYFija = 170f;
        public float distanciaMinSuelo = 3f;
        public float distanciaMaxSuelo = 6f;
        public Transform referenciaSuelo;
    }

    void Start()
    {
        if (camaraJuego == null)
            camaraJuego = Camera.main;
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Player")) return;
        if (soloUnaVez && yaActivado) return;

        yaActivado = true;

        foreach (SpawnConfig config in oleadas)
        {
            StartCoroutine(SpawnearOleada(config));
        }
    }

    IEnumerator SpawnearOleada(SpawnConfig config)
    {
        if (config.prefab == null) yield break;

        // Retraso inicial para que no todas las oleadas empiecen al mismo tiempo
        if (config.retrasoInicial > 0f)
            yield return new WaitForSeconds(config.retrasoInicial);

        // Generar tiempos aleatorios dentro de la duracion total
        float[] tiempos = new float[config.cantidad];
        for (int i = 0; i < config.cantidad; i++)
        {
            tiempos[i] = Random.Range(0f, config.duracionTotal);
        }

        // Ordenar de menor a mayor
        System.Array.Sort(tiempos);

        float tiempoAnterior = 0f;
        for (int i = 0; i < config.cantidad; i++)
        {
            float espera = tiempos[i] - tiempoAnterior;
            if (espera > 0f)
                yield return new WaitForSeconds(espera);
            tiempoAnterior = tiempos[i];

            Spawnear(config);
        }
    }

    void Spawnear(SpawnConfig config)
    {
        float ySpawn;
        if (config.usarYFijo)
        {
            ySpawn = config.posicionYFija;
        }
        else
        {
            float ySuelo = config.referenciaSuelo != null ? config.referenciaSuelo.position.y : transform.position.y;
            ySpawn = ySuelo + Random.Range(config.distanciaMinSuelo, config.distanciaMaxSuelo);
        }

        // Usar posicion X de la camara en lugar del trigger
        float xCamara = camaraJuego != null ? camaraJuego.transform.position.x : transform.position.x;

        Vector3 posSpawn = new Vector3(
            xCamara + config.offsetXAparicion,
            ySpawn,
            0f
        );

        GameObject obj = Instantiate(config.prefab, posSpawn, Quaternion.identity);

        Aguila aguila = obj.GetComponent<Aguila>();
        if (aguila != null)
            aguila.Activar();

        Motociclista moto = obj.GetComponent<Motociclista>();
        if (moto != null)
            moto.Activar();

        Helicoptero heli = obj.GetComponent<Helicoptero>();
        if (heli != null)
            heli.Activar();

        EnemigoMilitar militar = obj.GetComponent<EnemigoMilitar>();
        if (militar != null)
            militar.Activar();
    }

    void OnDrawGizmosSelected()
    {
        if (oleadas == null) return;

        foreach (SpawnConfig config in oleadas)
        {
            float xSpawn = transform.position.x + config.offsetXAparicion;

            if (config.usarYFijo)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(new Vector3(xSpawn, config.posicionYFija, 0f), 0.5f);
            }
            else if (config.referenciaSuelo != null)
            {
                float ySuelo = config.referenciaSuelo.position.y;
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(new Vector3(xSpawn, ySuelo + config.distanciaMinSuelo, 0f),
                                new Vector3(xSpawn, ySuelo + config.distanciaMaxSuelo, 0f));
                Gizmos.DrawWireSphere(new Vector3(xSpawn, ySuelo + config.distanciaMinSuelo, 0f), 0.3f);
                Gizmos.DrawWireSphere(new Vector3(xSpawn, ySuelo + config.distanciaMaxSuelo, 0f), 0.3f);
            }
        }
    }
}
