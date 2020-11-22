using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Teletransportador : MonoBehaviour {
        
    private bool estaTeletransportandose;
    
    private void OnEnable()
    {
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoTeletransportarse), Transportar);
    }

    private void OnDisable()
    {
        ControladorEventos.Instancia.DesubscribirseEvento(typeof(EventoTeletransportarse), Transportar);
    }
    
    private void Transportar(EventoBase mensaje)
    {
        EventoTeletransportarse e = (EventoTeletransportarse)mensaje;
        if (!estaTeletransportandose)
        {
            ControladorDatos.Instancia.ReproducirSonido(SonidoID.SonidoEntrarPuerta);
            ControladorDatos.Instancia.AniadirCorrutinaACola(MostrarPantallaDeCarga(e));
        }
    }    

    private IEnumerator MostrarPantallaDeCarga(EventoTeletransportarse e)
    {
        estaTeletransportandose = true;
        yield return StartCoroutine(CorrutinasComunes.AlfaDeCeroAUno(UIControlador.Instancia.Teletransportador.pantallaDeCarga));
        Personaje.TeletransportarPersonaje.CambiarPosicion(e.Destino, Herramientas.ObtenerDireccion(e.DireccionMirar));
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(CorrutinasComunes.AlfaDeUnoACero(UIControlador.Instancia.Teletransportador.pantallaDeCarga));
        estaTeletransportandose = false;
    }

}
