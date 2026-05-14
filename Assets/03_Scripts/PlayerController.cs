using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    // --- VARIABLES ESTÁTICAS PARA PERSISTENCIA ENTRE ESCENAS ---
    public static List<GameObject> inventarioSlimesPersistente = new List<GameObject>();
    public static int slimesEnCorralTotalEstatico = 0;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetearProgresoAlJugar()
    {
        inventarioSlimesPersistente.Clear();
        slimesEnCorralTotalEstatico = 0;
        nivelCorral = 1;
        capacidadMaximaCorral = 10;
        vidaActual = maxVida;
        manaActual = maxMana;
        dinero = 1000;
        cristalesAgua = 1;
        cristalesTierra = 1;
        cristalesFuego = 1;
        cristalesViento = 1;
        bifrostCount = 1;
    }
    
    // Corrales
    public static int nivelCorral = 1; // 1 = 10 slimes, 2 = 20 slimes, 3 = 30 slimes
    public static int capacidadMaximaCorral = 10;
    
    public static float maxVida = 100f;
    public static float vidaActual = 100f;

    public static float maxMana = 100f;
    public static float manaActual = 100f;
    
    public static int dinero = 1000;
    
    // Inventario de cristales y objetos (Empezando en 1 para pruebas)
    public static int cristalesAgua = 1;
    public static int cristalesTierra = 1;
    public static int cristalesFuego = 1;
    public static int cristalesViento = 1;
    public static int bifrostCount = 1;
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
    
    [Header("Textos de Cristales y Objetos")]
    public TextMeshProUGUI textoCristalTierra;
    public TextMeshProUGUI textoCristalAgua;
    public TextMeshProUGUI textoCristalViento;
    public TextMeshProUGUI textoCristalFuego;
    public TextMeshProUGUI textoBifrost;

    [Header("Referencias de Corral")]
    [Tooltip("Puntos de aparición: 0 para Corral 1, 1 para Corral 2, 2 para Corral 3")]
    public Transform[] puntosAparicionCorrales;


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
        if (other.CompareTag("Corral"))
        {
            if (inventarioSlimesPersistente.Count > 0)
            {
                SoltarSlimesEnCorral();
            }
            // ¡Recolectar automáticamente los cristales de los corrales!
            RecolectarCristalesEnGranja();
        }

        // Detección de cristales (por si pisas uno salvaje)
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

    void RecolectarCristalesEnGranja()
    {
        GameObject[] cristalesEscena = GameObject.FindGameObjectsWithTag("Cristal");
        int recolectados = 0;

        foreach (GameObject c in cristalesEscena)
        {
            CristalDroplet droplet = c.GetComponent<CristalDroplet>();
            if (droplet != null)
            {
                RecolectarCristal(droplet.tipo);
                Destroy(c);
                recolectados++;
            }
        }

        if (recolectados > 0)
        {
            Debug.Log("¡Recolectaste " + recolectados + " cristales de tus corrales mágicamente!");
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
                Transform puntoSpawn = ObtenerPuntoSpawnCorral();
                if (puntoSpawn != null)
                {
                    Instantiate(prefab, puntoSpawn.position, Quaternion.identity);
                    slimesEnCorralTotalEstatico++;
                    slimesDepositados++;
                }
                else
                {
                    // Fallback de seguridad
                    slimesSobrantes.Add(prefab);
                }
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

    Transform ObtenerPuntoSpawnCorral()
    {
        if (puntosAparicionCorrales == null || puntosAparicionCorrales.Length == 0) return transform; // Fallback al jugador

        if (slimesEnCorralTotalEstatico < 10 && puntosAparicionCorrales.Length > 0)
            return puntosAparicionCorrales[0];
        if (slimesEnCorralTotalEstatico < 20 && puntosAparicionCorrales.Length > 1)
            return puntosAparicionCorrales[1];
        if (puntosAparicionCorrales.Length > 2)
            return puntosAparicionCorrales[2];
            
        return puntosAparicionCorrales[0]; // Fallback al primero
    }

    public void ActualizarInterfaz()
    {
        if (textoInventario != null) textoInventario.text = "Slimes: " + inventarioSlimesPersistente.Count;
        if (textoCorral != null) textoCorral.text = "Corral: " + slimesEnCorralTotalEstatico;
        if (textoDinero != null) textoDinero.text = dinero.ToString();
        
        if (textoCristalTierra != null) textoCristalTierra.text = "x" + cristalesTierra;
        if (textoCristalAgua != null) textoCristalAgua.text = "x" + cristalesAgua;
        if (textoCristalViento != null) textoCristalViento.text = "x" + cristalesViento;
        if (textoCristalFuego != null) textoCristalFuego.text = "x" + cristalesFuego;
        if (textoBifrost != null) textoBifrost.text = bifrostCount.ToString();

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
