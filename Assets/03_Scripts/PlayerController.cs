using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    // --- VARIABLES ESTÁTICAS PARA PERSISTENCIA ENTRE ESCENAS ---
    public static List<GameObject> inventarioSlimesPersistente = new List<GameObject>();
    public static int slimesEnCorralTotalEstatico = 0;
    
    // Corrales
    public static int nivelCorral = 1; // 1 = 10 slimes, 2 = 20 slimes, 3 = 30 slimes
    public static int capacidadMaximaCorral = 10;
    
    public static float maxVida = 100f;
    public static float vidaActual = 100f;

    public static float maxMana = 100f;
    public static float manaActual = 100f;
    
    public static int dinero = 0;
    
    // Inventario de cristales
    public static int cristalesAgua = 0;
    public static int cristalesTierra = 0;
    public static int cristalesFuego = 0;
    public static int cristalesViento = 0;
    public static int cristalesPortal = 0;
    // -------------------------------------------------------------
    [Header("Configuración de Movimiento")]
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private Vector2 movimiento;

    [Header("Configuración de Captura")]
    public float rangoCaptura = 1.5f;
    public float costeManaPorCaptura = 10f; // Maná que consume cada click
    public LayerMask capaSlimes;

    [Header("UI - Barras e Inventario")]
    public UnityEngine.UI.Slider barraVidaUI;
    public UnityEngine.UI.Slider barraManaUI;
    public TextMeshProUGUI textoInventario;
    public TextMeshProUGUI textoCorral;
    public TextMeshProUGUI textoDinero;
    public TextMeshProUGUI textoCristales; // Para mostrar la cantidad de cristales

    [Header("Referencias de Corral")]
    public Transform puntoAparicionCorral;


    [Header("Audio")]
    public AudioSource fuenteAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movimiento.normalized * velocidad * Time.fixedDeltaTime);
    }

    void IntentarAtacarCapturar()
    {
        if (manaActual < costeManaPorCaptura)
        {
            Debug.Log("¡No tienes suficiente maná para atacar!");
            return;
        }

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
                    // Consumir maná por el intento
                    manaActual -= costeManaPorCaptura;
                    ActualizarInterfaz();

                    if (slime.RecibirIntentoCaptura())
                    {
                        if (slime.prefabCorralCorrespondiente != null)
                        {
                            inventarioSlimesPersistente.Add(slime.prefabCorralCorrespondiente);
                        }
                        ActualizarInterfaz();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Corral") && inventarioSlimesPersistente.Count > 0)
        {
            SoltarSlimesEnCorral();
        }

        // Detección de cristales
        if (other.CompareTag("Cristal"))
        {
            CristalDroplet cristal = other.GetComponent<CristalDroplet>();
            if (cristal != null)
            {
                RecolectarCristal(cristal.tipo);
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Se removió lógica de portal de aquí
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

        if (slimesDepositados == 0 && inventarioSlimesPersistente.Count > 0)
        {
            Debug.Log("¡El corral está lleno! Necesitas comprar el siguiente nivel.");
        }

        ActualizarInterfaz();
    }

    public void ActualizarInterfaz()
    {
        if (textoInventario != null) textoInventario.text = "Slimes: " + inventarioSlimesPersistente.Count;
        if (textoCorral != null) textoCorral.text = "Corral: " + slimesEnCorralTotalEstatico;
        if (textoDinero != null) textoDinero.text = "$" + dinero;
        
        if (textoCristales != null) 
        {
            textoCristales.text = $"Cristales: Agua({cristalesAgua}) Tierra({cristalesTierra}) Fuego({cristalesFuego}) Viento({cristalesViento})";
        }

        if (barraVidaUI != null)
        {
            barraVidaUI.maxValue = maxVida;
            barraVidaUI.value = vidaActual;
        }

        if (barraManaUI != null)
        {
            barraManaUI.maxValue = maxMana;
            barraManaUI.value = manaActual;
        }
    }

    public void RecolectarCristal(TipoSlime tipo)
    {
        switch (tipo)
        {
            case TipoSlime.Agua: cristalesAgua++; break;
            case TipoSlime.Tierra: cristalesTierra++; break;
            case TipoSlime.Fuego: cristalesFuego++; break;
            case TipoSlime.Viento: cristalesViento++; break;
        }
        ActualizarInterfaz();
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