using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntoTeletransporte : MonoBehaviour
{

    public PuntoTeletransporte destino;
    public Direccion direccionMirar = Direccion.PorDefecto;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Ajustes.Instancia.tagPersonaje) && destino != null)
        {
            ControladorEventos.Instancia.LanzarEvento(new EventoTeletransportarse(destino.transform.position, destino.direccionMirar));
        }
    }

#if UNITY_EDITOR

    private Vector2 direccionRayo;

    private void OnDrawGizmos()
    {
        if (destino != null && destino.destino != null)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(0.16f, 0.16f, 0));
            Gizmos.DrawLine(transform.position, destino.transform.position);
            if(direccionMirar != Direccion.PorDefecto)
            {
                direccionRayo = (Vector2)transform.position + (Ajustes.Instancia.tamanioCasilla * Herramientas.ObtenerDireccion(direccionMirar));
                Gizmos.DrawWireSphere(direccionRayo, 0.04f);
            }
            
        }
    }

#endif
}
