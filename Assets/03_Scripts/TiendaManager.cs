using UnityEngine;
using TMPro;

public class TiendaManager : MonoBehaviour
{
    [Header("UI Textos de Tienda")]
    public TextMeshProUGUI textoInformacion;

    [Header("Precios de Venta")]
    public int precioCristalComun = 10;
    public int precioCristalRaro = 50;

    [Header("Precios de Compra")]
    public int precioAumentoVida = 100;
    public int precioAumentoMana = 100;
    public int precioComidaMana = 20;
    public int precioCristalPortal = 200;
    public int precioCorralNivel2 = 500;
    public int precioCorralNivel3 = 1500;

    private bool jugadorCerca = false;

    void Update()
    {
        // Interacción rápida desde el mundo, o esto se puede vincular a botones de la UI
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            AbrirTienda();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            ActualizarInfo("Presiona 'E' para abrir tienda");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            ActualizarInfo("");
        }
    }

    public void AbrirTienda()
    {
        // Aquí podrías activar un Panel de UI de la Tienda
        // panelTienda.SetActive(true);
        ActualizarInfo("Tienda Abierta. Compra o Vende en el Menú.");
    }

    // ================= MÉTODOS PARA BOTONES DE UI ================= //

    public void VenderCristales()
    {
        int ganancia = 0;
        ganancia += PlayerController.cristalesAgua * precioCristalComun;
        ganancia += PlayerController.cristalesTierra * precioCristalComun;
        ganancia += PlayerController.cristalesViento * precioCristalRaro;
        ganancia += PlayerController.cristalesFuego * precioCristalRaro;

        if (ganancia > 0)
        {
            PlayerController.dinero += ganancia;
            
            PlayerController.cristalesAgua = 0;
            PlayerController.cristalesTierra = 0;
            PlayerController.cristalesViento = 0;
            PlayerController.cristalesFuego = 0;

            ActualizarInfo($"Vendiste cristales por ${ganancia}!");
            ActualizarUIJugador();
        }
        else
        {
            ActualizarInfo("No tienes cristales para vender.");
        }
    }

    public void ComprarAumentoVida()
    {
        if (PlayerController.dinero >= precioAumentoVida)
        {
            PlayerController.dinero -= precioAumentoVida;
            PlayerController.maxVida += 50f;
            PlayerController.vidaActual = PlayerController.maxVida; // Se cura al máximo
            ActualizarInfo("¡Aumento de Vida Comprado!");
            ActualizarUIJugador();
        }
        else
        {
            ActualizarInfo("Dinero insuficiente.");
        }
    }

    public void ComprarAumentoMana()
    {
        if (PlayerController.dinero >= precioAumentoMana)
        {
            PlayerController.dinero -= precioAumentoMana;
            PlayerController.maxMana += 50f;
            PlayerController.manaActual = PlayerController.maxMana;
            ActualizarInfo("¡Aumento de Maná Comprado!");
            ActualizarUIJugador();
        }
        else
        {
            ActualizarInfo("Dinero insuficiente.");
        }
    }

    public void ComprarComidaMana()
    {
        if (PlayerController.dinero >= precioComidaMana)
        {
            if (PlayerController.manaActual >= PlayerController.maxMana)
            {
                ActualizarInfo("El maná ya está al máximo.");
                return;
            }

            PlayerController.dinero -= precioComidaMana;
            PlayerController.manaActual = Mathf.Min(PlayerController.manaActual + 50f, PlayerController.maxMana);
            ActualizarInfo("¡Maná restaurado!");
            ActualizarUIJugador();
        }
        else
        {
            ActualizarInfo("Dinero insuficiente.");
        }
    }

    public void ComprarCristalPortal()
    {
        if (PlayerController.dinero >= precioCristalPortal)
        {
            PlayerController.dinero -= precioCristalPortal;
            PlayerController.cristalesPortal += 1;
            ActualizarInfo("¡Cristal de Portal Comprado!");
            ActualizarUIJugador();
        }
        else
        {
            ActualizarInfo("Dinero insuficiente.");
        }
    }

    public void ComprarSiguienteCorral()
    {
        if (PlayerController.nivelCorral == 1)
        {
            if (PlayerController.dinero >= precioCorralNivel2)
            {
                PlayerController.dinero -= precioCorralNivel2;
                PlayerController.nivelCorral = 2;
                PlayerController.capacidadMaximaCorral = 20;
                ActualizarInfo("¡Corral Nivel 2 Comprado (Capacidad: 20)!");
                ActualizarUIJugador();
            }
            else ActualizarInfo("Dinero insuficiente.");
        }
        else if (PlayerController.nivelCorral == 2)
        {
            if (PlayerController.dinero >= precioCorralNivel3)
            {
                PlayerController.dinero -= precioCorralNivel3;
                PlayerController.nivelCorral = 3;
                PlayerController.capacidadMaximaCorral = 30;
                ActualizarInfo("¡Corral Nivel 3 Comprado (Capacidad: 30)!");
                ActualizarUIJugador();
            }
            else ActualizarInfo("Dinero insuficiente.");
        }
        else
        {
            ActualizarInfo("Ya tienes el corral al máximo nivel.");
        }
    }

    private void ActualizarInfo(string mensaje)
    {
        if (textoInformacion != null)
        {
            textoInformacion.text = mensaje;
        }
        Debug.Log(mensaje);
    }

    private void ActualizarUIJugador()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.ActualizarInterfaz();
        }
    }
}
