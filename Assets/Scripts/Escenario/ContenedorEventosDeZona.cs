using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContenedorEventosDeZona : MonoBehaviour {

    private List<EventoDeZona> eventoDeZona;

    private void OnEnable()
    {
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoNuevoLogroConseguido), NuevoEventoDeZona);
        if (eventoDeZona == null || eventoDeZona.Count == 0)
            BuscarEventosDeZona();
    }

    private void OnDisable()
    {
        ControladorEventos.Instancia.DesubscribirseEvento(typeof(EventoNuevoLogroConseguido), NuevoEventoDeZona);
    }

    private void BuscarEventosDeZona()
    {
        eventoDeZona = new List<EventoDeZona>();
        for (int i = 0; i < transform.childCount; i++)
        {
            EventoDeZona e = transform.GetChild(i).GetComponent<EventoDeZona>();
            if (e != null)
                eventoDeZona.Add(e);
        }
    }

    private void NuevoEventoDeZona(EventoBase mensaje)
    {
        EventoNuevoLogroConseguido e = (EventoNuevoLogroConseguido)mensaje;
        for (int i = 0; i < eventoDeZona.Count; i++)
        {
            if (eventoDeZona[i] != null && eventoDeZona[i].logroDisparadorDelAcontecimiento == e.Logro)
            {
                eventoDeZona[i].EjecutarAccion();
                break;
            }
        }
    }
}
