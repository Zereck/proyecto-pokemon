using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIAprenderNuevoAtaqueDetalles : MonoBehaviour, IPointerClickHandler {

    public Text nombre;
    public Text elemento;
    public Text poderYTipo;

    private Ataque ataque;
    private UIAprenderNuevoAtaqueVentana componenteControlador;

	public void MostrarAtaque(Ataque ataquePokemon, UIAprenderNuevoAtaqueVentana controlador)
    {
        componenteControlador = controlador;
        ataque = ataquePokemon;

        if (ataque != null && ataque.ID != AtaqueID.NINGUNO)
        {
            nombre.text = ataque.nombre;
            elemento.text = Herramientas.TextoAtaqueElemento(ataque.ataqueElemento);
            poderYTipo.text = string.Concat(ataque.poder, " ", ataque.TextoTipoAtaque());
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(ataque != null && ataque.ID != AtaqueID.NINGUNO && componenteControlador.AtaqueSeleccionado == null)
        {
            componenteControlador.AtaquePulsado(ataque);
        }
    }
}
