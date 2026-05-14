using UnityEngine;

public class CristalDroplet : MonoBehaviour
{
    public TipoSlime tipo;
    public float velocidadFlotacion = 2f;
    public float alturaFlotacion = 0.2f;

    private Vector3 posInicial;

    void Start()
    {
        posInicial = transform.position;
    }

    void Update()
    {
        // Animación de flotar arriba y abajo
        transform.position = posInicial + new Vector3(0, Mathf.Sin(Time.time * velocidadFlotacion) * alturaFlotacion, 0);
    }
}
