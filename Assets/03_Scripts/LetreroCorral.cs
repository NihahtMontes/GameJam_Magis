using UnityEngine;
using TMPro;

public class LetreroCorral : MonoBehaviour
{
    [Header("Configuración del Letrero")]
    public int nivelCorralAComprar = 2; // Pon 2 o 3 en el Inspector
    public int precioCorral = 500;
    public KeyCode teclaComprar = KeyCode.E;

    [Header("Opcional")]
    public TextMeshProUGUI textoInformacion;

    private bool jugadorCerca = false;

    void Start()
    {
        // Si ya tenemos el nivel necesario desde antes, esconder el letrero
        if (PlayerController.nivelCorral >= nivelCorralAComprar)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(teclaComprar))
        {
            IntentarComprarCorral();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = true;
            MostrarMensaje($"Presiona '{teclaComprar}' para comprar el Corral {nivelCorralAComprar} por ${precioCorral}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = false;
            MostrarMensaje("");
        }
    }

    private void IntentarComprarCorral()
    {
        if (PlayerController.nivelCorral >= nivelCorralAComprar)
        {
            MostrarMensaje("Ya posees este corral.");
            return;
        }

        if (PlayerController.dinero >= precioCorral)
        {
            PlayerController.dinero -= precioCorral;
            PlayerController.nivelCorral = nivelCorralAComprar;

            if (nivelCorralAComprar == 2) PlayerController.capacidadMaximaCorral = 20;
            if (nivelCorralAComprar == 3) PlayerController.capacidadMaximaCorral = 30;

            MostrarMensaje($"¡Corral {nivelCorralAComprar} Comprado!");

            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null) player.ActualizarInterfaz();

            // Esconder el letrero para que el jugador sepa que ya es suyo
            gameObject.SetActive(false);
        }
        else
        {
            MostrarMensaje("No tienes suficiente oro.");
        }
    }

    private void MostrarMensaje(string msj)
    {
        if (textoInformacion != null) textoInformacion.text = msj;
        if (!string.IsNullOrEmpty(msj)) Debug.Log("[LETRERO] " + msj);
    }
}
