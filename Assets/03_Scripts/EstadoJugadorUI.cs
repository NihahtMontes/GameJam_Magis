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
        }
    }

    public void RestaurarMana()
    {
        if (sliderMana != null)
        {
            sliderMana.value = sliderMana.maxValue;
        }
    }
}
