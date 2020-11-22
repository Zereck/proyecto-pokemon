using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UITiendaElementoComprar : MonoBehaviour, IPointerClickHandler {

    public Text nombre;
    public Text precio;

    private Item item;

	public void AsignarValores(ItemID itemID)
    {
        item = ControladorDatos.Instancia.ObtenerItem(itemID);
        nombre.text = item.nombre;
        precio.text = item.precioEnTienda.ToString();
        gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIControlador.Instancia.Tienda.ItemPulsado(item);
    }
}
