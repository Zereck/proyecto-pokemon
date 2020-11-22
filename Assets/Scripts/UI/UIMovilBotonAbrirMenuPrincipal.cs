using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMovilBotonAbrirMenuPrincipal: MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData)
    {
        ControladorEventos.Instancia.LanzarEvento(new EventoBotonMovilPulsadoAbrirMenuPrincipal());
        
    }
}
