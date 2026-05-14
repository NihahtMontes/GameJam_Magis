using UnityEngine;
using TMPro;

public class InventarioCristales : MonoBehaviour
{
    [Header("Stock de cristales")]
    public int cristalTierra = 6;
    public int cristalAgua = 5;
    public int cristalViento = 1;
    public int cristalFuego = 0;

    [Header("Textos UI del inventario")]
    public TextMeshProUGUI textoCristalTierra;
    public TextMeshProUGUI textoCristalAgua;
    public TextMeshProUGUI textoCristalViento;
    public TextMeshProUGUI textoCristalFuego;

    // Constantes para evitar typos
    public const string TIPO_TIERRA = "Tierra";
    public const string TIPO_AGUA = "Agua";
    public const string TIPO_VIENTO = "Viento";
    public const string TIPO_FUEGO = "Fuego";

    private void Start()
    {
        ActualizarTextos();
    }

    public bool QuitarCristal(string tipoCristal)
    {
        switch (tipoCristal)
        {
            case TIPO_TIERRA:
                if (cristalTierra > 0)
                {
                    cristalTierra--;
                    ActualizarTextos();
                    return true;
                }
                break;

            case TIPO_AGUA:
                if (cristalAgua > 0)
                {
                    cristalAgua--;
                    ActualizarTextos();
                    return true;
                }
                break;

            case TIPO_VIENTO:
                if (cristalViento > 0)
                {
                    cristalViento--;
                    ActualizarTextos();
                    return true;
                }
                break;

            case TIPO_FUEGO:
                if (cristalFuego > 0)
                {
                    cristalFuego--;
                    ActualizarTextos();
                    return true;
                }
                break;

            default:
                Debug.LogWarning("Tipo de cristal no reconocido: " + tipoCristal);
                break;
        }

        return false;
    }

    public void AgregarCristal(string tipoCristal, int cantidad)
    {
        switch (tipoCristal)
        {
            case TIPO_TIERRA:
                cristalTierra += cantidad;
                break;

            case TIPO_AGUA:
                cristalAgua += cantidad;
                break;

            case TIPO_VIENTO:
                cristalViento += cantidad;
                break;

            case TIPO_FUEGO:
                cristalFuego += cantidad;
                break;

            default:
                Debug.LogWarning("Tipo de cristal no reconocido: " + tipoCristal);
                return;
        }

        ActualizarTextos();
    }

    public void ActualizarTextos()
    {
        if (textoCristalTierra != null)
            textoCristalTierra.text = "x" + cristalTierra;

        if (textoCristalAgua != null)
            textoCristalAgua.text = "x" + cristalAgua;

        if (textoCristalViento != null)
            textoCristalViento.text = "x" + cristalViento;

        if (textoCristalFuego != null)
            textoCristalFuego.text = "x" + cristalFuego;
    }
}
