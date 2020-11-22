using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMenuPokemonElementoLista : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerEnterHandler, IEndDragHandler, IPointerClickHandler
{
    public Image imagenPokemon;
    public Text nivelPokemon;
    public Image barraSalud;
    public Text textoSalud;
    public Text estadoAlterado;
    
    public PokemonModelo PokemonDatos { get; private set; }

    private static GameObject draggable;

    public void MostrarPokemon(PokemonModelo pokemonDatos)
    {
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
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if((ControladorCombate.DatosCombate == null || !ControladorCombate.DatosCombate.CombateActivo) && PokemonDatos != null)
        {
            draggable = gameObject;
        }        
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggable = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if ((ControladorCombate.DatosCombate == null || !ControladorCombate.DatosCombate.CombateActivo) && draggable != null && draggable != gameObject)
        {
            draggable.transform.SetSiblingIndex(transform.GetSiblingIndex());
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        UIControlador.Instancia.UIMenuPokemonElementoListaPulsado(this);
    }
    
}