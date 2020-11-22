using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemDeMundoQueDaPokemon : IInteractivo
{
    public PokemonID pokemon;
    public Logro[] darLogroAlObtenerlo;
    public int nivel;
    [Range(Ajustes.calidadMinimaPokemon, Ajustes.calidadMaximaPokemon)]
    public int calidad;
    public AtaqueID ataque1 = AtaqueID.NINGUNO;
    public AtaqueID ataque2 = AtaqueID.NINGUNO;
    public AtaqueID ataque3 = AtaqueID.NINGUNO;
    public AtaqueID ataque4 = AtaqueID.NINGUNO;

    public override void Interactuar(Vector2 direccionPersonaje)
    {
        Pokemon p = ControladorDatos.Instancia.ObtenerPokemon(pokemon);
        string mensajeFinalDeConfirmacion = Ajustes.Instancia.textoPokemonInicialMensajeConfirmacion.Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, p.nombre);
        string mensajeFinalTrasAceptar = Ajustes.Instancia.textoPokemonInicialMensajeTrasConfirmar.Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, p.nombre);
        ControladorEventos.Instancia.LanzarEvento(new EventoMostrarVentanaConfirmacion(AccionConfirmacion, null, mensajeFinalDeConfirmacion, mensajeFinalTrasAceptar, string.Empty, pokemon, true));
    }

    private void AccionConfirmacion()
    {
        for (int i = 0; i < darLogroAlObtenerlo.Length; i++)
        {
            if (darLogroAlObtenerlo[i] != Logro.NINGUNO)
                ControladorDatos.Instancia.Datos.AniadirLogro(darLogroAlObtenerlo[i]);
        }
        PokemonModelo pokemonDatos = new PokemonModelo(pokemon, nivel, calidad, new AtaqueID[] { ataque1, ataque2, ataque3, ataque4 }, 1);
        ControladorDatos.Instancia.Datos.AniadirNuevoPokemonCapturado(pokemonDatos);
        gameObject.SetActive(false);
    }
}
