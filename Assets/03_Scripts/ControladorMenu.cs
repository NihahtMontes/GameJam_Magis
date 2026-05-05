using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class ControladorMenu : MonoBehaviour
{
    [Header("Paneles Principales")]
    public GameObject panelDePausa;
    public GameObject panelDeAjustes;

    [Header("Componentes de Ajustes")]
    public TMPro.TMP_Dropdown dropdownResoluciones;
    public TMPro.TMP_Dropdown dropdownCalidad;
    public Toggle togglePantallaCompleta;
    public Slider sliderAudio;

    [Header("Configuración de Audio")]
    public AudioSource altavozSFX;
    public AudioClip sonidoClic;

    Resolution[] resolucionesDisponibles;

    [Header("Menú Desplegable")]
    public GameObject grupoOpciones;

    [Header("Sistema de Inventario")]
    public GameObject panelInventario;

    void Start()
    {
        CerrarTodo(); // Empezamos con la pantalla limpia
        SetupDropdownResoluciones();

        if (dropdownCalidad != null)
        {
            dropdownCalidad.value = QualitySettings.GetQualityLevel();
            dropdownCalidad.RefreshShownValue();
        }

        if (togglePantallaCompleta != null)
        {
            togglePantallaCompleta.isOn = Screen.fullScreen;
        }

        if (sliderAudio != null)
        {
            sliderAudio.value = AudioListener.volume;
        }
    }

    // --- NUEVA FUNCIÓN MAESTRA ---
    // Úsala para el botón de la esquina superior y para limpiar antes de abrir otros
    public void CerrarTodo()
    {
        if (panelDePausa != null) panelDePausa.SetActive(false);
        if (panelDeAjustes != null) panelDeAjustes.SetActive(false);
        if (panelInventario != null) panelInventario.SetActive(false);

        // El grupo de botones desplegables también se cierra
        if (grupoOpciones != null) grupoOpciones.SetActive(false);

        // Volvemos el tiempo a la normalidad
        Time.timeScale = 1f;
    }

    public void AlternarPausa()
    {
        if (panelDePausa != null)
        {
            if (panelDeAjustes.activeSelf)
            {
                AbrirPuntoPausaDesdeAjustes();
            }
            else
            {
                bool estadoActualPausa = !panelDePausa.activeSelf;

                // Si vamos a abrir pausa, cerramos inventario primero
                if (estadoActualPausa) CerrarTodo();

                panelDePausa.SetActive(estadoActualPausa);
                Time.timeScale = estadoActualPausa ? 0f : 1f;
            }
        }
    }

    public void AbrirPanelAjustes()
    {
        CerrarTodo(); // Limpiamos inventario u otros paneles
        if (panelDeAjustes != null) panelDeAjustes.SetActive(true);
        Time.timeScale = 0f; // Ajustes suele pausar el juego
    }

    public void AbrirPuntoPausaDesdeAjustes()
    {
        CerrarTodo();
        if (panelDePausa != null) panelDePausa.SetActive(true);
        Time.timeScale = 0f;
    }

    public void AbrirCerrarInventario()
    {
        if (panelInventario != null)
        {
            bool vaA_Abrirse = !panelInventario.activeSelf;

            if (vaA_Abrirse)
            {
                CerrarTodo(); // Cerramos Pausa o Ajustes antes de abrir Inventario
                panelInventario.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                CerrarTodo(); // Si ya estaba abierto, cerramos todo y reanudamos
            }

            ReproducirSonidoBoton();
        }
    }

    // --- Lógica de Ajustes ---
    public void ActualizarVolumenMaster(float valorSlider)
    {
        AudioListener.volume = valorSlider;
    }

    public void ReproducirSonidoBoton()
    {
        if (altavozSFX != null && sonidoClic != null)
        {
            altavozSFX.PlayOneShot(sonidoClic);
        }
    }

    public void CambiarCalidad(int indexCalidad)
    {
        QualitySettings.SetQualityLevel(indexCalidad);
    }

    public void CambiarPantallaCompleta(bool esPantallaCompleta)
    {
        Screen.fullScreen = esPantallaCompleta;
    }

    public void CambiarResolucion(int indexResolucion)
    {
        Resolution resolucion = resolucionesDisponibles[indexResolucion];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
    }

    void SetupDropdownResoluciones()
    {
        if (dropdownResoluciones == null) return;
        resolucionesDisponibles = Screen.resolutions;
        dropdownResoluciones.ClearOptions();
        List<string> opciones = new List<string>();
        int indexActualResolucion = 0;

        for (int i = 0; i < resolucionesDisponibles.Length; i++)
        {
            string opcion = resolucionesDisponibles[i].width + " x " + resolucionesDisponibles[i].height;
            opciones.Add(opcion);
            if (resolucionesDisponibles[i].width == Screen.currentResolution.width &&
                resolucionesDisponibles[i].height == Screen.currentResolution.height)
            {
                indexActualResolucion = i;
            }
        }
        dropdownResoluciones.AddOptions(opciones);
        dropdownResoluciones.value = indexActualResolucion;
        dropdownResoluciones.RefreshShownValue();
    }

    public void ToggleMenuDesplegable()
    {
        if (grupoOpciones != null)
        {
            bool estadoNuevo = !grupoOpciones.activeSelf;
            grupoOpciones.SetActive(estadoNuevo);
            ReproducirSonidoBoton();
        }
    }

// --- Funciones de Escenas (Las que ya tenías) ---
    public void IrAlJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }

    public void SalirDelJuego()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}