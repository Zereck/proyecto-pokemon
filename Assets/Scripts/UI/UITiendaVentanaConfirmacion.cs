using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITiendaVentanaConfirmacion : MonoBehaviour {

    public Text cantidadItems;
    public Text precioTotal;
    public Button botonConfirmar;

    private Item item;
    private int cantidad;
    
	public void MostrarVentanaConfirmacion(Item item)
    {
        this.item = item;

        cantidad = 1;
        ActualizarPrecioTotal();

        gameObject.SetActive(true);
    }

    public void AumentarCantidad()
    {
        cantidad = Mathf.Clamp(cantidad + 1, 0, Ajustes.cantidadMaximaItems);
        ActualizarPrecioTotal();
    }

    public void ReducirCantidad()
    {
        cantidad = Mathf.Clamp(cantidad - 1, 0, Ajustes.cantidadMaximaItems);
        ActualizarPrecioTotal();
    }

    private void ActualizarPrecioTotal()
    {
        cantidadItems.text = cantidad.ToString("D2");
        precioTotal.text = (cantidad * item.precioEnTienda).ToString();

        if (cantidad <= 0 || (cantidad * item.precioEnTienda) > ControladorDatos.Instancia.Datos.Monedas)
            botonConfirmar.interactable = false;
        else
            botonConfirmar.interactable = true;
    }

    public void ConfirmarOperacion()
    {
        ControladorDatos.Instancia.Datos.ComprarItem(item, cantidad);
        UIControlador.Instancia.Tienda.dineroActual.text = ControladorDatos.Instancia.Datos.Monedas.ToString();
        precioTotal.text = string.Empty;
        cantidad = 1;
        cantidadItems.text = cantidad.ToString("D2");
        gameObject.SetActive(false);
    }
}
