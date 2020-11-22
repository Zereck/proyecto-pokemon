using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeMundo : IInteractivo
{
    public ItemID itemID;
    [Range(1, 20)]
    public int cantidad = 1;

    private string nombreIdentificador;

    private void Start()
    {
        nombreIdentificador = string.Concat(transform.position.x, transform.position.y, itemID.ToString(), gameObject.name);
        if (itemID == ItemID.NINGUNO || ControladorDatos.Instancia.Datos.ItemDeMUndoYaConseguido(nombreIdentificador))
            gameObject.SetActive(false);
    }

    public override void Interactuar(Vector2 direccionPersonaje)
    {
        if (itemID != ItemID.NINGUNO)
        {
            string texto = Ajustes.Instancia.textoCuandoConsigueItem
                .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, ControladorDatos.Instancia.ObtenerItem(itemID).nombre)
                .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, cantidad.ToString());

            ControladorDatos.Instancia.Datos.ItemDeMundoConseguido(nombreIdentificador);
            ControladorDatos.Instancia.Datos.AniadirItemEncontradoAlInventario(itemID, cantidad);
            ControladorEventos.Instancia.LanzarEvento(new EventoMostrarMensaje(texto));
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("El item de mundo no tiene asignado un itemID");
        }
    }
}
