using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuPrincipal : MonoBehaviour {

    private void OnEnable()
    {
        Personaje.UIAbierta = true;
    }

    public void OnDisable()
    {
        Personaje.UIAbierta = false;
    }
}
