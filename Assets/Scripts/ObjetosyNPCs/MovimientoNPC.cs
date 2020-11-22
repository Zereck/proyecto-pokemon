using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPC))]
public class MovimientoNPC : MonoBehaviour, IMovible {

    public Sprite andarArriba1;
    public Sprite andarArriba2;
    public Sprite andarAbajo1;
    public Sprite andarAbajo2;
    public Sprite andarDerecha1;
    public Sprite andarDerecha2;
    public TipoMovimientoNPC tipoMovimientoCuantoTengaCombate = TipoMovimientoNPC.GirarParaDetectarPersonaje;
    public TipoMovimientoNPC tipoMovimientoCuantoSoloHable = TipoMovimientoNPC.NINGUNO;
    [Range(0, 15)]
    public int distanciaDeteccionPersonajeEnNumeroCasillas;
    
    private TipoMovimientoNPC tipoMovimientoActual;
    private Vector2 siguienteDireccion;
    private Vector2 siguientePosicion;
    private SpriteRenderer spriteRenderer;
    private NPC npc;
    private bool personajeEntroEnElRayo;
    private bool haDadoPaso1 = false;
    private float temporizadorAnimacioAndar;
    private float temporizadorAnimacionParado;
    private bool estaAndando = false;
    private bool controladorNPCHaDadoPaso = false;

    public Vector2 PosicionInicial { get; private set; }

    public IMovible MoverNPC { get; private set; }

    private void OnEnable()
    {
        PosicionInicial = transform.position;
        if (npc == null)
            npc = GetComponent<NPC>();
        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        ComprobarDireccionInicial();
        ComprobarTipoMovimientoActual();

        if (tipoMovimientoActual == TipoMovimientoNPC.NINGUNO || !npc.TieneDialogoPendiente())
        {
            enabled = false;
        }

        else if(tipoMovimientoActual != TipoMovimientoNPC.QuietoObservandoAreaDeDeteccion)
        {
            ControladorEventos.Instancia.LanzarEvento(new EventoAniadirNPCAlControlador(this));
        }
        
        siguientePosicion = transform.position;
        MoverNPC = this;
    }

    private void ComprobarTipoMovimientoActual()
    {
        if (npc.TieneDialogoPendiente())
        {
            TipoConversacion ultimaConversacion = npc.TipoDeLaUltimaConversacion();
            if (ultimaConversacion == TipoConversacion.Hablar)
                tipoMovimientoActual = tipoMovimientoCuantoSoloHable;
            else
                tipoMovimientoActual = tipoMovimientoCuantoTengaCombate;
        }
        else
        {
            tipoMovimientoActual = TipoMovimientoNPC.NINGUNO;
        }
    }
        
    private void ComprobarDireccionInicial()
    {
        if(spriteRenderer.sprite.name == npc.mirarAbajo.name)
        {
            siguienteDireccion = Vector2.down;
        }
        else if (spriteRenderer.sprite.name == npc.mirarArriba.name)
        {
            siguienteDireccion = Vector2.up;
        }
        else if (spriteRenderer.sprite.name == npc.mirarDerecha.name)
        {
            if (spriteRenderer.flipX)
            {
                siguienteDireccion = Vector2.left;
            }
            else
            {
                siguienteDireccion = Vector2.right;
            }
        }
    }

    private void FixedUpdate()
    {
        if(tipoMovimientoActual == TipoMovimientoNPC.GirarParaDetectarPersonaje && Personaje.PuedeMoverse)
        {
            if (!personajeEntroEnElRayo && DetectarPersonajeADistancia(siguienteDireccion, distanciaDeteccionPersonajeEnNumeroCasillas) == TipoColision.Personaje)
            {
                personajeEntroEnElRayo = true;
                PersonajeDetectado();
            }
            else if (personajeEntroEnElRayo && DetectarPersonajeADistancia(siguienteDireccion, distanciaDeteccionPersonajeEnNumeroCasillas) != TipoColision.Personaje)
            {
                personajeEntroEnElRayo = false;
            }
        }        
    }

    public void EjecutarComportamientoNPC()
    {
        if (tipoMovimientoActual == TipoMovimientoNPC.MoversePorZona)
        {
            if (RealizarAccion())
            {
                DireccionAleatoria();
                Girar();
            }
            if (RealizarAccion())
            {
                if (DetectarColisionesEnfrente(siguienteDireccion) == TipoColision.NINGUNO)
                    siguientePosicion = (Vector2)transform.position + (siguienteDireccion * Ajustes.Instancia.tamanioCasilla);
            }
        }
        else if (tipoMovimientoActual == TipoMovimientoNPC.GirarParaDetectarPersonaje)
        {
            if (RealizarAccion())
            {
                DireccionAleatoria();
                Girar();
            }
        }
    }

    private void Girar()
    {
        AnimacionParado(siguienteDireccion);
    }       

    private void DireccionAleatoria()
    {
        int direccion = Random.Range(1, 5);
        switch (direccion)
        {
            case 1:
                siguienteDireccion = Vector2.up;
                break;
            case 2:
                siguienteDireccion = Vector2.right;
                break;
            case 3:
                siguienteDireccion = Vector2.left;
                break;
            default:
                siguienteDireccion = Vector2.down;
                break;
        }
    }

    private bool RealizarAccion()
    {
        if (Random.Range(0f, 1f) <= 0.5f)
            return true;
        return false;
    }

    public void PersonajeDetectado()
    {
        Personaje.PuedeMoverse = false;
        StopAllCoroutines();
        ControladorDatos.Instancia.AniadirCorrutinaACola(CorrutinasComunes.MoverHastaPosicion(this, Personaje.Posicion));
        npc.MostrarDialogo();
    }

    public bool MoverDesdeControladorNPC()
    {
        if (EstaCercaDeLaSiguienteCasilla())
        {
            controladorNPCHaDadoPaso = false;
            AnimacionParado(siguienteDireccion);
            return true;
        }            
        else
        {
            if (!controladorNPCHaDadoPaso && Vector2.Distance(siguientePosicion, transform.position) < Ajustes.Instancia.TerceraParteTamanioCasilla)
            {
                controladorNPCHaDadoPaso = true;
                estaAndando = false;
                AnimacionPaso(siguienteDireccion);
            }                
            transform.position = Vector2.MoveTowards(transform.position, siguientePosicion, Ajustes.Instancia.velocidadAndar * Time.deltaTime);
            return false;
        }
    }

    public bool MoverExternamente(Vector2 nuevaCasilla, Vector2 direccion, bool estaSiguiendoAOtro = false)
    {
        siguientePosicion = nuevaCasilla;
        //Cuando el objeto está siguiendo a otro, para evitar giros en las animaciones demasiado pronto se asignará la nueva dirección al estar cerca de la casilla objetivo
        if(!estaSiguiendoAOtro)
            siguienteDireccion = direccion;

        float distancia = Vector2.Distance(siguientePosicion, transform.position);

        if (distancia <= Ajustes.Instancia.TerceraParteTamanioCasilla)
        {
            temporizadorAnimacioAndar = 0;
            temporizadorAnimacionParado += Time.deltaTime;
            if(temporizadorAnimacionParado > 0.4f)
            {
                temporizadorAnimacionParado = 0;
                AnimacionParado(direccion);
            }
            siguienteDireccion = direccion;
        }
        else
        {
           
            temporizadorAnimacionParado = 0;
            temporizadorAnimacioAndar += Time.deltaTime;
            if(temporizadorAnimacioAndar > ((Ajustes.Instancia.tamanioCasilla * Ajustes.Instancia.velocidadAnimacionNPC) / Ajustes.Instancia.velocidadAndar))
            {
                temporizadorAnimacioAndar = 0;
                AnimacionPaso(siguienteDireccion);
            }
        }

        if (EstaCercaDeLaSiguienteCasilla())
            return true;
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, nuevaCasilla, Ajustes.Instancia.velocidadAndar * Time.deltaTime);
            return false;
        }
    }

    public bool EstaCercaDeLaSiguienteCasilla()
    {
        if (Vector2.Distance(siguientePosicion, transform.position) > float.Epsilon)
            return false;
        return true;
    }

    public Transform Transform()
    {
        return transform;
    }

    private void AnimacionPaso(Vector2 direccion)
    {
        if(estaAndando)
        {
            estaAndando = false;
            AnimacionParado(direccion);
        }
        else
        {
            estaAndando = true;

            Sprite paso1;
            Sprite paso2;

            if (direccion == Vector2.down)
            {
                paso1 = andarAbajo1;
                paso2 = andarAbajo2;
            }
            else if (direccion == Vector2.up)
            {
                paso1 = andarArriba1;
                paso2 = andarArriba2;
            }
            else if (direccion == Vector2.right)
            {
                paso1 = andarDerecha1;
                paso2 = andarDerecha2;
                spriteRenderer.flipX = false;
            }
            else
            {
                paso1 = andarDerecha1;
                paso2 = andarDerecha2;
                spriteRenderer.flipX = true;
            }

            if (haDadoPaso1)
                spriteRenderer.sprite = paso2;
            else
                spriteRenderer.sprite = paso1;
            haDadoPaso1 = !haDadoPaso1;
        }
        


    }

    private void AnimacionParado(Vector2 direccion)
    {
        if (direccion == Vector2.right)
        {
            spriteRenderer.sprite = npc.mirarDerecha;
            spriteRenderer.flipX = false;
        }
        else if (direccion == Vector2.left)
        {
            spriteRenderer.sprite = npc.mirarDerecha;
            spriteRenderer.flipX = true;
        }
        else if (direccion == Vector2.down)
        {
            spriteRenderer.sprite = npc.mirarAbajo;
        }
        else if (direccion == Vector2.up)
        {
            spriteRenderer.sprite = npc.mirarArriba;
        }
    }

    public TipoColision DetectarColisionesEnfrente(Vector2 direccion)
    {
        Vector2 origenAreaDeteccion = (Vector2)transform.position + (direccion * Ajustes.Instancia.tamanioCasilla);

        Collider2D[] objetoDelante = Physics2D.OverlapBoxAll(origenAreaDeteccion, Ajustes.Instancia.TamanioAreaColisiones, 0);
        
        if (objetoDelante != null && objetoDelante.Length > 0)
        {
            for (int i = 0; i < objetoDelante.Length; i++)
            {
                if (Herramientas.LayerSonIguales(objetoDelante[i].gameObject.layer, Ajustes.Instancia.layerColision))
                    return TipoColision.ObjetoColision;
                else if (objetoDelante[i].gameObject.CompareTag(Ajustes.Instancia.tagPersonaje))
                    return TipoColision.Personaje;
            }            
        }
        return TipoColision.NINGUNO;
    }

    private TipoColision DetectarPersonajeADistancia(Vector2 direccion, int distanciaRayoEnNumeroDeCasillas = 0)
    {
        float distanciaRayoFinal = Ajustes.Instancia.tamanioCasilla;
        if (distanciaRayoEnNumeroDeCasillas > 1)
            distanciaRayoFinal = Ajustes.Instancia.tamanioCasilla * distanciaRayoEnNumeroDeCasillas;

        RaycastHit2D[] colisiones = Physics2D.RaycastAll(transform.position, direccion, distanciaRayoFinal);
        if (colisiones != null && colisiones.Length > 0)
        {
            for (int i = 0; i < colisiones.Length; i++)
            {
                if (colisiones[i].collider.gameObject != gameObject)
                {
                    if (Herramientas.LayerSonIguales(colisiones[i].collider.gameObject.layer, Ajustes.Instancia.layerColision))
                        return TipoColision.ObjetoColision;
                    else if (colisiones[i].collider.gameObject.CompareTag(Ajustes.Instancia.tagPersonaje))
                        return TipoColision.Personaje;
                }
            }
        }
        return TipoColision.NINGUNO;
    }

    public void DetenerAnimacion()
    {
        AnimacionParado(siguienteDireccion);
    }

    public Vector2 SiguienteDireccion()
    {
        return siguienteDireccion;
    }

    public Vector2 SiguientePosicion()
    {
        return siguientePosicion;
    }

#if UNITY_EDITOR
    public bool mostrarLineaGizmos = true;

    private void OnDrawGizmos()
    {
        if (mostrarLineaGizmos)
        {
            if (tipoMovimientoCuantoSoloHable == TipoMovimientoNPC.GirarParaDetectarPersonaje || tipoMovimientoCuantoTengaCombate == TipoMovimientoNPC.GirarParaDetectarPersonaje)
            {
                float distanciaRayoFinal = Ajustes.Instancia.tamanioCasilla;
                if (distanciaDeteccionPersonajeEnNumeroCasillas > 1)
                    distanciaRayoFinal = Ajustes.Instancia.tamanioCasilla * distanciaDeteccionPersonajeEnNumeroCasillas;
                Gizmos.DrawLine(transform.position, ((Vector2)transform.position + (EDITOR_DireccionActual() * distanciaRayoFinal)));

            }
        }       

    }

    private Vector2 EDITOR_DireccionActual()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (npc == null)
            npc = GetComponent<NPC>();


        if (spriteRenderer.sprite.name == npc.mirarAbajo.name)
            return Vector2.down;
        else if (spriteRenderer.sprite.name == npc.mirarArriba.name)
            return Vector2.up;
        else
        {
            if (spriteRenderer.flipX)
            {
                return Vector2.left;
            }
            else
            {
                return Vector2.right;
            }
        }
    }

#endif

}