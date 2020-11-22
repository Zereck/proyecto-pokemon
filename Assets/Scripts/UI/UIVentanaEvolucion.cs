using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVentanaEvolucion : MonoBehaviour {

    public RectTransform posicionPokemon;
    public Animation animacion;
    public bool EvolucionDetenida { get; set; }

    private void OnEnable()
    {
       EvolucionDetenida = false;
    }

    public void CancelarEvolucion()
    {
        EvolucionDetenida = true;
    }
    
}
