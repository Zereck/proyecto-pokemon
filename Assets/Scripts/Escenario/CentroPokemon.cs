using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentroPokemon : MonoBehaviour {

    public NPC npc;

#if UNITY_EDITOR

    private Vector2 direccionRayo;
    public bool mostrarGizmos = true;

    private void OnDrawGizmos()
    {
        if (mostrarGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.16f);
            Gizmos.color = Color.white;
        }        
    }
#endif

    
}
