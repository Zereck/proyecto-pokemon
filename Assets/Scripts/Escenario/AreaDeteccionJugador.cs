using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AreaDeteccionJugador : MonoBehaviour
{
    private List<SecuenciaDeAcciones> secuenciaDeAcciones;

    private void Start()
    {
        secuenciaDeAcciones = new List<SecuenciaDeAcciones>();
        for (int i = 0; i < transform.childCount; i++)
        {
            SecuenciaDeAcciones s = transform.GetChild(i).GetComponent<SecuenciaDeAcciones>();
            if (s != null)
                secuenciaDeAcciones.Add(s);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Ajustes.Instancia.tagPersonaje))
        {
            for (int i = 0; i < secuenciaDeAcciones.Count; i++)
            {
                if (secuenciaDeAcciones[i].CumpleSusCondiciones())
                {
                    Personaje.PuedeMoverse = false;
                    secuenciaDeAcciones[i].EjecutarAccion();
                    break;
                }
            }
        }
    }


#if UNITY_EDITOR

    [HideInInspector]
    public BoxCollider2D collider;
    public bool mostrarGizmos = true;
    [Range(0.01f, 2f)]
    public float tamanioGizmos = 0.08f;
    
    private void OnDrawGizmos()
    {
        if (collider == null)
            collider = GetComponent<BoxCollider2D>();

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + collider.offset.x, transform.position.y + collider.offset.y), collider.size);

    }
#endif
}
