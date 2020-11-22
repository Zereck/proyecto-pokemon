using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ControlesPersonaje))]
public class Personaje : MonoBehaviour, IMovible, ITeletransportable
{
    
    //PRIVATE
    private Vector2 siguienteDireccion;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Transform contenedorTransform;
    private int pasosHastaElProximoCombate;

    //PROPIEDADES
    public static bool UIAbierta { get; set; }
    public static bool BloquearMovimiento { get; private set; }
    public static bool PuedeMoverse { get; set; }
    public static bool ControladoExternamente { get; set; }
    public static IMovible MoverPersonaje { get; private set; }
    public static ITeletransportable TeletransportarPersonaje { get; private set; }
    public static Vector2 Posicion
    {
        get
        {
            return ControladorDatos.Instancia.Datos.posicion;
        }
        private set
        {
            ControladorDatos.Instancia.Datos.posicion = value;
        }
    }

    private void Start ()
    {
        contenedorTransform = transform.parent;
        if(Posicion == Vector2.zero)
            Posicion = contenedorTransform.position;
        else
            contenedorTransform.position = Posicion;

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        MoverPersonaje = this;
        TeletransportarPersonaje = this;
        Posicion = contenedorTransform.position;
        PuedeMoverse = true;
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoPersonajeMirarDireccion), NPCDetectaJugador);
        pasosHastaElProximoCombate = UnityEngine.Random.Range(Ajustes.Instancia.pasosMinimosHastaElProximoCombate, Ajustes.Instancia.pasosMaximosHastaElProximoCombate);
    }

    private void Update()
    {        
        if (!BloquearMovimiento)
        {
            if (PuedeMoverse)
            {
                AsignarSiguientePosicion();
            }
            //... Se mueve hasta su casilla objetivo y luego comprueba si no se puede mover, para así bloquear el movimiento dejar el personaje alineado
            MoverConControles();
        }
        else if (PuedeMoverse)
            BloquearMovimiento = false;
        else
            DetenerAnimacion();
    }
        

    private void AsignarSiguientePosicion()
    {
        if (EstaCercaDeLaSiguienteCasilla())
        {
            if (ControlesPersonaje.DireccionMovimiento == Vector2.zero)
                DetenerAnimacion();
            else if(PuedeMoverse)
            {
                siguienteDireccion = ControlesPersonaje.DireccionMovimiento;
                if (PuedeMoverseALaSiguienteCasilla())
                {
                    Posicion += (Ajustes.Instancia.tamanioCasilla * siguienteDireccion);
                    ComprobarCombatesAleatorios();
                }
            }
        }
    }

    private bool MoverConControles()
    {
        AsignarAnimacion();
        if (!EstaCercaDeLaSiguienteCasilla())
        {
            ReanudarAnimacion();
            contenedorTransform.position = Vector2.MoveTowards(contenedorTransform.position, Posicion, ControlesPersonaje.velocidadMovimiento * Time.deltaTime);
            return false;
        }
        else if (!PuedeMoverse)
        {
            DetenerAnimacion();
            BloquearMovimiento = true;
        }
        return true;
    }

    private bool PuedeMoverseALaSiguienteCasilla()
    {
        Vector2 origenAreaDeteccion = (Vector2)contenedorTransform.position + (siguienteDireccion * Ajustes.Instancia.tamanioCasilla);
        //Si en la dirección del próximo movimiento hay un collider del layer definido como obstáculo no se puede mover
        Collider2D hit = Physics2D.OverlapBox(origenAreaDeteccion, Ajustes.Instancia.TamanioAreaColisiones, 0, Ajustes.Instancia.layerColision);
        if (hit != null)
        {
            if ((hit.gameObject.CompareTag(Ajustes.Instancia.tagSueloSaltable) && origenAreaDeteccion.y < contenedorTransform.position.y))
            {
                ControladorDatos.Instancia.ReproducirSonido(SonidoID.SonidoSueloSaltable);
                Posicion += (Ajustes.Instancia.tamanioCasilla * 2 * siguienteDireccion);
                anim.SetBool(Ajustes.Instancia.nombreParametroAnimacionSaltar, true);
            }
            else
            {
                DetenerAnimacion();
            }
            return false;
        }
        return true;
    }

    private void ComprobarCombatesAleatorios()
    {
        Collider2D hit = Physics2D.OverlapBox(Posicion, Ajustes.Instancia.TamanioAreaColisiones, 0, Ajustes.Instancia.layerHierba);
        if (hit != null)
        {
            pasosHastaElProximoCombate--;
            if(pasosHastaElProximoCombate <= 0)
            {
                PokemonEnLaZona zonaHierba = hit.transform.parent.GetComponent<PokemonEnLaZona>();
                if(zonaHierba != null)
                {
                    ControladorEventos.Instancia.LanzarEvento(new EventoIniciarCombatePokemonSalvaje(zonaHierba));
                    pasosHastaElProximoCombate = UnityEngine.Random.Range(Ajustes.Instancia.pasosMinimosHastaElProximoCombate, Ajustes.Instancia.pasosMaximosHastaElProximoCombate);
                }                
            }
        }
    }
    

    public bool EstaCercaDeLaSiguienteCasilla()
    {
        if (Vector2.Distance(Posicion, contenedorTransform.position) < float.Epsilon)
        {
            anim.SetBool(Ajustes.Instancia.nombreParametroAnimacionSaltar, false);
            return true;
        }
        return false;
    }

    public void DetenerAnimacion()
    {
        if (anim.speed != 0)
        {            
            anim.speed = 0;
            anim.Play(anim.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
        }          
    }

    private void ReanudarAnimacion()
    {
        if (anim.speed != 1)
        {
            anim.speed = 1;
        }
    }

    private void AsignarAnimacion()
    {
        if (siguienteDireccion == Vector2.left)
        {
            spriteRenderer.flipX = true;
        }
        else if (siguienteDireccion == Vector2.right)
        {
            spriteRenderer.flipX = false;
        }
        if(siguienteDireccion != Vector2.zero)
        {            
            anim.SetFloat("DireccionX", siguienteDireccion.x);
            anim.SetFloat("DireccionY", siguienteDireccion.y);
        }            
    }

    public void CambiarPosicion(Vector2 nuevaPosicion, Vector2 direccionMirar)
    {
        contenedorTransform.position = nuevaPosicion;
        Posicion = contenedorTransform.position;

        if (direccionMirar != Vector2.zero)
        {
            siguienteDireccion = direccionMirar;
            ReanudarAnimacion();
            AsignarAnimacion();
        }
    }
    
    public void Interactuar()
    {
        if (!BloquearMovimiento && PuedeMoverse)
        {
            Vector2 origenAreaDeteccion = (Vector2)contenedorTransform.position + (siguienteDireccion * Ajustes.Instancia.tamanioCasilla);
            Collider2D[] hit = Physics2D.OverlapBoxAll(origenAreaDeteccion, Ajustes.Instancia.TamanioAreaColisiones, 0, Ajustes.Instancia.layerColision);
            if (hit != null && hit.Length > 0)
            {
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].gameObject.CompareTag(Ajustes.Instancia.tagInteraccion))
                    {
                        hit[i].gameObject.GetComponent<IInteractivo>().Interactuar(contenedorTransform.position);
                        break;
                    }
                }
            }           
        }
    }

    private void NPCDetectaJugador(EventoBase mensaje)
    {
        EventoPersonajeMirarDireccion e = (EventoPersonajeMirarDireccion)mensaje;
        siguienteDireccion = Herramientas.ObtenerDireccion(contenedorTransform.position, e.PosicionObjetivo);
        AsignarAnimacion();
        DetenerAnimacion();
    }
    
    public Transform Transform()
    {
        return contenedorTransform;
    }
    
    public TipoColision DetectarColisionesEnfrente(Vector2 direccion)
    {
        siguienteDireccion = direccion;
        if (!PuedeMoverseALaSiguienteCasilla())
            return TipoColision.ObjetoColision;
        return TipoColision.NINGUNO;
    }

    public bool MoverExternamente(Vector2 nuevaCasilla, Vector2 nuevaDireccion, bool estaSiguiendoAOtro = false)
    {
        if (estaSiguiendoAOtro)
        {
            PuedeMoverse = false;
            float distancia = Vector2.Distance(nuevaCasilla, contenedorTransform.position);
            if (distancia <= Ajustes.Instancia.TerceraParteTamanioCasilla)
            {
                siguienteDireccion = nuevaDireccion;
            }
        }
        else
        {
            siguienteDireccion = nuevaDireccion;
        }
        Posicion = nuevaCasilla;
        if (MoverConControles())
            return true;
        return false;
    }

    public Vector2 SiguienteDireccion()
    {
        return siguienteDireccion;
    }

    public Vector2 SiguientePosicion()
    {
        return Posicion;
    }
}
