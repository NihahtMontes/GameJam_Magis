using UnityEngine;
using UnityEngine.UI;

public class EstadoJugadorUI : MonoBehaviour
{
    public Slider sliderVida;
    public Slider sliderMana;

    public void RestaurarVida()
    {
        if (sliderVida != null)
        {
            sliderVida.value = sliderVida.maxValue;
            Debug.Log("[ESTADO JUGADOR] Vida restaurada al máximo.");
        }
        else
        {
            Debug.LogWarning("[ESTADO JUGADOR] sliderVida es NULL. Arrastra el objeto BarraVida (con componente Slider) al campo 'Slider Vida'.");
        }
    }

    public void RestaurarMana()
    {
        if (sliderMana != null)
        {
            sliderMana.value = sliderMana.maxValue;
            Debug.Log("[ESTADO JUGADOR] Mana restaurado al máximo.");
        }
        else
        {
            Debug.LogWarning("[ESTADO JUGADOR] sliderMana es NULL. Arrastra el objeto BarraMana (con componente Slider) al campo 'Slider Mana'.");
        }
    }
}
