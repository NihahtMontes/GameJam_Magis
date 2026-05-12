using UnityEngine;

public class CamaraSigue : MonoBehaviour
{
    [Header("Configuración")]
    public Transform objetivo; // Aquí arrastra a tu Player en el Inspector
    public float suavizado = 0.125f; // Qué tan rápido sigue al jugador (menor es más suave)
    public Vector3 desfase = new Vector3(0, 0, -10); // Mantiene la cámara a distancia del mapa

    void LateUpdate()
    {
        if (objetivo != null)
        {
            // Calcula la posición deseada
            Vector3 posicionDeseada = objetivo.position + desfase;

            // Interpola entre la posición actual y la deseada para que sea fluido
            Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, suavizado);

            // Aplica la posición
            transform.position = posicionSuavizada;
        }
    }
}