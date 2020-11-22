using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBotonSonido : MonoBehaviour, IPointerClickHandler {

    public SonidoID sonido = SonidoID.SonidoPulsarBotonUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        ControladorDatos.Instancia.ReproducirSonido(sonido);
    }
    
}
