using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlesPersonaje : MonoBehaviour {

	public static Vector2 DireccionMovimiento { get; private set; }
    public static float velocidadMovimiento { get; private set; }
    public static bool botonCorrerPulsado = false;

    private Personaje personaje;

    private void Start()
    {
        personaje = GetComponent<Personaje>();
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoBotonMovilPulsadoAbrirMenuPrincipal), AbrirMenuPrincipal);
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoBotonMovilPulsadoInteractuar), Interactuar);
        velocidadMovimiento = Ajustes.Instancia.velocidadAndar;
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") > 0 
            || SimpleInput.GetAxis("Horizontal") > 0
            )
            DireccionMovimiento = Vector2.right;
        else if (Input.GetAxis("Horizontal") < 0 
            || SimpleInput.GetAxis("Horizontal") < 0
            )
            DireccionMovimiento = Vector2.left;
        else if (Input.GetAxis("Vertical") > 0 
            || SimpleInput.GetAxis("Vertical") > 0
            )
            DireccionMovimiento = Vector2.up;
        else if (Input.GetAxis("Vertical") < 0 
            || SimpleInput.GetAxis("Vertical") < 0
            )
            DireccionMovimiento = Vector2.down;
        else
            DireccionMovimiento = Vector2.zero;

        if (Personaje.PuedeMoverse && Input.GetKeyDown(Ajustes.Instancia.teclaAbrirMenu))
            ControladorEventos.Instancia.LanzarEvento(new EventoAbrirMenuPrincipal());

        if (Input.GetKeyDown(Ajustes.Instancia.teclaInteractuar))
            personaje.Interactuar();

        if (Personaje.PuedeMoverse && (Input.GetKey(Ajustes.Instancia.teclaCorrer) || botonCorrerPulsado))
        {
            velocidadMovimiento = Ajustes.Instancia.velocidadCorrer;
        }
        else
        {
            velocidadMovimiento = Ajustes.Instancia.velocidadAndar;
        }
    }
    
    private void AbrirMenuPrincipal(EventoBase e)
    {
        if (Personaje.PuedeMoverse)
            ControladorEventos.Instancia.LanzarEvento(new EventoAbrirMenuPrincipal());
    }

    private void Interactuar(EventoBase e)
    {
        personaje.Interactuar();
    }
}
