using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public static List<GameObject> inventarioSlimesPersistente = new List<GameObject>();
    public static int slimesEnCorralTotalEstatico = 0;
    public static int nivelCorral = 1;
    public static int capacidadMaximaCorral = 10;

    [Header("Configuración de Movimiento")]
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private Vector2 movimiento;

    [Header("Configuración de Captura")]
    public float rangoCaptura = 1.5f;
    public LayerMask capaSlimes;

    [Header("Inventario y UI")]
    public List<GameObject> inventarioSlimes = new List<GameObject>();
    public int slimesEnCorralTotal = 0;
    public TextMeshProUGUI textoInventario;
    public TextMeshProUGUI textoCorral;
    public JugadorDinero jugadorDinero;
    public InventarioCristales inventarioCristales;

    [Header("Referencias de Corral")]
    public Transform puntoAparicionCorral;

    [Header("Sistema de Portal")]
    private bool estaEnPortal = false;
    private string nombreEscenaDestino;

    [Header("Audio")]
    public AudioSource fuenteAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inventarioSlimes = inventarioSlimesPersistente;
        slimesEnCorralTotal = slimesEnCorralTotalEstatico;

        if (fuenteAudio == null)
        {
            fuenteAudio = GetComponent<AudioSource>();
        }

        ActualizarInterfaz();
    }

    void Update()
    {
        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento.y = Input.GetAxisRaw("Vertical");

        GestionarSonidoCaminata();

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
                    if (slime.RecibirIntentoCaptura())
                    {
                        if (slime.prefabCorralCorrespondiente != null)
                        {
                            inventarioSlimesPersistente.Add(slime.prefabCorralCorrespondiente);
                            inventarioSlimes = inventarioSlimesPersistente;
                        }

                        ActualizarInterfaz();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Corral") && inventarioSlimes.Count > 0)
        {
            SoltarSlimesEnCorral();
        }

        if (other.CompareTag("Cristal"))
        {
            CristalDroplet cristal = other.GetComponent<CristalDroplet>();

            if (cristal != null)
            {
                RecolectarCristal(cristal.tipo);
                Destroy(other.gameObject);
            }
        }

        if (other.CompareTag("Portal"))
        {
            estaEnPortal = true;

            TeleportPortal portal = other.GetComponent<TeleportPortal>();

            if (portal != null)
            {
                nombreEscenaDestino = portal.nombreEscenaDestino;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            estaEnPortal = false;
            nombreEscenaDestino = "";
        }
    }

    void SoltarSlimesEnCorral()
    {
        int slimesDepositados = 0;
        List<GameObject> slimesSobrantes = new List<GameObject>();

        foreach (GameObject prefab in inventarioSlimesPersistente)
        {
            if (slimesEnCorralTotalEstatico < capacidadMaximaCorral)
            {
                Instantiate(prefab, puntoAparicionCorral.position, Quaternion.identity);
                slimesEnCorralTotalEstatico++;
                slimesDepositados++;
            }
            else
            {
                slimesSobrantes.Add(prefab);
            }
        }

        inventarioSlimesPersistente = slimesSobrantes;
        inventarioSlimes = inventarioSlimesPersistente;
        slimesEnCorralTotal = slimesEnCorralTotalEstatico;

        if (slimesDepositados == 0 && inventarioSlimesPersistente.Count > 0)
        {
            Debug.Log("El corral está lleno. Necesitas comprar el siguiente nivel.");
        }

        ActualizarInterfaz();
    }

    void ActualizarInterfaz()
    {
        if (textoInventario != null)
        {
            textoInventario.text = "Slimes: " + inventarioSlimesPersistente.Count;
        }

        if (textoCorral != null)
        {
            textoCorral.text = "Corral: " + slimesEnCorralTotalEstatico;
        }
    }

    public void RecolectarCristal(TipoSlime tipo)
    {
        if (inventarioCristales == null)
        {
            inventarioCristales = GetComponent<InventarioCristales>();
        }

        if (inventarioCristales == null)
        {
            Debug.LogWarning("No se asignó InventarioCristales en PlayerController.");
            return;
        }

        switch (tipo)
        {
            case TipoSlime.Agua:
                inventarioCristales.AgregarCristal(InventarioCristales.TIPO_AGUA, 1);
                break;
            case TipoSlime.Tierra:
                inventarioCristales.AgregarCristal(InventarioCristales.TIPO_TIERRA, 1);
                break;
            case TipoSlime.Viento:
                inventarioCristales.AgregarCristal(InventarioCristales.TIPO_VIENTO, 1);
                break;
            case TipoSlime.Fuego:
                inventarioCristales.AgregarCristal(InventarioCristales.TIPO_FUEGO, 1);
                break;
        }
    }

    private void GestionarSonidoCaminata()
    {
        bool seEstaMoviendo = movimiento.sqrMagnitude > 0.01f;

        if (fuenteAudio == null) return;

        if (seEstaMoviendo)
        {
            if (!fuenteAudio.isPlaying)
            {
                fuenteAudio.loop = true;
                fuenteAudio.Play();
            }
        }
        else
        {
            if (fuenteAudio.isPlaying)
            {
                fuenteAudio.Stop();
            }
        }
    }
}
