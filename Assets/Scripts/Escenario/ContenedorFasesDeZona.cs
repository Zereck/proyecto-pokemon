using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContenedorFasesDeZona : MonoBehaviour {

    private List<FaseDeZona> fase;

    private void OnEnable()
    {
        if (fase == null || fase.Count == 0)
            BuscarFases();

        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoNuevoLogroConseguido), ComprobarFases);
        ComprobarFases(null);
    }

    private void OnDisable()
    {
        ControladorEventos.Instancia.DesubscribirseEvento(typeof(EventoNuevoLogroConseguido), ComprobarFases);
    }

    private void ComprobarFases(EventoBase mensaje)
    {
        for (int i = 0; i < fase.Count; i++)
        {
            if (fase[i].CumpleLasCondiciones())
            {
                fase[i].ActivarFase();
            }
        }
    }

    private void BuscarFases () {
        fase = new List<FaseDeZona>();
        for (int i = 0; i < transform.childCount; i++)
        {
            FaseDeZona f = transform.GetChild(i).GetComponent<FaseDeZona>();
            if (f != null)
                fase.Add(f);
        }
	}
    
}
