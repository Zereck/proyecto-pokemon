using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuPokemon : MonoBehaviour {
    
    private void OnEnable()
    {
        UIControlador.Instancia.DetallesPokemon.componentePrincipal.gameObject.SetActive(false);
        PokemonModelo[] equipoJugador = ControladorDatos.Instancia.Datos.ObtenerEquipoPokemon();
        UIControlador.Instancia.EquipoPokemon.pokemon1.MostrarPokemon(equipoJugador[0]);
        UIControlador.Instancia.EquipoPokemon.pokemon2.MostrarPokemon(equipoJugador[1]);
        UIControlador.Instancia.EquipoPokemon.pokemon3.MostrarPokemon(equipoJugador[2]);
        UIControlador.Instancia.EquipoPokemon.pokemon4.MostrarPokemon(equipoJugador[3]);
        UIControlador.Instancia.EquipoPokemon.pokemon5.MostrarPokemon(equipoJugador[4]);
        UIControlador.Instancia.EquipoPokemon.pokemon6.MostrarPokemon(equipoJugador[5]);
    }
    
    
}
