using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMovilBotonInteractuar : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

    public void OnPointerClick(PointerEventData eventData)
    {
        ControladorEventos.Instancia.LanzarEvento(new EventoBotonMovilPulsadoInteractuar());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ControlesPersonaje.botonCorrerPulsado = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ControlesPersonaje.botonCorrerPulsado = false;
    }
}
