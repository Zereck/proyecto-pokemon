using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPokemonPokedex : MonoBehaviour, IPointerClickHandler {

    public PokemonID pokemon;
    public Text nombre;
    public Image iconoEstado;
    public Text numero;

    private PokedexTipoAvistamiento tipoAvistamiento;

    private void OnEnable()
    {
        tipoAvistamiento = ControladorDatos.Instancia.Datos.Pokedex(pokemon);
        if(tipoAvistamiento == PokedexTipoAvistamiento.NINGUNO)
        {
            nombre.text = Ajustes.Instancia.PokedexNombrePokemonCuandoNoLoHavistoNiCapturado;
            iconoEstado.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(tipoAvistamiento != PokedexTipoAvistamiento.NINGUNO)
            UIControlador.Instancia.pokedexDetalles.MostrarPokemonPokedex(pokemon, tipoAvistamiento);
    }

}
