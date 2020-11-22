using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDetallesPokemon : MonoBehaviour {
        
    public void MostrarDetallesPokemon(PokemonModelo pokemonDatos)
    {
        gameObject.SetActive(true);
        UIControlador.Instancia.DetallesPokemon.imagen.sprite = pokemonDatos.DatosFijos.sprite;
        UIControlador.Instancia.DetallesPokemon.nombre.text = pokemonDatos.DatosFijos.nombre;
        string ele = string.Empty;
        if (pokemonDatos.DatosFijos.tipoElemento1 != Elemento.NINGUNO)
            ele = Herramientas.TextoAtaqueElemento(pokemonDatos.DatosFijos.tipoElemento1);
        if (pokemonDatos.DatosFijos.tipoElemento2 != Elemento.NINGUNO)
            ele = string.Concat(ele, " ", Herramientas.TextoAtaqueElemento(pokemonDatos.DatosFijos.tipoElemento2));
        UIControlador.Instancia.DetallesPokemon.elementos.text = ele;

        UIControlador.Instancia.DetallesPokemon.ataques1.MostrarAtaque(pokemonDatos.Ataques()[0], pokemonDatos);
        UIControlador.Instancia.DetallesPokemon.ataques2.MostrarAtaque(pokemonDatos.Ataques()[1], pokemonDatos);
        UIControlador.Instancia.DetallesPokemon.ataques3.MostrarAtaque(pokemonDatos.Ataques()[2], pokemonDatos);
        UIControlador.Instancia.DetallesPokemon.ataques4.MostrarAtaque(pokemonDatos.Ataques()[3], pokemonDatos);

        UIControlador.Instancia.DetallesPokemon.salud.text = pokemonDatos.EstadisticaSaludMaxima().ToString();
        UIControlador.Instancia.DetallesPokemon.ataqueFisico.text = pokemonDatos.EstadisticaAtaqueFisico().ToString();
        UIControlador.Instancia.DetallesPokemon.defensaFisica.text = pokemonDatos.EstadisticaDefensaFisica().ToString();
        UIControlador.Instancia.DetallesPokemon.ataqueMagico.text = pokemonDatos.EstadisticaAtaqueMagico().ToString();
        UIControlador.Instancia.DetallesPokemon.defensaMagica.text = pokemonDatos.EstadisticaDefensaMagica().ToString();
        UIControlador.Instancia.DetallesPokemon.velocidad.text = pokemonDatos.EstadisticaVelocidad().ToString();
    }
}
