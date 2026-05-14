using UnityEngine;

public class CristalDroplet : MonoBehaviour
{
    public TipoSlime tipo;
    public float velocidadFlotacion = 2f;
    public float alturaFlotacion = 0.2f;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        transform.position = posicionInicial + new Vector3(0, Mathf.Sin(Time.time * velocidadFlotacion) * alturaFlotacion, 0);
    }
}
