using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBotonCombateGenerico : MonoBehaviour, IPointerClickHandler {

    public TipoAccion accionAsociada;

    public void OnPointerClick(PointerEventData eventData)
    {
        UIControlador.Instancia.Combate.menuEleccionesCombate.gameObject.SetActive(false);
        if(accionAsociada == TipoAccion.CambiarPokemon)
        {
            //... Muestra ventana de selección de pokémon
            UIControlador.Instancia.EquipoPokemon.ventanaPrincipal.SetActive(true);
        }
        else if(accionAsociada == TipoAccion.Escapar)
        {
            ControladorEventos.Instancia.LanzarEvento(new EventoProximaAccionCombate(TipoAccion.Escapar, AtaqueID.NINGUNO, null));
        }
    }
}
