using UnityEngine;

public class CabanaTienda : MonoBehaviour
{
    [Header("Paneles UI")]
    public GameObject panelOpciones;
    public GameObject panelComprar;
    public GameObject panelVender;

    private bool jugadorCerca = false;
    private bool tiendaCerradaManual = false;

    void Start()
    {
        panelOpciones.SetActive(false);
        panelComprar.SetActive(false);
        panelVender.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = true;

            if (!tiendaCerradaManual)
            {
                panelOpciones.SetActive(true);
                panelComprar.SetActive(false);
                panelVender.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = false;
            tiendaCerradaManual = false;

            panelOpciones.SetActive(false);
            panelComprar.SetActive(false);
            panelVender.SetActive(false);
        }
    }

    public void AbrirComprar()
    {
        if (!jugadorCerca) return;

        tiendaCerradaManual = false;

        panelOpciones.SetActive(false); // se cierra el panel de opciones
        panelComprar.SetActive(true);   // se abre comprar
        panelVender.SetActive(false);   // se cierra vender
    }

    public void AbrirVender()
    {
        if (!jugadorCerca) return;

        tiendaCerradaManual = false;

        panelOpciones.SetActive(false); // se cierra el panel de opciones
        panelComprar.SetActive(false);  // se cierra comprar
        panelVender.SetActive(true);    // se abre vender
    }

    public void CerrarComprar()
    {
        panelComprar.SetActive(false);

        if (jugadorCerca && !tiendaCerradaManual)
        {
            panelOpciones.SetActive(true);
        }
    }

    public void CerrarVender()
    {
        panelVender.SetActive(false);

        if (jugadorCerca && !tiendaCerradaManual)
        {
            panelOpciones.SetActive(true);
        }
    }

    public void SalirTienda()
    {
        tiendaCerradaManual = true;

        panelOpciones.SetActive(false);
        panelComprar.SetActive(false);
        panelVender.SetActive(false);
    }

    // ==========================
    // SISTEMA DE ORO Y CRISTALES
    // ==========================

    [Header("Sistema de Oro")]
    public JugadorDinero jugadorDinero;

    [Header("Sistema de Cristales")]
    public InventarioCristales inventarioCristales;

    [Header("Precios de compra")]
    public int precioBifrost = 300;
    public int precioVida = 150;
    public int precioMana = 120;

    [Header("Precios de venta de cristales")]
    public int ventaCristalTierra = 50;
    public int ventaCristalAgua = 70;
    public int ventaCristalViento = 90;
    public int ventaCristalFuego = 120;

    // ==========================
    // COMPRAR
    // ==========================

    public void ComprarBifrost()
    {
        ComprarItem("Bifrost", precioBifrost);
    }

    public void ComprarVida()
    {
        ComprarItem("Vida", precioVida);
    }

    public void ComprarMana()
    {
        ComprarItem("Mana", precioMana);
    }

    private void ComprarItem(string nombreItem, int precio)
    {
        if (jugadorDinero == null)
        {
            Debug.LogWarning("No se asignó JugadorDinero en CabanaTienda.");
            return;
        }

        if (jugadorDinero.TieneOroSuficiente(precio))
        {
            jugadorDinero.RestarOro(precio);
            Debug.Log("Compraste " + nombreItem + " por " + precio + " oro.");
        }
        else
        {
            Debug.Log("No tienes suficiente oro para comprar " + nombreItem + ". Necesitas " + precio + " oro.");
        }
    }

    // ==========================
    // VENDER CRISTALES
    // ==========================

    public void VenderCristalTierra()
    {
        VenderCristal(InventarioCristales.TIPO_TIERRA, ventaCristalTierra);
    }

    public void VenderCristalAgua()
    {
        VenderCristal(InventarioCristales.TIPO_AGUA, ventaCristalAgua);
    }

    public void VenderCristalViento()
    {
        VenderCristal(InventarioCristales.TIPO_VIENTO, ventaCristalViento);
    }

    public void VenderCristalFuego()
    {
        VenderCristal(InventarioCristales.TIPO_FUEGO, ventaCristalFuego);
    }

    private void VenderCristal(string tipoCristal, int precioVenta)
    {
        if (jugadorDinero == null)
        {
            Debug.LogWarning("No se asignó JugadorDinero en CabanaTienda.");
            return;
        }

        if (inventarioCristales == null)
        {
            Debug.LogWarning("No se asignó InventarioCristales en CabanaTienda.");
            return;
        }

        if (inventarioCristales.QuitarCristal(tipoCristal))
        {
            jugadorDinero.SumarOro(precioVenta);
            Debug.Log("Vendiste Cristal " + tipoCristal + " por " + precioVenta + " oro.");
        }
        else
        {
            Debug.Log("No tienes Cristal " + tipoCristal + " para vender.");
        }
    }
}