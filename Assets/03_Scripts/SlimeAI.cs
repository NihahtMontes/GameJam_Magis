using UnityEngine;

public enum TipoSlime { Agua, Tierra, Viento, Fuego }

public class SlimeAI : MonoBehaviour
{
    public TipoSlime tipo;
    public int resistenciaCaptura = 1;
    public float velocidad = 2f;
    public float distanciaDeteccion = 4f;

    [Header("Ataque")]
    public float danioAtaque = 10f;
    private float tiempoUltimoAtaque = 0f;
    private float cooldownAtaque = 1.5f;

    [Header("Referencia para el Corral")]
    public GameObject prefabCorralCorrespondiente; // Arrastra aquí el prefab que irá al corral

    private Transform jugador;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) jugador = playerObj.transform;
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia < distanciaDeteccion)
        {
            if (tipo == TipoSlime.Viento || tipo == TipoSlime.Fuego)
                Atacar(); // Raros/Super Raros atacan [cite: 204]
            else
                Huir(); // Comunes se escapan [cite: 204]
        }
    }

    public bool RecibirIntentoCaptura()
    {
        resistenciaCaptura--;
        if (resistenciaCaptura <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    void Huir()
    {
        Vector2 direccion = (transform.position - jugador.position).normalized;
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }

    void Atacar()
    {
        Vector2 direccion = (jugador.position - transform.position).normalized;
        transform.Translate(direccion * (velocidad * 1.5f) * Time.deltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (tipo == TipoSlime.Viento || tipo == TipoSlime.Fuego))
        {
            if (Time.time >= tiempoUltimoAtaque + cooldownAtaque)
            {
                tiempoUltimoAtaque = Time.time;
                PlayerController.vidaActual -= danioAtaque;
                
                if (PlayerController.vidaActual < 0) 
                    PlayerController.vidaActual = 0;
                
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.ActualizarInterfaz();
                    Debug.Log("¡El slime salvaje te atacó! Vida restante: " + PlayerController.vidaActual);
                }
            }
        }
    }
}