using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private Vector2 movimiento;

    [Header("Configuración de Captura")]
    public float rangoCaptura = 1.5f;
    public LayerMask capaSlimes;

    [Header("Sistema de Portal")]
    private bool estaEnPortal = false;
    private string nombreEscenaDestino;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento.y = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            IntentarAtacarCapturar();
        }

        if (estaEnPortal && Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movimiento.normalized * velocidad * Time.fixedDeltaTime);
    }

    void IntentarAtacarCapturar()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0f, capaSlimes);

        if (hit.collider != null && hit.collider.CompareTag("Slime"))
        {
            float distancia = Vector2.Distance(transform.position, hit.collider.transform.position);

            if (distancia <= rangoCaptura)
            {
                SlimeAI slime = hit.collider.GetComponent<SlimeAI>();
                if (slime != null)
                {
                    slime.RecibirIntentoCaptura();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            estaEnPortal = true;
            TeleportPortal portal = other.GetComponent<TeleportPortal>();
            if (portal != null) nombreEscenaDestino = portal.nombreEscenaDestino;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            estaEnPortal = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoCaptura);
    }
}