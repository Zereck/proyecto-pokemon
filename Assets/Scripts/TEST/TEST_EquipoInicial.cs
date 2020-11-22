using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EquipoPokemonEntrenador))]
public class TEST_EquipoInicial : MonoBehaviour {

    private void Start()
    {
        EquipoPokemonEntrenador equipo = GetComponent<EquipoPokemonEntrenador>();
        if(equipo != null && equipo.pokemons != null && equipo.pokemons.Count > 0)
        {
            for (int i = 0; i < equipo.pokemons.Count; i++)
            {
                if(equipo.pokemons[i].id != PokemonID.NINGUNO)
                {
                    ControladorDatos.Instancia.Datos.AniadirNuevoPokemonCapturado(
                        new PokemonModelo(equipo.pokemons[i].id, equipo.pokemons[i].nivel, equipo.pokemons[i].calidad,
                        new AtaqueID[] { equipo.pokemons[i].ataque1, equipo.pokemons[i].ataque2, equipo.pokemons[i].ataque3 }, 0.3f));
                }
            }
        }
    }
}
