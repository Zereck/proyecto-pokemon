using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventoBase
{
    public string nombreEvento;
    public EventoBase() { nombreEvento = this.GetType().Name; }
}

//MOVER PERSONAJE DE POSICIÓN
public class EventoTeletransportarse : EventoBase
{
    public Vector2 Destino { get; private set; }
    public Direccion DireccionMirar { get; private set; }

    public EventoTeletransportarse(Vector2 destino, Direccion direccion)
    {
        Destino = destino;
        DireccionMirar = direccion;
    }
}
public class EventoTeletransportarseCentroPokemon : EventoBase { }

public class EventoMostrarVentanaConfirmacion : EventoBase
{
    public Action AccionConfirmar { get; private set; }
    public Action AccionDenegar { get; private set; }
    public string MensajeConfirmacion { get; private set; }
    public string MensajeTrasAceptar { get; private set; }
    public string MensajeTrasRechazar { get; private set; }
    public PokemonID PokemonPreview { get; private set; }
    public bool EncolarCorrutina { get; private set; }

    public EventoMostrarVentanaConfirmacion(Action accionConfirmar, Action accionDenegar, string mensajeConfirmacion, string mensajeTrasAceptar, string mensajeTrasRechazar, PokemonID pokemonPreview, bool encolarCorrutina)
    {
        this.AccionConfirmar += accionConfirmar;
        this.AccionDenegar += accionDenegar;
        this.MensajeConfirmacion = mensajeConfirmacion;
        this.MensajeTrasAceptar = mensajeTrasAceptar;
        this.MensajeTrasRechazar = mensajeTrasRechazar;
        this.PokemonPreview = pokemonPreview;
        this.EncolarCorrutina = encolarCorrutina;
    }
}
public class EventoPersonajeMirarDireccion : EventoBase
{
    public Vector2 PosicionObjetivo { get; private set; }

    public EventoPersonajeMirarDireccion(Vector2 posicionObjetivo)
    {
        PosicionObjetivo = posicionObjetivo;
    }
}

//SISTEMA DE DIÁLOGOS E INTERACCIONES CON ITEM DEL MAPA
public class EventoMostrarMensaje : EventoBase
{
    public string Mensaje { get; private set; }

    public EventoMostrarMensaje(string mensaje)
    {
        Mensaje = mensaje;
    }
}
public class EventoMostrarMensajeConversacion : EventoBase
{
    public Conversacion Conversacion { get; private set; }

    public EventoMostrarMensajeConversacion(Conversacion mensaje)
    {
        Conversacion = mensaje;
    }
}
public class EventoPokemonEnMapa : EventoBase
{
    public PokemonModelo PokemonDatos { get; private set; }

    public EventoPokemonEnMapa(PokemonModelo pokemonDatos)
    {
        PokemonDatos = pokemonDatos;
    }
}
public class EventoAniadirNPCAlControlador : EventoBase
{
    public MovimientoNPC NPC { get; private set; }

    public EventoAniadirNPCAlControlador(MovimientoNPC npc)
    {
        NPC = npc;
    }
}

//UI
public class EventoAbrirMenuPrincipal : EventoBase { }
public class EventoBotonMovilPulsadoAbrirMenuPrincipal : EventoBase { }
public class EventoBotonMovilPulsadoInteractuar : EventoBase { }

//COMBATE
public class EventoIniciarCombateContraEntrenador : EventoBase
{
    public Conversacion Conversacion { get; private set; }
    public string NombreNPC { get; private set; }

    public EventoIniciarCombateContraEntrenador(Conversacion conversacion, string nombreNPC)
    {
        Conversacion = conversacion;
        NombreNPC = nombreNPC;
    }
}
public class EventoIniciarCombatePokemonSalvaje : EventoBase
{
    public PokemonEnLaZona PokemonEnLaZona { get; private set; }

    public EventoIniciarCombatePokemonSalvaje(PokemonEnLaZona pokemonEnLaZona)
    {
        PokemonEnLaZona = pokemonEnLaZona;
    }
}
public class EventoProximaAccionCombate : EventoBase
{
    public TipoAccion TipoAccion { get; private set; }
    public AtaqueID Ataque { get; private set; }
    public Item Item { get; private set; }
    public PokemonModelo ProximoPokemon { get; private set; }

    public EventoProximaAccionCombate(TipoAccion tipoAccion, AtaqueID ataque, Item item, PokemonModelo proximoPokemon = null)
    {
        TipoAccion = tipoAccion;
        Ataque = ataque;
        Item = item;
        ProximoPokemon = proximoPokemon;
    }
}

//ZONAS
public class EventoNuevoLogroConseguido : EventoBase
{
    public Logro Logro { get; private set; }

    public EventoNuevoLogroConseguido(Logro logro)
    {
        Logro = logro;
    }
}