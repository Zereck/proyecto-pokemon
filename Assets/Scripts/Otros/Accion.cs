using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Accion {

    public MovimientoNPC npc;
    public AccionSequencia TipoDeAccion;
    public Direccion direccion;
    public Transform posicion;
    public UnityEvent eventoUnity;
    public float tiempoDeEspera;
    public Animator animator;
    public string nombreAnimacionODialogo;
}
