using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPokemonPokedexDetalles : MonoBehaviour {

    public Image imagen;
    public Text nombre;
    public Text elementos;
    public Text descripcion;

    public void MostrarPokemonPokedex(PokemonID id, PokedexTipoAvistamiento tipoAvistamiento)
    {
        Pokemon p = ControladorDatos.Instancia.ObtenerPokemon(id);
        imagen.sprite = p.sprite;

        switch (tipoAvistamiento)
        {
            //case PokedexTipoAvistamiento.NINGUNO:
            //    imagen.color = Color.black;
            //    nombre.text = Ajustes.Instancia.PokedexNombrePokemonCuandoNoLoHavistoNiCapturado;
            //    elementos.text = Ajustes.Instancia.PokedexNombrePokemonCuandoNoLoHavistoNiCapturado;
            //    descripcion.text = string.Empty;
            //    break;
            case PokedexTipoAvistamiento.Visto:
                imagen.color = Color.white;
                nombre.text = p.nombre;
                elementos.text = Ajustes.Instancia.PokedexNombrePokemonCuandoNoLoHavistoNiCapturado;
                descripcion.text = string.Empty;
                break;
            case PokedexTipoAvistamiento.Capturado:
                imagen.color = Color.white;
                nombre.text = p.nombre;
                string elementosTexto = string.Empty;
                if (p.tipoElemento1 != Elemento.NINGUNO)
                    elementosTexto = string.Concat(Herramientas.TextoAtaqueElemento(p.tipoElemento1), "\n");
                if (p.tipoElemento2 != Elemento.NINGUNO)
                    elementosTexto = string.Concat(elementosTexto, Herramientas.TextoAtaqueElemento(p.tipoElemento2));
                elementos.text = elementosTexto;
                descripcion.text = p.descripcion;
                break;
        }        
        gameObject.SetActive(true);
    }
}
