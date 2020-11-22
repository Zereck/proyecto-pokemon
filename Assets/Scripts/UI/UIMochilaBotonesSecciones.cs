using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMochilaBotonesSecciones : MonoBehaviour, IPointerClickHandler {

    public GameObject seccionAbrir;

    private Vector2 tamanioInicial;
    private RectTransform rectTrans;
    private bool pulsado;

    private void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        tamanioInicial = rectTrans.localScale;
    }

    public void RestablecerTamanio()
    {
        pulsado = false;
        rectTrans.localScale = tamanioInicial;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!pulsado)
        {
            pulsado = true;
            rectTrans.localScale *= 1.1f;
            UIControlador.Instancia.MenuMochila_BotonSeccionPulsado(this, seccionAbrir);
        }
    }
    
}
