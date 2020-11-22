using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVentanaDialogo : MonoBehaviour
{
    private GameObject ventanaDialogo;
    private UIVentanaConfirmacion ventanConfirmacion;
    private Image ventanaPokemonPreview;
    private Text campoDeTexto;
    private GameObject iconoContinuarDialogo;    

    private int textoPosicionActual;
    private int textoPosicionFinal;

    private void OnEnable()
    {        
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoMostrarMensaje), MostrarMensaje);
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoMostrarMensajeConversacion), MostrarConversacion);
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoMostrarVentanaConfirmacion), MostrarVentanaConfirmacion);
    }

    private void OnDisable()
    {
        ControladorEventos.Instancia.DesubscribirseEvento(typeof(EventoMostrarMensaje), MostrarMensaje);
        ControladorEventos.Instancia.DesubscribirseEvento(typeof(EventoMostrarMensajeConversacion), MostrarConversacion);
        ControladorEventos.Instancia.DesubscribirseEvento(typeof(EventoMostrarVentanaConfirmacion), MostrarVentanaConfirmacion);
    }

    private void Start()
    {
        ventanaDialogo = UIControlador.Instancia.Dialogo.ventanaDialogo;
        ventanConfirmacion = UIControlador.Instancia.Dialogo.ventanConfirmacion;
        ventanaPokemonPreview = UIControlador.Instancia.Dialogo.ventanaPokemonPreview;
        campoDeTexto = UIControlador.Instancia.Dialogo.campoDeTexto;
        iconoContinuarDialogo = UIControlador.Instancia.Dialogo.iconoContinuarDialogo;
    }

    private void MostrarMensaje(EventoBase mensaje)
    {
        EventoMostrarMensaje evento = (EventoMostrarMensaje)mensaje;
        ControladorDatos.Instancia.AniadirCorrutinaACola(MostrarTextoCorrutina(evento.Mensaje, true));
    }

    private void MostrarConversacion(EventoBase e)
    {
        EventoMostrarMensajeConversacion evento = ((EventoMostrarMensajeConversacion)e);
        if (!string.IsNullOrEmpty(evento.Conversacion.texto))
        {
            ControladorDatos.Instancia.ReproducirSonido(SonidoID.SonidoHablarNPC);
            ControladorDatos.Instancia.AniadirCorrutinaACola(MostrarMensajeConversacion(evento));
        }
    }

    public IEnumerator MostrarMensajeConversacion(EventoMostrarMensajeConversacion evento, bool conversacionTrasCombate = false)
    {
        string mensaje = evento.Conversacion.texto;
        if(conversacionTrasCombate)
            mensaje = evento.Conversacion.texto2;

        yield return StartCoroutine(MostrarTextoCorrutina(mensaje, false));

        if (evento.Conversacion.darItemPorTerminarConversacion != ItemID.NINGUNO && evento.Conversacion.cantidadDeItems > 0)
        {
            string nombreItem = ControladorDatos.Instancia.ObtenerItem(evento.Conversacion.darItemPorTerminarConversacion).nombre;
            string texto = Ajustes.Instancia.textoCuandoConsigueItem
                .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, nombreItem)
                .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, evento.Conversacion.cantidadDeItems.ToString());
            ControladorDatos.Instancia.Datos.AniadirItemEncontradoAlInventario(evento.Conversacion.darItemPorTerminarConversacion, evento.Conversacion.cantidadDeItems);
            yield return StartCoroutine(MostrarTextoCorrutina(texto, false));
        }        
        
        FinalizarDialogo();

        if (evento.Conversacion.darLogroPorTerminarConversacion != Logro.NINGUNO)
        {
            ControladorDatos.Instancia.Datos.AniadirLogro(evento.Conversacion.darLogroPorTerminarConversacion);
        }
    }
    
    

    public IEnumerator MostrarTextoCorrutina(string texto, bool desactivarVentanaAlTerminar, bool leerTecladoEnUltimaVentana = true,
        float tiempoEsperaUltimaVentanaSiNoSeLeeTeclado = Ajustes.tiempoDeEsperaUltimaVentanaDialogoSiNoSeLeeElTeclado)
    {
        ActivarVentana();
        campoDeTexto.text = string.Empty;
        textoPosicionActual = 0;
        texto = texto.Replace("\r", " ").Replace("\n", " ");

        while (texto.Length > 0)
        {
            //Devuelve la posición actual a 0 ya que se han eliminado los textos mostrados
            textoPosicionActual = 0;
            campoDeTexto.text = string.Empty;

            //Comprobamos posición de la última palabra que se admite en el máximo de letras por ventana
            if (texto.Length > Ajustes.Instancia.maximasLetrasPorVentanaDeDialogo)
                textoPosicionFinal = texto.Substring(0, Ajustes.Instancia.maximasLetrasPorVentanaDeDialogo).LastIndexOf(" ");
            else
                textoPosicionFinal = texto.Length - 1;

            if (Ajustes.Instancia.velocidadMensajeDeDialogo > 0)
            {
                //Mientras la posición actual no haya llegado a la final sigue imprimiendo letra a letra
                while (textoPosicionActual <= textoPosicionFinal)
                {
                    textoPosicionActual++;
                    campoDeTexto.text = texto.Substring(0, textoPosicionActual);
                    yield return new WaitForSeconds(Ajustes.Instancia.velocidadMensajeDeDialogo);
                }
            }
            else
            {
                campoDeTexto.text = texto.Substring(0, textoPosicionFinal);
                textoPosicionActual = textoPosicionFinal + 1;
            }

            //Elimina las letras ya mostradas del mensaje
            texto = texto.Remove(0, textoPosicionActual);


            //Espera a que el jugador pulse la tecla para continuar, ya que hemos llenado una ventana
            if (texto.Length > 0 || leerTecladoEnUltimaVentana)
            {
                iconoContinuarDialogo.SetActive(true);
                while (!Input.GetKeyDown(Ajustes.Instancia.teclaInteractuar) && !Input.GetMouseButtonDown(0))
                {
                    yield return null;
                }
                ControladorDatos.Instancia.ReproducirSonido(SonidoID.SonidoHablarNPC);
                iconoContinuarDialogo.SetActive(false);
            }
            else if (!leerTecladoEnUltimaVentana)
            {
                yield return new WaitForSeconds(tiempoEsperaUltimaVentanaSiNoSeLeeTeclado);
            }


            yield return null;
        }

        if (desactivarVentanaAlTerminar)
        {
            ventanaDialogo.gameObject.SetActive(false);
        }
    }
    
    private void ActivarVentana()
    {
        if (!ventanaDialogo.gameObject.activeSelf)
        {
            ventanaDialogo.gameObject.SetActive(true);
        }        
    }

    private void FinalizarDialogo()
    {
        ventanaDialogo.gameObject.SetActive(false);
    }

    private void MostrarVentanaConfirmacion(EventoBase mensaje)
    {
        EventoMostrarVentanaConfirmacion e = (EventoMostrarVentanaConfirmacion)mensaje;
        if (e.EncolarCorrutina)
            ControladorDatos.Instancia.AniadirCorrutinaACola(MostrarVentanaConfirmacion(e));
        else
            StartCoroutine(MostrarVentanaConfirmacion(e));
    }

    private IEnumerator MostrarVentanaConfirmacion(EventoMostrarVentanaConfirmacion e)
    {
        if(e.PokemonPreview != PokemonID.NINGUNO)
        {
            ventanaPokemonPreview.gameObject.SetActive(true);
            ventanaPokemonPreview.sprite = ControladorDatos.Instancia.ObtenerPokemon(e.PokemonPreview).sprite;
        }
        ventanConfirmacion.MostrarVentana();
        yield return StartCoroutine(MostrarTextoCorrutina(e.MensajeConfirmacion, false, false, 0f));
        while (ventanConfirmacion.UltimaEleccion == Eleccion.EnEspera)
            yield return null;
        ventanaPokemonPreview.gameObject.SetActive(false);
        if (ventanConfirmacion.UltimaEleccion == Eleccion.Si)
        {
            if(!string.IsNullOrEmpty(e.MensajeTrasAceptar))
                yield return StartCoroutine(MostrarTextoCorrutina(e.MensajeTrasAceptar, true));
            if (e.AccionConfirmar != null)
                e.AccionConfirmar();
        }
        else if (ventanConfirmacion.UltimaEleccion == Eleccion.No)
        {
            if(!string.IsNullOrEmpty(e.MensajeTrasRechazar))
                yield return StartCoroutine(MostrarTextoCorrutina(e.MensajeTrasRechazar, true));
            if(e.AccionDenegar != null)
                e.AccionDenegar();
        }
        FinalizarDialogo();
    }

   
}
