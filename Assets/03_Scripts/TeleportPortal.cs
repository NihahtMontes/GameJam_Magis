using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class TeleportPortal : MonoBehaviour
{
    [Header("Configuración del Portal")]
    public string nombreEscenaDestino = "Inferno"; // El nombre debe ser exacto a tu escena
    public KeyCode teclaInteractuar = KeyCode.F; // La tecla para viajar

    private bool jugadorEstaCerca = false;

    // Se activa cuando el jugador entra en el área del portal
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorEstaCerca = true;
            Debug.Log("Jugador cerca del portal. Presiona " + teclaInteractuar + " para viajar.");
        }
    }

    // Se activa cuando el jugador sale del área del portal
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorEstaCerca = false;
        }
    }

    void Update()
    {
        // Si el jugador está cerca y presiona la tecla F
        if (jugadorEstaCerca && Input.GetKeyDown(teclaInteractuar))
        {
            ViajarAEscena();
        }
    }

    public void ViajarAEscena()
    {
        // Asegúrate de que el tiempo no esté pausado (por si vienes del menú)
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaDestino);
    }
}