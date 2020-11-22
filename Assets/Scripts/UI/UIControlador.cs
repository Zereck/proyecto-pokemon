using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControlador : MonoBehaviour {
    
    public static UIControlador Instancia { get; private set; }

    //REFERENCIAS TELETRANSPORTADOR
    [System.Serializable]
    public struct CampoTeletransportador
    {
        public Teletransportador componentePrincipal;
        public Image pantallaDeCarga;
    }
    [Header("Teletransportador")]
    [SerializeField]
    private CampoTeletransportador _teletransportador;
    public CampoTeletransportador Teletransportador { get { return _teletransportador; } }


    //REFERENCIAS CONTROLADOR COMBATE
    [System.Serializable]
    public struct CampoCombate
    {
        public ControladorCombate componentePrincipal;
        public GameObject panelPrincipalUICombate;
        public Button botonEscapar;
        public Image barraSaludPokemonEnemigo;
        public Text textoSaludPokemonEnemigo;
        public GameObject panelBarraSaludPokemonEnemigo;
        public Text nombreYNivelPokemonEnemigo;
        public Image barraSaludPokemonJugador;
        public Text textoSaludPokemonJugador;
        public GameObject panelBarraSaludPokemonJugador;
        public Text nombreYNivelPokemonJugador;
        public VentanaSpritesCombate spritesCombate;
        public GameObject menuEleccionesCombate;
        public Image barraExperienciaJugador;
        public UIVentanaIncrementoEstadisticas ventanaIncrementoEstadisticas;
    }
    [Space(10)]
    [Header("Combate")]
    [SerializeField]
    private CampoCombate _combate;
    public CampoCombate Combate { get { return _combate; } }

    //REFERENCIAS COMBATE ATAQUES 
    [System.Serializable]
    public struct CampoCombateAtaques
    {
        public UIBotonAtaque botonAtaque1;
        public UIBotonAtaque botonAtaque2;
        public UIBotonAtaque botonAtaque3;
        public UIBotonAtaque botonAtaque4;
    }
    [SerializeField]
    private CampoCombateAtaques _combateAtaques;
    public CampoCombateAtaques CombateAtaques { get { return _combateAtaques; } }

    //REFERENCIAS A VENTANA EVOLUCION
    public VentanaSpritesEvolucion ventanaSpriteEvolucion;



    //MENU PRINCIPAL
    [Space(10)]
    [Header("Menú Principal")]
    [SerializeField]
    private GameObject menuPrincipal;

    //REFERENCIAS LISTA POKÉMON
    [System.Serializable]
    public struct CampoEquipoPokemon
    {
        public GameObject ventanaPrincipal;
        public GameObject botonCerrarVentana;
        public UIMenuPokemonElementoLista pokemon1;
        public UIMenuPokemonElementoLista pokemon2;
        public UIMenuPokemonElementoLista pokemon3;
        public UIMenuPokemonElementoLista pokemon4;
        public UIMenuPokemonElementoLista pokemon5;
        public UIMenuPokemonElementoLista pokemon6;
    }
    [SerializeField]
    private CampoEquipoPokemon _equipoPokemon;
    public CampoEquipoPokemon EquipoPokemon { get { return _equipoPokemon; } }

    //REFERENCIAS VENTANA DIÁLOGO
    [System.Serializable]
    public struct CampoDialogo
    {
        public UIVentanaDialogo componentePrincipal;
        public GameObject ventanaDialogo;
        public UIVentanaConfirmacion ventanConfirmacion;
        public Image ventanaPokemonPreview;
        public Text campoDeTexto;
        public GameObject iconoContinuarDialogo;
    }
    [SerializeField]
    private CampoDialogo _dialogo;
    public CampoDialogo Dialogo { get { return _dialogo; } }

    //REFERENCIAS DETALLES POKÉMON
    [System.Serializable]
    public struct CampoDetallesPokemon
    {
        public UIDetallesPokemon componentePrincipal;
        public Image imagen;
        public Text nombre;
        public Text elementos;
        public UIDetallesPokemonAtaque ataques1;
        public UIDetallesPokemonAtaque ataques2;
        public UIDetallesPokemonAtaque ataques3;
        public UIDetallesPokemonAtaque ataques4;
        public Text salud;
        public Text ataqueFisico;
        public Text defensaFisica;
        public Text ataqueMagico;
        public Text defensaMagica;
        public Text velocidad;
    }
    [SerializeField]
    private CampoDetallesPokemon _detallesPokemon;
    public CampoDetallesPokemon DetallesPokemon { get { return _detallesPokemon; } }

    //REFERENCIAS VENTANA POKEDEX
    [SerializeField]
    private GameObject ventanaPokedex;

    //REFERENCIAS DETALLES POKEDEX
    public UIPokemonPokedexDetalles pokedexDetalles;

    //REFERENCIAS VENTANA MOCHILA
    [System.Serializable]
    public struct CampoMochila
    {
        public GameObject ventanaPrincipal;
        public UIMochilaBotonesSecciones[] botonesSeccion;
        public GameObject[] VentanasItem;
    }
    [SerializeField]
    private CampoMochila _mochila;

    //REFERENCIAS VENTANA TIENDA    
    [SerializeField]
    private UITienda _tienda;
    public UITienda Tienda { get { return _tienda; } }

    //REFERENCIAS VENTANA APRENDER NUEVO ATAQUE    
    [SerializeField]
    private UIAprenderNuevoAtaqueVentana _aprenderAtaque;
    public UIAprenderNuevoAtaqueVentana AprenderAtaque { get { return _aprenderAtaque; } }


    //REFERENCIAS A LA VENTANA DE PC POKEMON  
    [SerializeField]
    private UIPCControlador uIPCControlador;
    public UIPCControlador PCControlador { get { return uIPCControlador; } }

    //OTROS
    [Space(10)]
    [Header("Otros Elementos")]
    [SerializeField]
    private GameObject panelBloqueadorDePulsaciones;

    //ÚLTIMO ITEM SELECCIONADO
    [HideInInspector]
    [System.NonSerialized]
    public ItemID ultimoItemSeleccionado;
        

    private void OnEnable()
    {
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoAbrirMenuPrincipal), AbrirMenuPrincipal);
    }

    private void OnDisable()
    {
        ControladorEventos.Instancia.DesubscribirseEvento(typeof(EventoAbrirMenuPrincipal), AbrirMenuPrincipal);
    }

    private void Awake()
    {
        if (Instancia == null)
            Instancia = this;
        else
            Destroy(gameObject);
    }


    private void AbrirMenuPrincipal(EventoBase mensaje)
    {
        Personaje.UIAbierta = true;
        Personaje.PuedeMoverse = false;
        menuPrincipal.gameObject.SetActive(true);
    }
    
    public void UIMenuPokemonElementoListaPulsado(UIMenuPokemonElementoLista uiElemento)
    {
        //Si antes de pulsar se seleccionó un item de la mochila...
        if (ultimoItemSeleccionado != ItemID.NINGUNO)
        {
            //... Bloqueamos la detección de pulsaciones hasta comprobar si se puede utilizar el objeto
            panelBloqueadorDePulsaciones.gameObject.SetActive(true);            
            //... Iniciamos la comprobación del objeto
            UsarItem(uiElemento);
        }
        //Si durante un combate se ha cambiado de pokémon (por derrota del pokémon actual o por cambio directo)...
        else if(ControladorCombate.DatosCombate != null && ControladorCombate.DatosCombate.CombateActivo)
        {
            //... Comprobamos si se ha pulsado en un pokémon vivo...
            if (uiElemento.PokemonDatos.EstaVivo() && ControladorCombate.DatosCombate.PokemonJugadorActivo().Pokemon.IdentificardorUnico != uiElemento.PokemonDatos.IdentificardorUnico)
            {
                //... Si lo ha hecho, lo marcamos como el pokémon activo del jugador
                ControladorEventos.Instancia.LanzarEvento(new EventoProximaAccionCombate(TipoAccion.CambiarPokemon, AtaqueID.NINGUNO, null, uiElemento.PokemonDatos));
                //... Y Desactivamos el objeto para que la corrutina principal del combate siga ejecutándose
                EquipoPokemon.ventanaPrincipal.SetActive(false);
            }
        }
        //Si no está en combate, ni se está cambiando de pokémon durante el combate y no se ha seleccionado un item antes, muestra los detalles del pokémon
        else
        {
            _detallesPokemon.componentePrincipal.MostrarDetallesPokemon(uiElemento.PokemonDatos);
        }
    }

    private void UsarItem(UIMenuPokemonElementoLista pokemon)
    {
        Item item = ControladorDatos.Instancia.ObtenerItem(ultimoItemSeleccionado);
        switch (item.tipoDeItem)
        {
            case TipoDeItem.Curacion:
                if(item.restaurarEstadoAlterado != EstadoAlterado.NINGUNO)
                    RestaurarEstadoAlterado(item, pokemon);
                if (item.cantidadSanacion > 0)
                    RestaurarSalud(item, pokemon);
                break;
            //case TipoDeItem.MT:
            //    //TODO: mostrar confirmación de si quiere enseñarle el ataque
            //    break;
        }
    }
        

    private void ItemUsadoConfirmado()
    {
        Item item = ControladorDatos.Instancia.ObtenerItem(ultimoItemSeleccionado);
        ControladorDatos.Instancia.Datos.ItemUsado(item.ID);
        if (ControladorCombate.DatosCombate != null && ControladorCombate.DatosCombate.CombateActivo)
        {
            EquipoPokemon.ventanaPrincipal.SetActive(false);
            ControladorEventos.Instancia.LanzarEvento(new EventoProximaAccionCombate(TipoAccion.UsarItem, AtaqueID.NINGUNO, item));
            ultimoItemSeleccionado = ItemID.NINGUNO;
        }
        else if (!ControladorDatos.Instancia.Datos.TieneItem(item.ID))
        {
            EquipoPokemon.ventanaPrincipal.SetActive(false);
            _mochila.ventanaPrincipal.SetActive(true);
            ultimoItemSeleccionado = ItemID.NINGUNO;
        }

        panelBloqueadorDePulsaciones.gameObject.SetActive(false);
    }
    
    private void RestaurarEstadoAlterado(Item item, UIMenuPokemonElementoLista pokemonLista)
    {
        if (pokemonLista.PokemonDatos.EstadoAlterado == item.restaurarEstadoAlterado)
        {
            pokemonLista.PokemonDatos.EstadoAlterado = EstadoAlterado.NINGUNO;
            pokemonLista.estadoAlterado.text = string.Empty;
            if (item.cantidadSanacion <= 0)
                ItemUsadoConfirmado();
        }
        else
        {
            panelBloqueadorDePulsaciones.gameObject.SetActive(false);
        }
    }

    private void RestaurarSalud(Item item, UIMenuPokemonElementoLista pokemonLista)
    {
        if (!pokemonLista.PokemonDatos.TieneLaSaludCompleta())
        {
            pokemonLista.PokemonDatos.RestaurarSalud(item.cantidadSanacion);
            float porcentajeDeSalud = pokemonLista.PokemonDatos.SaludEnEscalaDe1();
            StartCoroutine(CorrutinasComunes.MoverBarraDeslizadora(pokemonLista.barraSalud, porcentajeDeSalud, pokemonLista.PokemonDatos.EstadisticaSaludMaxima(), pokemonLista.textoSalud, ItemUsadoConfirmado));
        }
        else
        {
            panelBloqueadorDePulsaciones.gameObject.SetActive(false);
        }
    }
    
    public void MenuPrincipal_CerrarMenuPrincipal()
    {
        Personaje.UIAbierta = false;
        menuPrincipal.SetActive(false);
    }

    public void MenuPrincipal_AbrirMenuPokemons()
    {
        menuPrincipal.SetActive(false);
        EquipoPokemon.ventanaPrincipal.SetActive(true);
    }

    public void MenuPrincipal_AbrirMochila()
    {
        menuPrincipal.SetActive(false);
        _mochila.ventanaPrincipal.SetActive(true);
    }

    public void MenuMochila_CerrarMochila()
    {
        VolverAlMenuPrincipal(_mochila.ventanaPrincipal);
    }

    public void MenuPokemon_CerrarMenu()
    {
        VolverAlMenuPrincipal(EquipoPokemon.ventanaPrincipal);
    }

    private void VolverAlMenuPrincipal(GameObject menuActual)
    {
        //Desactivamos el menú actual
        menuActual.SetActive(false);
        //Si estamos en combate, activamos el menú del combate
        if (ControladorCombate.DatosCombate != null && ControladorCombate.DatosCombate.CombateActivo)
            Combate.menuEleccionesCombate.SetActive(true);
        //Si no está en combate pero estaba usando items en los pokemon
        else if (ultimoItemSeleccionado != ItemID.NINGUNO)
        {
            EquipoPokemon.ventanaPrincipal.SetActive(false);
            _mochila.ventanaPrincipal.SetActive(true);
            ultimoItemSeleccionado = ItemID.NINGUNO;
        }
        //Si no, activamos el menú principal
        else
            menuPrincipal.SetActive(true);
    }

    public void MenuPrincipal_AbrirPokedex()
    {
        menuPrincipal.SetActive(false);
        pokedexDetalles.gameObject.SetActive(false);
        ventanaPokedex.gameObject.SetActive(true);
    }

    public void MenuPokedex_CerrarPokedex()
    {
        pokedexDetalles.gameObject.SetActive(false);
        ventanaPokedex.gameObject.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    public void MenuMedallas_CerrarVentana()
    {
        pokedexDetalles.gameObject.SetActive(false);
        ventanaPokedex.gameObject.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    public void MenuMochila_BotonSeccionPulsado(UIMochilaBotonesSecciones boton, GameObject seccion)
    {
        for (int i = 0; i < _mochila.botonesSeccion.Length; i++)
        {
            if(_mochila.botonesSeccion[i].gameObject.GetInstanceID() != boton.gameObject.GetInstanceID())
                _mochila.botonesSeccion[i].RestablecerTamanio();
        }

        for (int i = 0; i < _mochila.VentanasItem.Length; i++)
        {
            _mochila.VentanasItem[i].SetActive(false);
        }
        seccion.gameObject.SetActive(true);

    }

    public void MenuMochila_ItemPulsado(Item item)
    {
        _mochila.ventanaPrincipal.gameObject.SetActive(false);
        panelBloqueadorDePulsaciones.SetActive(true);
        ultimoItemSeleccionado = item.ID;

        switch (item.tipoDeItem)
        {
            case TipoDeItem.Curacion:
            //case TipoDeItem.MT:
                MenuPrincipal_AbrirMenuPokemons();
                break;
            case TipoDeItem.Pokeball:
                if (ControladorCombate.DatosCombate != null && ControladorCombate.DatosCombate.CombateActivo)
                {
                    ControladorEventos.Instancia.LanzarEvento(new EventoProximaAccionCombate(TipoAccion.UsarItem, AtaqueID.NINGUNO, item));
                }
                break;
            case TipoDeItem.Otros:
                break;
        }
        panelBloqueadorDePulsaciones.SetActive(false);

    }

    public void GuardarPartida()
    {
        menuPrincipal.SetActive(false);
        ControladorEventos.Instancia.LanzarEvento(new EventoMostrarVentanaConfirmacion(AccionConfirmacion, AccionDenegar, Ajustes.Instancia.textoGuardarPartida, Ajustes.Instancia.textoPartidaGuardada, string.Empty, PokemonID.NINGUNO, true));
    }

    private void AccionConfirmacion()
    {
        ControladorDatos.Instancia.GuardarPartida();
        menuPrincipal.SetActive(true);
    }

    private void AccionDenegar()
    {
        menuPrincipal.SetActive(true);
    }

}