using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIVentanaConfirmacion : MonoBehaviour {
    
    public Eleccion UltimaEleccion { get; private set; }
    
    public void MostrarVentana()
    {
        UltimaEleccion = Eleccion.EnEspera;
        gameObject.SetActive(true);

    }

	public void BotonConfirmacionPulsado()
    {
        UltimaEleccion = Eleccion.Si;
        Restablecer();
    }

    public void BotonNegacionPulsado()
    {
        UltimaEleccion = Eleccion.No;
        Restablecer();
    }

    private void Restablecer()
    {
        gameObject.SetActive(false);
    }
}
