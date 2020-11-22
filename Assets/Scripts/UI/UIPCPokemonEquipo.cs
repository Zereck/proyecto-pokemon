﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPCPokemonEquipo : MonoBehaviour, IPointerClickHandler
{

    public Image panelFondo;
    public Image imagenPokemon;
    public Text nivelPokemon;
    public Image barraSalud;
    public Text textoSalud;
    public Text estadoAlterado;
    UIPCControlador uiPcPokemon;
    Color panelFondoColorOriginal;

    public PokemonModelo PokemonDatos { get; private set; }

    private void Start()
    {
        panelFondoColorOriginal = panelFondo.color;
    }

    public void PokemonPulsado(PokemonModelo pokemon)
    {
        if (!gameObject.activeSelf) return;

        panelFondo.color = panelFondoColorOriginal;

        // Si el pokemon es NULL significa que debemos establecer todos los botones como si no estuviesen pulsados
        if (pokemon != null && PokemonDatos.IdentificardorUnico == pokemon.IdentificardorUnico)
            panelFondo.color = Color.green;
    }



    public void MostrarPokemon(PokemonModelo pokemonDatos, UIPCControlador uiMoverPokemon)
    {
        this.uiPcPokemon = uiMoverPokemon;
        if (pokemonDatos == null || pokemonDatos.ID == PokemonID.NINGUNO)
            gameObject.SetActive(false);
        else
        {
            PokemonDatos = pokemonDatos;
            imagenPokemon.sprite = pokemonDatos.DatosFijos.sprite;
            nivelPokemon.text = string.Concat(pokemonDatos.DatosFijos.nombre, "\n Lvl. ", pokemonDatos.Nivel);
            barraSalud.fillAmount = pokemonDatos.SaludEnEscalaDe1();
            textoSalud.text = string.Concat(pokemonDatos.Salud, "/", pokemonDatos.EstadisticaSaludMaxima());
            if (pokemonDatos.EstadoAlterado != EstadoAlterado.NINGUNO)
                estadoAlterado.text = pokemonDatos.EstadoAlterado.ToString().Substring(0, 3).ToUpper();
            else
                estadoAlterado.text = "   ";
            gameObject.SetActive(true);
        }

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        uiPcPokemon.PokemonEquipoPulsado(PokemonDatos);
    }
}