using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMochilaItem : MonoBehaviour, IPointerClickHandler {

    public Text nombre;
    public Text descripcion;
    public Text cantidadText;

    private Item itemRelacionado;

    public void MostrarItem(Item item, int cantidad)
    {
        itemRelacionado = item;
        nombre.text = item.nombre;
        descripcion.text = item.descripcion;
        cantidadText.text = cantidad.ToString("D2");
        gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (itemRelacionado.tipoDeItem)
        {
            case TipoDeItem.Curacion:
                UIControlador.Instancia.MenuMochila_ItemPulsado(itemRelacionado);
                break;
            case TipoDeItem.Pokeball:
                //Sólo se pueden utilizar pokeballs en combates contra pokémon salvajes
                if(ControladorCombate.DatosCombate != null && ControladorCombate.DatosCombate.CombateActivo && ControladorCombate.DatosCombate.TipoCombate == TipoDeCombate.PokemonSalvaje)
                    UIControlador.Instancia.MenuMochila_ItemPulsado(itemRelacionado);
                break;
            //case TipoDeItem.MT:
            case TipoDeItem.Otros:
                //Sólo se pueden utilizar fuera de combate
                if (ControladorCombate.DatosCombate != null && !ControladorCombate.DatosCombate.CombateActivo)
                    UIControlador.Instancia.MenuMochila_ItemPulsado(itemRelacionado);
                break;
        }
    }
}