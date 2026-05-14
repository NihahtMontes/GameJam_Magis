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
}