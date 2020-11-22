using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIBotonAtaque : MonoBehaviour, IPointerClickHandler {

    public Text nombre;
    public Text pp;
    public Text elemento;
    public Text poder;

    private AtaqueID ataqueID;

    public void MostrarDatosAtaque(AtaquesModelo ataque)
    {
        if (ataque == null || ataque.ID == AtaqueID.NINGUNO)
            gameObject.SetActive(false);
        else
        {
            nombre.text = ataque.DatosFijos.nombre;
            pp.text = ataque.TextoPPActualYMaximo();
            elemento.text = Herramientas.TextoAtaqueElemento(ataque.DatosFijos.ataqueElemento);
            poder.text = ataque.DatosFijos.poder.ToString();
            ataqueID = ataque.ID;
            gameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(ataqueID != AtaqueID.NINGUNO)
        {
            ControladorEventos.Instancia.LanzarEvento(new EventoProximaAccionCombate(TipoAccion.Atacar, ataqueID, null));
            ataqueID = AtaqueID.NINGUNO;
        }
    }

}
