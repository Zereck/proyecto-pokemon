using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonEnLaZona : MonoBehaviour {

    [Range(Ajustes.nivelMinimo,Ajustes.nivelMaximo)]
    public int nivelMinimoPokemonSalvaje = 5;
    [Range(Ajustes.nivelMinimo, Ajustes.nivelMaximo)]
    public int nivelMaximoPokemonSalvaje = 8;
    [HideInInspector]
    public List<PokemonSalvaje> pokemons;
}
