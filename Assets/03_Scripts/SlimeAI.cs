using UnityEngine;

public enum TipoSlime { Agua, Tierra, Viento, Fuego }

public class SlimeAI : MonoBehaviour
{
    public TipoSlime tipo;
    public int resistenciaCaptura = 3;
    public float velocidad = 2f;
    public float distanciaDeteccion = 4f;
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
                Atacar();
            else
                Huir();
        }
    }

    public void RecibirIntentoCaptura()
    {
        resistenciaCaptura--;
        if (resistenciaCaptura <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Slime Capturado!");
        }
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
}