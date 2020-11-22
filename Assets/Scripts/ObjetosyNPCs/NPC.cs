using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPC : IInteractivo
{
    [HideInInspector]
    public List<Conversacion> listaConversaciones;
    public Sprite mirarAbajo;
    public Sprite mirarArriba;
    public Sprite mirarDerecha;
    public string nombre;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Interactuar(Vector2 posicionPersonaje)
    {
        Vector2 direccionPersonaje = Herramientas.ObtenerDireccion(transform.position, posicionPersonaje);
        MirarPersonaje(direccionPersonaje);
        MostrarDialogo();
    }

    public void MirarPersonaje(Vector2 direccionPersonaje)
    {
        if (direccionPersonaje == Vector2.right)
        {
            spriteRenderer.sprite = mirarDerecha;
            spriteRenderer.flipX = false;
        }
        else if (direccionPersonaje == Vector2.left)
        {
            spriteRenderer.sprite = mirarDerecha;
            spriteRenderer.flipX = true;
        }
        else if (direccionPersonaje == Vector2.down)
        {
            spriteRenderer.sprite = mirarAbajo;
        }
        else if (direccionPersonaje == Vector2.up)
        {
            spriteRenderer.sprite = mirarArriba;
        }
    }

    public void MirarDireccion(Vector2 mirarDireccion)
    {        
        if (mirarDireccion == Vector2.right)
        {
            spriteRenderer.sprite = mirarDerecha;
            spriteRenderer.flipX = false;
        }
        else if (mirarDireccion == Vector2.left)
        {
            spriteRenderer.sprite = mirarDerecha;
            spriteRenderer.flipX = true;
        }
        else if (mirarDireccion == Vector2.down)
        {
            spriteRenderer.sprite = mirarAbajo;
        }
        else if (mirarDireccion == Vector2.up)
        {
            spriteRenderer.sprite = mirarArriba;
        }
    }

    public void MirarDireccionEditor(Vector2 mirarDireccion)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (mirarDireccion == Vector2.right)
        {
            spriteRenderer.sprite = mirarDerecha;
            spriteRenderer.flipX = false;
        }
        else if (mirarDireccion == Vector2.left)
        {
            spriteRenderer.sprite = mirarDerecha;
            spriteRenderer.flipX = true;
        }
        else if (mirarDireccion == Vector2.down)
        {
            spriteRenderer.sprite = mirarAbajo;
        }
        else if (mirarDireccion == Vector2.up)
        {
            spriteRenderer.sprite = mirarArriba;
        }
    }


    public void MostrarDialogo()
    {
        for (int i = listaConversaciones.Count - 1; i >= 0; i--)
        {
            if (listaConversaciones[i].logroConseguido == Logro.NINGUNO || ControladorDatos.Instancia.Datos.ContieneLogro(listaConversaciones[i].logroConseguido))
            {
                Personaje.PuedeMoverse = false;
                ControladorEventos.Instancia.LanzarEvento(new EventoPersonajeMirarDireccion(transform.position));
                if (listaConversaciones[i].tipoConversacion == TipoConversacion.Hablar)
                {
                    ControladorEventos.Instancia.LanzarEvento(new EventoMostrarMensajeConversacion(listaConversaciones[i]));
                    if (listaConversaciones[i].curarEquipoPokemonDelJugador)
                        ControladorDatos.Instancia.Datos.CentroPokemon();
                }
                else if(listaConversaciones[i].tipoConversacion == TipoConversacion.Luchar && listaConversaciones[i].equipoPokemon != null)
                    ControladorEventos.Instancia.LanzarEvento(new EventoIniciarCombateContraEntrenador(listaConversaciones[i], nombre));
                break;
            }
        }
    }

    public Conversacion AniadirDialogoASecuencia(Conversacion ultimaConversacion)
    {
        for (int i = listaConversaciones.Count - 1; i >= 0; i--)
        {
            if (ultimaConversacion != null && listaConversaciones[i].logroConseguido == ultimaConversacion.logroConseguido)
            {
                return ultimaConversacion;
            }

            if (listaConversaciones[i].logroConseguido == Logro.NINGUNO || ControladorDatos.Instancia.Datos.ContieneLogro(listaConversaciones[i].logroConseguido) || (ultimaConversacion != null && ultimaConversacion.darLogroPorTerminarConversacion == listaConversaciones[i].logroConseguido))
            {
                ControladorEventos.Instancia.LanzarEvento(new EventoPersonajeMirarDireccion(transform.position));
                if (listaConversaciones[i].tipoConversacion == TipoConversacion.Hablar)
                    ControladorEventos.Instancia.LanzarEvento(new EventoMostrarMensajeConversacion(listaConversaciones[i]));
                else if (listaConversaciones[i].tipoConversacion == TipoConversacion.Luchar && listaConversaciones[i].equipoPokemon != null)
                    ControladorEventos.Instancia.LanzarEvento(new EventoIniciarCombateContraEntrenador(listaConversaciones[i], nombre));
                return listaConversaciones[i];
            }
        }
        return null;
    }


    public bool TieneDialogoPendiente()
    {
        for (int i = listaConversaciones.Count - 1; i >= 0; i--)
        {
            if (listaConversaciones[i].logroConseguido == Logro.NINGUNO || ControladorDatos.Instancia.Datos.ContieneLogro(listaConversaciones[i].logroConseguido))
            {
                return true;
            }
        }
        return false;
    }

    public TipoConversacion TipoDeLaUltimaConversacion()
    {
        for (int i = listaConversaciones.Count - 1; i >= 0; i--)
        {
            if (listaConversaciones[i].logroConseguido == Logro.NINGUNO || ControladorDatos.Instancia.Datos.ContieneLogro(listaConversaciones[i].logroConseguido))
            {
                return listaConversaciones[i].tipoConversacion;
            }
        }
        return TipoConversacion.Hablar;
    }
    
}
