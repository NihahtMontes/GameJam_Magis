using UnityEngine;

public class SlimeCorral : MonoBehaviour
{
    public float velocidad = 2f;
    private Vector2 direccion;
    private Rigidbody2D rb;

    public GameObject prefabCristal; // Arrastrar el prefab de Cristal que suelta
    public TipoSlime tipoSlime;
    
    private int cristalesSoltados = 0;
    public int maxCristales = 4;
    public float tiempoGeneracionCristal = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Congelamos la rotación para que el slime no ruede por el suelo
        rb.freezeRotation = true;
        CambiarDireccion();
        
        StartCoroutine(RutinaMovimiento());
        StartCoroutine(RutinaDropearCristales());
    }

    System.Collections.IEnumerator RutinaMovimiento()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            CambiarDireccion();
        }
    }

    System.Collections.IEnumerator RutinaDropearCristales()
    {
        while (cristalesSoltados < maxCristales)
        {
            yield return new WaitForSeconds(tiempoGeneracionCristal);
            DropearCristal();
        }
    }

    void DropearCristal()
    {
        if (prefabCristal != null)
        {
            GameObject cristal = Instantiate(prefabCristal, transform.position, Quaternion.identity);
            CristalDroplet droplet = cristal.GetComponent<CristalDroplet>();
            if (droplet != null)
            {
                droplet.tipo = tipoSlime;
            }
            cristalesSoltados++;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direccion * velocidad;
    }

    void CambiarDireccion()
    {
        // Elige una dirección al azar
        direccion = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    // Al chocar con el Box Collider del corral, cambia de dirección
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CambiarDireccion();
    }
}