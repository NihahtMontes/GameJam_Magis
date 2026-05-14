using UnityEngine;
using TMPro;

public class JugadorDinero : MonoBehaviour
{
    [Header("Oro del jugador")]
    public int oroActual = 1000;

    [Header("UI")]
    public TextMeshProUGUI textoOro;

    private void Start()
    {
        ActualizarTextoOro();
    }

    public bool TieneOroSuficiente(int cantidad)
    {
        return oroActual >= cantidad;
    }

    public void RestarOro(int cantidad)
    {
        oroActual -= cantidad;

        if (oroActual < 0)
        {
            oroActual = 0;
        }

        ActualizarTextoOro();
    }

    public void SumarOro(int cantidad)
    {
        oroActual += cantidad;
        ActualizarTextoOro();
    }

    private void ActualizarTextoOro()
    {
        if (textoOro != null)
        {
            textoOro.text = oroActual.ToString();
        }
    }
}
