using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMedallasGimnasio : MonoBehaviour {

    public Logro logroMedalla = Logro.Medalla_Gimnasion_1;

    private Image imagen;
    
	private void OnEnable () {

        if(imagen == null)
            imagen = GetComponent<Image>();

        if (ControladorDatos.Instancia.Datos.ContieneLogro(logroMedalla))
            imagen.color = Color.white;
        else
            imagen.color = Color.black;
	}	
}
