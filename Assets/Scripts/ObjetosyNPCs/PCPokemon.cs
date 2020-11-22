using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPokemon : IInteractivo
{    
    public override void Interactuar(Vector2 posicionPersonaje)
    {
        UIControlador.Instancia.PCControlador.Mostrar();
    }    
}
