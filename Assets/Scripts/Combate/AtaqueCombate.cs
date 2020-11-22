using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueCombate {

    public AtaqueID id;
    public int danio;
    public Elemento elemento;
    public bool pokemonOriginalSeHaceDanio;
    public bool pokemonOriginalSeCura;
    public float porcentajeDanioPokemonOriginal;
    public float porcentajeCuracionPokemonOriginal;
    public EstadoAlterado estadoAlteradoProvocado;
    public bool haFallado;
    public bool esCritico;
    public EstadoAlterado estadoAlteradoDelPokemonAtacante;
    public TipoDeAtaque tipoAtaque;

    public AtaqueCombate()
    {
        this.danio = 0;
        this.pokemonOriginalSeHaceDanio = false;
        this.pokemonOriginalSeCura = false;
        this.porcentajeDanioPokemonOriginal = 0;
        this.porcentajeCuracionPokemonOriginal = 0;
        this.estadoAlteradoProvocado = EstadoAlterado.NINGUNO;
        this.haFallado = false;
        this.esCritico = false;
        this.estadoAlteradoDelPokemonAtacante = EstadoAlterado.NINGUNO;
        this.tipoAtaque = TipoDeAtaque.Fisico;
    }

    public override string ToString()
    {
        return string.Concat(" - Falla? ", haFallado, " - Daño ", danio);
    }
}
