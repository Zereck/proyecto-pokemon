using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PokemonLuchador {

    public PokemonModelo Pokemon { get; private set; }

    //Constructor para los Pokémon del jugador
    public PokemonLuchador(PokemonModelo pokemonJugador)
    {
        Pokemon = pokemonJugador;
        Debug.Log("Pokemon jugador " + Pokemon.ToString());
    }

    //Constructor para los Pokémon enemigos
    public PokemonLuchador(PokemonID pokemonID, int nivel, int calidadDelPokemon, AtaqueID[] ataques)
    {
        Pokemon = new PokemonModelo(pokemonID, nivel, calidadDelPokemon, ataques, 1);
        Debug.Log("Pokemon enemigo " + Pokemon.ToString());
    }

    public void RecibirAtaque(AtaqueCombate ataque)
    {
        Pokemon.ReducirSalud(ataque.danio);
        if(ataque.estadoAlteradoProvocado != EstadoAlterado.NINGUNO && Pokemon.EstaVivo())
        {
            Pokemon.EstadoAlterado = ataque.estadoAlteradoProvocado;
        }
    }

    public void RecibirAutoAtaque(AtaqueCombate autoAtaque)
    {
        int cantidadDanio = (int)(autoAtaque.danio * autoAtaque.porcentajeDanioPokemonOriginal);
        Pokemon.ReducirSalud(cantidadDanio);
    }

    public void RecibirAutoSanacion(AtaqueCombate autoSanacion)
    {
        int cantidadRestaurada = (int)(autoSanacion.danio * autoSanacion.porcentajeCuracionPokemonOriginal);
        Pokemon.RestaurarSalud(cantidadRestaurada);
    }

    public AtaqueCombate Atacar(AtaqueID ataqueID)
    {
        AtaqueCombate proximoAtaque = new AtaqueCombate();
        proximoAtaque.haFallado = ComprobarSiElAtaqueFalla(ataqueID);
        proximoAtaque.tipoAtaque = Pokemon.Ataque(ataqueID).DatosFijos.tipoDeAtaque;

        if (!proximoAtaque.haFallado)
        {
            proximoAtaque.id = ataqueID;
            proximoAtaque.danio = CalcularDanio(ataqueID);
            proximoAtaque.estadoAlteradoProvocado = CalcularEstadoAlterado(ataqueID);
            if (ComprobarSiSeAutoCura(ataqueID))
            {
                proximoAtaque.pokemonOriginalSeCura = true;
                proximoAtaque.porcentajeCuracionPokemonOriginal = Pokemon.Ataque(ataqueID).DatosFijos.porcentajeDeCuracion;
            }
            if (ComprobarSiSeAutoInflingeDanio(ataqueID))
            {
                proximoAtaque.pokemonOriginalSeHaceDanio = true;
                proximoAtaque.porcentajeDanioPokemonOriginal = Pokemon.Ataque(ataqueID).DatosFijos.porcentajeDeDanioAutoInflingido;
            }            
        }
        Debug.Log(string.Concat("Pokemon ", Pokemon.ID.ToString(), " va a atacar: ", ataqueID.ToString(), " ", proximoAtaque.ToString()));
        return proximoAtaque;
    }

    private bool AtaqueCritico(AtaqueID ataqueID)
    {
        return ComprobarProbabilidad(Pokemon.Ataque(ataqueID).DatosFijos.posibilidadCritico);
    }

    private bool ComprobarSiSeAutoCura(AtaqueID ataqueID)
    {
        if (Pokemon.Ataque(ataqueID).DatosFijos.seAutoCura)
        {
            return ComprobarProbabilidad(Pokemon.Ataque(ataqueID).DatosFijos.probabilidadDeCurarse);
        }
        return false;
    }
    

    private bool ComprobarSiSeAutoInflingeDanio(AtaqueID ataqueID)
    {
        if (Pokemon.Ataque(ataqueID).DatosFijos.seAutoInflingeDanio)
        {
            return ComprobarProbabilidad(Pokemon.Ataque(ataqueID).DatosFijos.probabilidadDeAutoInflingirseDanio);
        }
        return false;
    }

    private EstadoAlterado CalcularEstadoAlterado(AtaqueID ataqueID)
    {
        if(Pokemon.Ataque(ataqueID).DatosFijos.provocaEstadoAlterado != EstadoAlterado.NINGUNO)
        {
            if(ComprobarProbabilidad(Pokemon.Ataque(ataqueID).DatosFijos.probabilidadDeProvocarEstadoAlterado))
                return Pokemon.Ataque(ataqueID).DatosFijos.provocaEstadoAlterado;
        }
        return EstadoAlterado.NINGUNO;
    }

    private bool ComprobarSiElAtaqueFalla(AtaqueID ataqueID)
    {
        //TODO: implementar
        switch (Pokemon.EstadoAlterado)
        {
            case EstadoAlterado.Paralizado:
                break;
            case EstadoAlterado.Confuso:
                break;
            case EstadoAlterado.Dormido:
                break;
            case EstadoAlterado.Envenedado:
                break;
        }

        //Devolvemos lo contrario al método
        return !ComprobarProbabilidad(Pokemon.Ataque(ataqueID).DatosFijos.precision);
    }

    private bool ComprobarProbabilidad(int probabilidad)
    {
        if (probabilidad == 100)
            return true;
        if (probabilidad == 0)
            return false;
        int precision = UnityEngine.Random.Range(0, 101);
        if (precision <= probabilidad)
            return true;
        return false;
    }

    private int CalcularDanio(AtaqueID ataqueID)
    {
        float valorPorElementoDelPokemon = ValorPorElementoDelPokemon(ataqueID);
        int valorPorEstadisticaDelPokemon = ValorPorEstadisticaDelPokemon(ataqueID);
        float valorPorPotenciaDelAtaqueBase = (Pokemon.Ataque(ataqueID).DatosFijos.poder / 10);
        return (int)((valorPorEstadisticaDelPokemon + valorPorPotenciaDelAtaqueBase) * valorPorElementoDelPokemon);
    }

    private float ValorPorElementoDelPokemon(AtaqueID ataqueID)
    {
        float multiplicadorTipo = 1;
        if (Pokemon.Ataque(ataqueID).DatosFijos.ataqueElemento == Pokemon.DatosFijos.tipoElemento1 || Pokemon.Ataque(ataqueID).DatosFijos.ataqueElemento == Pokemon.DatosFijos.tipoElemento2)
            multiplicadorTipo = 1.2f;
        return multiplicadorTipo;
    }

    private int ValorPorEstadisticaDelPokemon(AtaqueID ataqueID)
    {
        int multiplicadorTipoDeAtaque = 1;
        if (Pokemon.Ataque(ataqueID).DatosFijos.tipoDeAtaque == TipoDeAtaque.Fisico)
            multiplicadorTipoDeAtaque = Pokemon.EstadisticaAtaqueFisico();
        else
            multiplicadorTipoDeAtaque = Pokemon.EstadisticaAtaqueMagico();
        return multiplicadorTipoDeAtaque;
    }

    public void RestarPP(AtaqueID ataque)
    {
        Pokemon.Ataque(ataque).RestarPP();
    }

    public AtaqueCombate PokemonEnemigoAtaqueAleatorio()
    {
        List<AtaquesModelo> ataquesTemp = Pokemon.Ataques().Where(x => x != null && x.ID != AtaqueID.NINGUNO).ToList();
        if(ataquesTemp.Count == 1)
        {
            Debug.Log(string.Concat("Pokemon enemigo lanza ataque... ", ataquesTemp[0].ID));
            return Atacar(ataquesTemp[0].ID);
        }
        else
        {
            int ataque = UnityEngine.Random.Range(0, ataquesTemp.Count);
            Debug.Log(string.Concat("Pokemon enemigo lanza ataque... ", ataquesTemp[ataque].ID));
            return Atacar(ataquesTemp[ataque].ID);
        }
    }

    
}
