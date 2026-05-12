using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportPortal : MonoBehaviour
{
    [Header("Configuración de Destino")]
    [Tooltip("El nombre exacto de la escena a la que quieres ir")]
    public string nombreEscenaDestino = "Inferno";

    [Header("Interacción")]
    public KeyCode teclaInteractuar = KeyCode.F;

    [Header("Efectos (Opcional)")]
    public GameObject efectoVisual; // Por si quieres instanciar chispas al entrar

    [Header("Sonidos")]
    public AudioSource audioViaje;

    private bool jugadorEstaCerca = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos que sea el jugador
        if (collision.CompareTag("Player"))
        {
            jugadorEstaCerca = true;
            // Un mensaje en consola para confirmar que el trigger funciona
            Debug.Log($"[Portal] Presiona {teclaInteractuar} para entrar a {nombreEscenaDestino}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorEstaCerca = false;
        }
    }

    void Update()
    {
        // Detectar la tecla solo si el jugador está dentro del área
        if (jugadorEstaCerca && Input.GetKeyDown(teclaInteractuar))
        {
            ViajarAEscena();
            
        }
    }

    public void ViajarAEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscenaDestino))
        {
            // 1. Verificamos que el audio existe
            if (audioViaje != null && audioViaje.clip != null)
            {
                // 2. PlayClipAtPoint crea un objeto de audio en el mundo 
                // que NO depende de este portal. Sobrevive al cambio de escena.
                AudioSource.PlayClipAtPoint(audioViaje.clip, transform.position);

                Debug.Log("Sonido de teletransporte enviado al motor de audio.");
            }

            // 3. Cambiamos de escena inmediatamente
            Time.timeScale = 1f;
            SceneManager.LoadScene(nombreEscenaDestino);
        }
    }

   
}