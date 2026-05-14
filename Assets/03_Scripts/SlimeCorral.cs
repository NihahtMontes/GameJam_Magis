using UnityEngine;

public class SlimeCorral : MonoBehaviour
{
    public float velocidad = 2f;
    private Vector2 direccion;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Congelamos la rotación para que el slime no ruede por el suelo
        rb.freezeRotation = true;
        CambiarDireccion();
        // Cambia de dirección cada 2 a 4 segundos
        InvokeRepeating("CambiarDireccion", 2f, Random.Range(2f, 4f));
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