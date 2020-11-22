using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVentanaIncrementoEstadisticas : MonoBehaviour {

    public Text saludActual;
    public Text saludIncremento;
    public Text ataqueFisicoActual;
    public Text ataqueFisicoIncremento;
    public Text defensaFisicaActual;
    public Text defensaFisicaIncremento;
    public Text ataqueMagicoActual;
    public Text ataqueMagicoIncremento;
    public Text defensaMagicaActual;
    public Text defensaMagicaIncremento;
    public Text velocidadActual;
    public Text velocidadIncremento;

    public void MostrarIncrementoEstadisticas(PokemonModelo pokemon)
    {
        saludActual.text = pokemon.EstadisticaSaludMaxima().ToString();
        ataqueFisicoActual.text = pokemon.EstadisticaAtaqueFisico().ToString();
        defensaFisicaActual.text = pokemon.EstadisticaDefensaFisica().ToString();
        ataqueMagicoActual.text = pokemon.EstadisticaAtaqueMagico().ToString();
        defensaMagicaActual.text = pokemon.EstadisticaDefensaMagica().ToString();
        velocidadActual.text = pokemon.EstadisticaVelocidad().ToString();

        IncrementoEstadisticas incrementos = pokemon.IncrementoDeEstadisticasRespectoAlNivelAnterior();

        saludIncremento.text = string.Concat("+", incrementos.saludIncremento.ToString());
        ataqueFisicoIncremento.text = string.Concat("+", incrementos.ataqueFisicoIncremento.ToString());
        defensaFisicaIncremento.text = string.Concat("+", incrementos.defensaFisicaIncremento.ToString());
        ataqueMagicoIncremento.text = string.Concat("+", incrementos.ataqueMagicoIncremento.ToString());
        defensaMagicaIncremento.text = string.Concat("+", incrementos.defensaMagicaIncremento.ToString());
        velocidadIncremento.text = string.Concat("+", incrementos.velocidadIncremento.ToString());

        gameObject.SetActive(true);
    }
}
