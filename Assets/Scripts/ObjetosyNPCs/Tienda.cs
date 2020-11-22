using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tienda : IInteractivo
{
    public ItemID[] itemTienda;

    public override void Interactuar(Vector2 posicionPersonaje)
    {
        UIControlador.Instancia.Tienda.MostrarTienda(itemTienda);
    }    
}
