using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeSpawner : MonoBehaviour
{
    [Header("Prefabs de Slimes")]
    public GameObject prefabAgua;
    public GameObject prefabTierra;
    public GameObject prefabViento;
    public GameObject prefabFuego;

    [Header("Puntos de Spawn")]
    public Transform[] puntosAgua;
    public Transform[] puntosTierra;
    public Transform[] puntosEspeciales;

    public float tiempoSpawn = 3f;
    public int maxSlimes = 15;

    void Start() => InvokeRepeating("SpawnRandomSlime", 1f, tiempoSpawn);

    void SpawnRandomSlime()
    {
        if (GameObject.FindGameObjectsWithTag("Slime").Length >= maxSlimes) return;

        string escenaActual = SceneManager.GetActiveScene().name;
        Transform puntoElegido = null;
        GameObject prefabAElegir = null;

        if (escenaActual == "Main")
        {
            int azar = Random.Range(1, 101);
            if (azar <= 50 && puntosAgua.Length > 0)
            {
                puntoElegido = puntosAgua[Random.Range(0, puntosAgua.Length)];
                prefabAElegir = prefabAgua;
            }
            else if (puntosTierra.Length > 0)
            {
                puntoElegido = puntosTierra[Random.Range(0, puntosTierra.Length)];
                prefabAElegir = prefabTierra;
            }
        }
        else if (escenaActual == "Inferno")
        {
            if (puntosEspeciales.Length > 0)
            {
                puntoElegido = puntosEspeciales[Random.Range(0, puntosEspeciales.Length)];
                prefabAElegir = Random.value > 0.5f ? prefabViento : prefabFuego;
            }
        }

        if (puntoElegido != null && prefabAElegir != null)
        {
            Instantiate(prefabAElegir, puntoElegido.position, Quaternion.identity);
        }
    }
}