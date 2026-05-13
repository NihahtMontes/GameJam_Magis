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
        panelOpciones.SetActive(true);
        panelComprar.SetActive(true);
        panelVender.SetActive(false);
    }

    public void AbrirVender()
    {
        if (!jugadorCerca) return;

        tiendaCerradaManual = false;
        panelOpciones.SetActive(true);
        panelComprar.SetActive(false);
        panelVender.SetActive(true);
    }

    public void CerrarComprar()
    {
        panelComprar.SetActive(false);
    }

    public void CerrarVender()
    {
        panelVender.SetActive(false);
    }

    public void SalirTienda()
    {
        tiendaCerradaManual = true;

        panelOpciones.SetActive(false);
        panelComprar.SetActive(false);
        panelVender.SetActive(false);
    }
}