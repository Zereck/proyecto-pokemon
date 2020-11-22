using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DatosCombate
{
    public bool CombateActivo { get; set; }
    public List<PokemonLuchador> EquipoPokemonEnemigos { get; private set; }
    public List<PokemonLuchador> EquipoPokemonJugador { get; private set; }
    public bool CombateGanado { get; set; }
    public TipoDeCombate TipoCombate { get; private set; }


    private int pokemonJugadorActivoIndice = 0;
    private int pokemonEnemigoActivoIndice = 0;

    public DatosCombate()
    {
        EquipoPokemonEnemigos = new List<PokemonLuchador>();
        EquipoPokemonJugador = new List<PokemonLuchador>();
        CombateGanado = false;
        CombateActivo = true;
    }

    public void PrepararCombateContraEntrenador(EventoIniciarCombateContraEntrenador entrenador)
    {
        GenerarEquipoPokemonDelJugador();
        GenerarEquipoPokemonDelRival(entrenador);
        TipoCombate = TipoDeCombate.Entrenador;
    }

    public void PrepararCombateContraPokemonSalvaje(EventoIniciarCombatePokemonSalvaje pokemon)
    {
        GenerarEquipoPokemonDelJugador();
        GenerarPokemonSalvaje(pokemon);
        TipoCombate = TipoDeCombate.PokemonSalvaje;
    }

    private void GenerarEquipoPokemonDelRival(EventoIniciarCombateContraEntrenador entrenador)
    {
        for (int i = 0; i < entrenador.Conversacion.equipoPokemon.pokemons.Count; i++)
        {
            AniadirPokemonAlEquipoEnemigo(entrenador.Conversacion.equipoPokemon.pokemons[i]);
        }
    }

    private void AniadirPokemonAlEquipoEnemigo(PokemonEntrenador p)
    {
        if (p.id != PokemonID.NINGUNO)
            EquipoPokemonEnemigos.Add(new PokemonLuchador(p.id, p.nivel, 1, new AtaqueID[] { p.ataque1, p.ataque2, p.ataque3, p.ataque4 }));
    }

    private void GenerarEquipoPokemonDelJugador()
    {
        PokemonModelo[] equipoPokemon = ControladorDatos.Instancia.Datos.ObtenerEquipoPokemon();
        for (int i = 0; i < equipoPokemon.Length; i++)
        {
            if(equipoPokemon[i] != null && equipoPokemon[i].ID != PokemonID.NINGUNO)
                EquipoPokemonJugador.Add(new PokemonLuchador(equipoPokemon[i]));
        }
    }

    private void GenerarPokemonSalvaje(EventoIniciarCombatePokemonSalvaje pokemonSalvaje)
    {
        int posibilidadTotal = 0;
        ProbabilidadEncuentroPokemon[] pokemonOrdenadorPorProbabilidad = new ProbabilidadEncuentroPokemon[pokemonSalvaje.PokemonEnLaZona.pokemons.Count];

        //Almacenamos el porcentaje de todos los pokémon
        for (int i = 0; i < pokemonSalvaje.PokemonEnLaZona.pokemons.Count; i++)
        {
            posibilidadTotal += pokemonSalvaje.PokemonEnLaZona.pokemons[i].posibilidadAparicion;
        }

        //Calculamos el porcentaje individual de cada pokémon en proporción al total
        for (int i = 0; i < pokemonSalvaje.PokemonEnLaZona.pokemons.Count; i++)
        {
            pokemonOrdenadorPorProbabilidad[i].probabilidad = ((pokemonSalvaje.PokemonEnLaZona.pokemons[i].posibilidadAparicion * 100) / posibilidadTotal);
            pokemonOrdenadorPorProbabilidad[i].pokemon = pokemonSalvaje.PokemonEnLaZona.pokemons[i];
        }
        //Lo ordenamos por porcentaje de menos a más
        pokemonOrdenadorPorProbabilidad = pokemonOrdenadorPorProbabilidad.OrderBy(i => i.probabilidad).ToArray();

        //Sumamos el porcentaje del pokémon anterior al actual para establecer el rango de aparición y que no se solapen
        float porcentajeYaAsignado = 0;
        for (int i = 0; i < pokemonOrdenadorPorProbabilidad.Length; i++)
        {
            pokemonOrdenadorPorProbabilidad[i].probabilidad += porcentajeYaAsignado;
            porcentajeYaAsignado = pokemonOrdenadorPorProbabilidad[i].probabilidad;
        }
        float random = UnityEngine.Random.Range(0f, 100f);
        int nivelPokemon = UnityEngine.Random.Range(pokemonSalvaje.PokemonEnLaZona.nivelMinimoPokemonSalvaje, pokemonSalvaje.PokemonEnLaZona.nivelMaximoPokemonSalvaje + 1);
        int calidadPokemon = UnityEngine.Random.Range(Ajustes.calidadMinimaPokemon, Ajustes.calidadMaximaPokemon + 1);

        for (int i = 0; i < pokemonOrdenadorPorProbabilidad.Length; i++)
        {            
            if (random < pokemonOrdenadorPorProbabilidad[i].probabilidad || i == pokemonOrdenadorPorProbabilidad.Length - 1) //Si es el último pokémon y no se ha asgiando todavía el pokémon salvaje forzamos que se asigne el último
            {
                EquipoPokemonEnemigos.Add(new PokemonLuchador(pokemonOrdenadorPorProbabilidad[i].pokemon.id, nivelPokemon, calidadPokemon, new AtaqueID[] { pokemonOrdenadorPorProbabilidad[i].pokemon.ataque1, pokemonOrdenadorPorProbabilidad[i].pokemon.ataque2, pokemonOrdenadorPorProbabilidad[i].pokemon.ataque3, pokemonOrdenadorPorProbabilidad[i].pokemon.ataque4 }));
                break;
            }
        }        
    }

    public PokemonLuchador PokemonJugadorActivo()
    {
        return EquipoPokemonJugador[pokemonJugadorActivoIndice];
    }

    public PokemonLuchador PokemonEnemigoActivo()
    {
        return EquipoPokemonEnemigos[pokemonEnemigoActivoIndice];
    }

    public int PokemonEnemigoDerrotadoCalcularXP()
    {
        float multiplicadorTipoCombate = 1;
        if (TipoCombate == TipoDeCombate.Entrenador)
            multiplicadorTipoCombate = 1.5f;

        return (int)(multiplicadorTipoCombate * Ajustes.Instancia.multiplicadorExperienciaBase * (float)PokemonEnemigoActivo().Pokemon.Nivel) / 7;
        

    }

    public bool ElJugadorTieneAlgunPokemonVivo()
    {
        return EquipoPokemonJugador.Where(x => x.Pokemon.EstaVivo()).ToList().Count() > 0;
    }

    public bool ElEnemigoTieneAlgunPokemonVivo()
    {
        return EquipoPokemonEnemigos.Where(x => x.Pokemon.EstaVivo()).ToList().Count() > 0;
    }

    public void PokemonEnemigoDerrotadoCambiarPorUnoVivo()
    {
        pokemonEnemigoActivoIndice = BuscarIndiceDelSiguientePokemonVivo(EquipoPokemonEnemigos);
    }

    private int BuscarIndiceDelSiguientePokemonVivo(List<PokemonLuchador> listaDePokemonAComprobar)
    {
        for (int i = 0; i < listaDePokemonAComprobar.Count; i++)
        {
            if (listaDePokemonAComprobar[i].Pokemon.EstaVivo())
            {
                return i;
            }

        }
        return -1;
    }
    
    public void CambiarPokemonActivoDelJugador(PokemonModelo activarPokemon)
    {
        for (int i = 0; i < EquipoPokemonJugador.Count; i++)
        {
            if(EquipoPokemonJugador[i].Pokemon.IdentificardorUnico == activarPokemon.IdentificardorUnico)
            {
                pokemonJugadorActivoIndice = i;
                break;
            }
        }
    }
    
    private struct ProbabilidadEncuentroPokemon
    {
        public float probabilidad;
        public PokemonSalvaje pokemon;
    }
}
