using UnityEngine;
using TMPro;

public class InventarioItems : MonoBehaviour
{
    [Header("Stock de items comprables")]
    public int bifrost = 0;

    [Header("Textos UI del inventario")]
    public TextMeshProUGUI textoBifrost;

    private void Start()
    {
        ActualizarTexto();
    }

    public void AgregarBifrost(int cantidad)
    {
        bifrost += cantidad;
        ActualizarTexto();
    }

    public void ActualizarTexto()
    {
        if (textoBifrost != null)
        {
            textoBifrost.text = bifrost.ToString();
        }
    }
}
