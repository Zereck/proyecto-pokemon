using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBotonesAtaquePanel : MonoBehaviour {
    
    private void Start()
    {
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoProximaAccionCombate), ProximaAccionSeleccionada);
    }

    private void ProximaAccionSeleccionada(EventoBase mensaje)
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        PokemonLuchador p = ControladorCombate.DatosCombate.PokemonJugadorActivo();
        UIControlador.Instancia.CombateAtaques.botonAtaque1.MostrarDatosAtaque(p.Pokemon.Ataques()[0]);
        UIControlador.Instancia.CombateAtaques.botonAtaque2.MostrarDatosAtaque(p.Pokemon.Ataques()[1]);
        UIControlador.Instancia.CombateAtaques.botonAtaque3.MostrarDatosAtaque(p.Pokemon.Ataques()[2]);
        UIControlador.Instancia.CombateAtaques.botonAtaque4.MostrarDatosAtaque(p.Pokemon.Ataques()[3]);
    }

}
