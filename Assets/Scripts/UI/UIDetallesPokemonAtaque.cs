using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIDetallesPokemonAtaque : MonoBehaviour {

    public Text nombre;
    public Text elemento;
    public Text pp;
    public Text poderYTipo;

	public void MostrarAtaque(AtaquesModelo ataque, PokemonModelo pokemon)
    {
        if(ataque == null || ataque.ID == AtaqueID.NINGUNO || pokemon == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            nombre.text = pokemon.Ataque(ataque.ID).DatosFijos.nombre;
            elemento.text = Herramientas.TextoAtaqueElemento(pokemon.Ataque(ataque.ID).DatosFijos.ataqueElemento);
            pp.text = pokemon.Ataque(ataque.ID).TextoPPActualYMaximo();
            poderYTipo.text = string.Concat(pokemon.Ataque(ataque.ID).DatosFijos.poder, " ", pokemon.Ataque(ataque.ID).DatosFijos.TextoTipoAtaque());
            gameObject.SetActive(true);
        }        
    }
}
