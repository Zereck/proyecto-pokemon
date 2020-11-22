using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorNPC : MonoBehaviour {

    private List<MovimientoNPC> npcs = new List<MovimientoNPC>();
    private List<MovimientoNPC> moverNPC = new List<MovimientoNPC>();

    private void OnEnable()
    {
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoAniadirNPCAlControlador), AniadirNPC);
    }

    private void Start()
    {
        StartCoroutine(ControlarComportamientosNPC());
    }

    private void OnDisable()
    {
        ControladorEventos.Instancia.DesubscribirseEvento(typeof(EventoAniadirNPCAlControlador), AniadirNPC);
        StopAllCoroutines();
    }

    private void AniadirNPC(EventoBase mensaje)
    {
        EventoAniadirNPCAlControlador e = (EventoAniadirNPCAlControlador)mensaje;
        bool yaExisteElNPC = false;
        for (int i = 0; i < npcs.Count; i++)
        {
            if (npcs[i].gameObject.GetInstanceID() == e.NPC.gameObject.GetInstanceID())
            {
                yaExisteElNPC = true;
                break;
            }
        }
        if (!yaExisteElNPC)
            npcs.Add(e.NPC);
    }

    private IEnumerator ControlarComportamientosNPC()
    {
        while (true)
        {
            while (!Personaje.PuedeMoverse || npcs.Count == 0)
                yield return new WaitForSeconds(1);

            for (int i = 0; i < npcs.Count; i++)
            {
                if (npcs[i].gameObject.activeSelf)
                {
                    npcs[i].EjecutarComportamientoNPC();
                    if (!npcs[i].EstaCercaDeLaSiguienteCasilla())
                        moverNPC.Add(npcs[i]);
                }
                else
                    npcs.RemoveAt(i);
                yield return null;
            }

            while(moverNPC.Count > 0)
            {
                for (int i = 0; i < moverNPC.Count; i++)
                {
                    if (moverNPC[i].MoverDesdeControladorNPC())
                        moverNPC.RemoveAt(i);
                }
                yield return null;
            }

            yield return new WaitForSeconds(Ajustes.Instancia.segundosParaMoverNPC);
        }        
    }

    
}
