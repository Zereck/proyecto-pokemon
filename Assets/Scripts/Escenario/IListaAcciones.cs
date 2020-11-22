using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public delegate void DelegateAccionInicial(Action a);

public class IListaAcciones : MonoBehaviour {

    [HideInInspector]
    public List<Accion> secuenciaDeAcciones;
    
    private Accion accionActual;   

    public void EjecutarAccion()
    {
        Conversacion ultimaConversacion = null;

        for (int i = 0; i < secuenciaDeAcciones.Count; i++)
        {
            accionActual = secuenciaDeAcciones[i];
            
            if (accionActual.npc != null && !accionActual.npc.gameObject.activeSelf)
                accionActual.npc.gameObject.SetActive(true);
            switch (accionActual.TipoDeAccion)
            {
                case AccionSequencia.NPC_MoverAPosicionEspecifica:
                    ControladorDatos.Instancia.AniadirCorrutinaACola(CorrutinasComunes.MoverHastaPosicion(accionActual.npc.MoverNPC, accionActual.posicion.position));
                    break;
                case AccionSequencia.NPC_MoverASuPosicionInicial:
                    ControladorDatos.Instancia.AniadirCorrutinaACola(CorrutinasComunes.MoverHastaPosicion(accionActual.npc.MoverNPC, accionActual.npc.PosicionInicial));
                    break;
                case AccionSequencia.NPC_MoverHastaElPersonaje:
                    ControladorDatos.Instancia.AniadirCorrutinaACola(CorrutinasComunes.MoverHastaPosicion(accionActual.npc.MoverNPC, Personaje.Posicion));
                    break;
                case AccionSequencia.NPC_MostrarConversacion:
                    NPC npc = accionActual.npc.GetComponent<NPC>();
                    if (npc != null && npc.TieneDialogoPendiente())
                    {
                        ultimaConversacion = npc.AniadirDialogoASecuencia(ultimaConversacion);
                    }
                    break;
                case AccionSequencia.NPC_Ocultar:
                    ControladorDatos.Instancia.AniadirCorrutinaACola(DesactivarNPC(accionActual.npc.gameObject));
                    break;
                case AccionSequencia.Personaje_MoverAPosicionEspecífica:
                    ControladorDatos.Instancia.AniadirCorrutinaACola(CorrutinasComunes.MoverHastaPosicion(Personaje.MoverPersonaje, accionActual.posicion.position));
                    break;
                case AccionSequencia.Personaje_TeletransportarAPosicionEspecifica:
                    ControladorEventos.Instancia.LanzarEvento(new EventoTeletransportarse(accionActual.posicion.position, accionActual.direccion));
                    break;
                case AccionSequencia.Personaje_SeguirNPC:
                    ControladorDatos.Instancia.AniadirCorrutinaACola(CorrutinasComunes.SeguirObjeto(Personaje.MoverPersonaje, accionActual.npc.MoverNPC), false, "Seguir");
                    break;
                case AccionSequencia.Personaje_DejarDeSeguirNPC:
                    ControladorDatos.Instancia.AniadirCorrutinaACola(DetenerSeguimiento("Seguir"), false);
                    break;
                case AccionSequencia.EjecutarMetodos:
                    ControladorDatos.Instancia.AniadirCorrutinaACola(EjecutarEventoUnity(accionActual.eventoUnity));
                    break;
                case AccionSequencia.EsperarTiempo:
                    ControladorDatos.Instancia.AniadirCorrutinaACola(DetenerTiempo(accionActual.tiempoDeEspera));
                    break;
                case AccionSequencia.ReproducirAnimacion:
                    ControladorDatos.Instancia.AniadirCorrutinaACola(ReproducirAnimacion(accionActual.animator, accionActual.nombreAnimacionODialogo));
                    break;
                case AccionSequencia.CurarEquipoPokemon:
                    ControladorDatos.Instancia.AniadirCorrutinaACola(CentroPokemon());
                    break;
                case AccionSequencia.MostrarDialogoEspecifico:
                    ControladorDatos.Instancia.AniadirCorrutinaACola(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(accionActual.nombreAnimacionODialogo, true));
                    break;
            }
        }

        ControladorDatos.Instancia.AniadirCorrutinaACola(Fin(), false);
    }

    private IEnumerator Fin()
    {
        ControladorDatos.Instancia.DetenerCorrutina("Seguir");
        Personaje.MoverPersonaje.Transform().gameObject.GetComponent<Collider2D>().enabled = true;
        yield return null;
    }

    private IEnumerator DesactivarNPC(GameObject npc)
    {
        npc.SetActive(false);
        yield return null;
    }

    private IEnumerator ReproducirAnimacion(Animator animador, string animacion)
    {
        if(animador != null && animador.runtimeAnimatorController.animationClips.Length > 0)
        {
            animador.Play(animacion);
            while (animador.GetCurrentAnimatorStateInfo(0).IsName(animacion) && animador.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                yield return null;
        }
    }

    private IEnumerator CentroPokemon()
    {
        ControladorDatos.Instancia.Datos.CentroPokemon();
        yield return null;
    }

    private IEnumerator DetenerSeguimiento(string id)
    {
        ControladorDatos.Instancia.DetenerCorrutina(id);
        yield return null;
    }

    private IEnumerator EjecutarEventoUnity(UnityEvent e)
    {
        e.Invoke();
        yield return new WaitForSeconds(0.1f);
    }
    private IEnumerator DetenerTiempo(float tiempoDeEspera)
    {
        yield return new WaitForSeconds(tiempoDeEspera);
    }

    private IEnumerator EjecutarTeletransporte(Vector2 posicionObjetivo, Direccion direccion)
    {
        yield return new WaitForSeconds(0.1f);
    }
    

#if UNITY_EDITOR

    [HideInInspector]
    public bool mostrarGizmos = true;
    [HideInInspector]
    public float tamanioGizmos = 0.08f;

    private void OnDrawGizmos()
    {

        if (mostrarGizmos && secuenciaDeAcciones.Count > 0)
        {
            for (int i = 0; i < secuenciaDeAcciones.Count; i++)
            {
                Gizmos.color = Color.red;
                switch (secuenciaDeAcciones[i].TipoDeAccion)
                {
                    case AccionSequencia.Personaje_SeguirNPC:
                        if (secuenciaDeAcciones[i].npc != null)
                            Gizmos.DrawLine(transform.position, secuenciaDeAcciones[i].npc.transform.position);
                        break;
                    case AccionSequencia.NPC_MoverAPosicionEspecifica:
                        if (secuenciaDeAcciones[i].npc != null)
                            Gizmos.DrawLine(transform.position, secuenciaDeAcciones[i].npc.transform.position);
                        if (secuenciaDeAcciones[i].posicion != null)
                        {
                            Gizmos.color = Color.green;
                            Gizmos.DrawLine(secuenciaDeAcciones[i].npc.transform.position, secuenciaDeAcciones[i].posicion.position);
                            Gizmos.DrawWireSphere(secuenciaDeAcciones[i].posicion.position, tamanioGizmos);
                        }

                        break;
                    case AccionSequencia.NPC_MoverASuPosicionInicial:
                        if (secuenciaDeAcciones[i].npc != null)
                            Gizmos.DrawLine(transform.position, secuenciaDeAcciones[i].npc.transform.position);
                        break;
                    case AccionSequencia.NPC_MoverHastaElPersonaje:
                        if (secuenciaDeAcciones[i].npc != null)
                            Gizmos.DrawLine(transform.position, secuenciaDeAcciones[i].npc.transform.position);
                        break;
                    case AccionSequencia.NPC_MostrarConversacion:
                        if (secuenciaDeAcciones[i].npc != null)
                            Gizmos.DrawLine(transform.position, secuenciaDeAcciones[i].npc.transform.position);
                        break;
                    case AccionSequencia.NPC_Ocultar:
                        if (secuenciaDeAcciones[i].npc != null)
                            Gizmos.DrawLine(transform.position, secuenciaDeAcciones[i].npc.transform.position);
                        break;
                    case AccionSequencia.Personaje_MoverAPosicionEspecífica:
                        if (secuenciaDeAcciones[i].posicion != null)
                        {
                            Gizmos.DrawLine(transform.position, secuenciaDeAcciones[i].posicion.position);
                            Gizmos.color = Color.green;
                            Gizmos.DrawWireSphere(secuenciaDeAcciones[i].posicion.position, tamanioGizmos);
                        }
                        break;
                    case AccionSequencia.Personaje_TeletransportarAPosicionEspecifica:
                        if (secuenciaDeAcciones[i].posicion != null)
                        {
                            Gizmos.DrawLine(transform.position, secuenciaDeAcciones[i].posicion.position);
                            Gizmos.color = Color.green;
                            Gizmos.DrawWireSphere(secuenciaDeAcciones[i].posicion.position, tamanioGizmos);
                        }
                        break;
                }
            }
        }

    }
#endif
}
