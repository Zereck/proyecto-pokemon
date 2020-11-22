using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

public class ControladorCombate : MonoBehaviour
{    
    public static DatosCombate DatosCombate { get; private set; }
    
    private TablaTipos tablaTipos = new TablaTipos();
    private EventoIniciarCombatePokemonSalvaje pokemonSalvaje;
    private EventoIniciarCombateContraEntrenador entrenador;
    private EventoProximaAccionCombate proximaAccion;
    private GameObject panelPrincipalUICombate;
    private Button botonEscapar;
    private Image barraSaludPokemonEnemigo;
    private Text textoSaludPokemonEnemigo;
    private GameObject panelBarraSaludPokemonEnemigo;
    private Text nombreYNivelPokemonEnemigo;
    private Image barraSaludPokemonJugador;
    private Text textoSaludPokemonJugador;
    private GameObject panelBarraSaludPokemonJugador;
    private Text nombreYNivelPokemonJugador;
    private VentanaSpritesCombate spritesCombate;
    private Image pantallaDeCarga;
    private GameObject menuEleccionesCombate;
    private Image barraExperienciaJugador;

    private void OnEnable()
    {
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoIniciarCombatePokemonSalvaje), IniciarCombateContraPokemonSalvaje);
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoIniciarCombateContraEntrenador), IniciarCombateContraEntrenador);
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoProximaAccionCombate), ProximaAccionSeleccionada);
    }

    private void OnDisable()
    {
        ControladorEventos.Instancia.DesubscribirseEvento(typeof(EventoIniciarCombatePokemonSalvaje), IniciarCombateContraPokemonSalvaje);
        ControladorEventos.Instancia.DesubscribirseEvento(typeof(EventoIniciarCombateContraEntrenador), IniciarCombateContraEntrenador);
        ControladorEventos.Instancia.DesubscribirseEvento(typeof(EventoProximaAccionCombate), ProximaAccionSeleccionada);
    }

    private void Start()
    {
        panelPrincipalUICombate = UIControlador.Instancia.Combate.panelPrincipalUICombate;
        botonEscapar = UIControlador.Instancia.Combate.botonEscapar;
        barraSaludPokemonEnemigo = UIControlador.Instancia.Combate.barraSaludPokemonEnemigo;
        textoSaludPokemonEnemigo = UIControlador.Instancia.Combate.textoSaludPokemonEnemigo;
        panelBarraSaludPokemonEnemigo = UIControlador.Instancia.Combate.panelBarraSaludPokemonEnemigo;
        nombreYNivelPokemonEnemigo = UIControlador.Instancia.Combate.nombreYNivelPokemonEnemigo;
        barraSaludPokemonJugador = UIControlador.Instancia.Combate.barraSaludPokemonJugador;
        textoSaludPokemonJugador = UIControlador.Instancia.Combate.textoSaludPokemonJugador;
        panelBarraSaludPokemonJugador = UIControlador.Instancia.Combate.panelBarraSaludPokemonJugador;
        nombreYNivelPokemonJugador = UIControlador.Instancia.Combate.nombreYNivelPokemonJugador;
        spritesCombate = UIControlador.Instancia.Combate.spritesCombate;
        pantallaDeCarga = UIControlador.Instancia.Teletransportador.pantallaDeCarga;
        menuEleccionesCombate = UIControlador.Instancia.Combate.menuEleccionesCombate;
        barraExperienciaJugador = UIControlador.Instancia.Combate.barraExperienciaJugador;
    }

    private void IniciarCombateContraEntrenador(EventoBase mensaje)
    {
        if (DatosCombate == null)
        {
            entrenador = (EventoIniciarCombateContraEntrenador)mensaje;
            DatosCombate = new DatosCombate();
            botonEscapar.interactable = false;
            ControladorDatos.Instancia.AniadirCorrutinaACola(CombateCorrutina(TipoDeCombate.Entrenador));
        }
        
    }

    private void IniciarCombateContraPokemonSalvaje(EventoBase mensaje)
    {
        if (DatosCombate == null)
        {
            DatosCombate = new DatosCombate();
            botonEscapar.interactable = true;
            pokemonSalvaje = (EventoIniciarCombatePokemonSalvaje)mensaje;
            ControladorDatos.Instancia.AniadirCorrutinaACola(CombateCorrutina(TipoDeCombate.PokemonSalvaje));
        }           
    }

    private void ProximaAccionSeleccionada(EventoBase mensaje)
    {
        proximaAccion = (EventoProximaAccionCombate)mensaje;
    }


    private IEnumerator CombateCorrutina(TipoDeCombate tipoCombate)
    {
        ControladorDatos.Instancia.ReproducirMusicaCombate();
        DatosCombate.CombateActivo = true;

        //*********************
        // TEXTO INICIAL DEL ENTRENADOR ANTES DEL COMBATE
        //*********************
        if (tipoCombate == TipoDeCombate.Entrenador)
            yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(entrenador.Conversacion.texto, false, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));

        //*********************
        // TRANSICION A OSCURO
        //*********************
        yield return StartCoroutine(CorrutinasComunes.AlfaDeCeroAUno(pantallaDeCarga));

        //*********************
        // ESTABLECE VALORES POR DEFECTO
        //*********************
        //Rotamos la cámara para evitar problemas con otros sprites
        Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y + 180, Camera.main.transform.rotation.eulerAngles.z);
        panelBarraSaludPokemonJugador.SetActive(false);
        panelBarraSaludPokemonEnemigo.SetActive(false);
        menuEleccionesCombate.SetActive(false);
        UIControlador.Instancia.Dialogo.ventanaDialogo.SetActive(false);
        UIControlador.Instancia.Dialogo.campoDeTexto.text = string.Empty;
        panelPrincipalUICombate.SetActive(true);

        //*********************
        // PREPARAR EQUIPOS DE POKEMON LUCHADORES
        //*********************
        DatosCombate = new DatosCombate();
        if (tipoCombate == TipoDeCombate.PokemonSalvaje)
            DatosCombate.PrepararCombateContraPokemonSalvaje(pokemonSalvaje);
        else
            DatosCombate.PrepararCombateContraEntrenador(entrenador);


        //*********************
        // ASIGNA LOS SPRITES RENDERER Y RELLENA LAS BARRAS DE VIDA
        //*********************
        spritesCombate.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        RellenarInterfazConDatosDelPokemonActivo(DatosCombate.PokemonJugadorActivo(), barraSaludPokemonJugador, nombreYNivelPokemonJugador, textoSaludPokemonJugador);
        
        yield return new WaitForSeconds(0.05f);
        RellenarInterfazConDatosDelPokemonActivo(DatosCombate.PokemonEnemigoActivo(), barraSaludPokemonEnemigo, nombreYNivelPokemonEnemigo, textoSaludPokemonEnemigo, false);


        //*********************
        //  TRANSICION A CLARO
        //*********************
        yield return StartCoroutine(CorrutinasComunes.AlfaDeUnoACero(pantallaDeCarga));

        ////*********************
        ////  MUESTRA MENSAJE DE ENTRENADOR O POKÉMON SALVaJE
        ////*********************
        if (tipoCombate == TipoDeCombate.PokemonSalvaje)
        {
            string texto = Ajustes.Instancia.textoPokemonSalvajeAparecio.Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, DatosCombate.PokemonEnemigoActivo().Pokemon.DatosFijos.nombre);
            yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, true, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));
            while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.PokemonSalvajeAparece))
                yield return null;
        }
        else
        {
            string texto = Ajustes.Instancia.textoEntrenadorTeDesafia.Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, entrenador.NombreNPC);
            yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, false, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));
            texto = Ajustes.Instancia.textoEntrenadorEnviaPokemon
                .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, entrenador.NombreNPC)
                .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, DatosCombate.PokemonEnemigoActivo().Pokemon.DatosFijos.nombre);
            yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, false, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));
            while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.EntrenadorEnviaPokemon))
                yield return null;
        }

        panelBarraSaludPokemonEnemigo.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        string texto2 = Ajustes.Instancia.textoJugadorEnviaPokemon.Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, DatosCombate.PokemonJugadorActivo().Pokemon.DatosFijos.nombre);
        yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto2, true, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));
        while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.JugadorEnviaPokemon))
            yield return null;
        panelBarraSaludPokemonJugador.SetActive(true);
        yield return new WaitForSeconds(1f);
        menuEleccionesCombate.SetActive(true);

        //*********************
        //  BUCLE HASTA FINAL DE COMBATE (ENEMIGOS DERROTADOS, JUGADOR DERROTADO O ESCAPE)
        //*********************
        IEnumerator turno1 = TurnoJugadorCorrutina();
        IEnumerator turno2 = TurnoOponenteCorrutina();
        while (DatosCombate.CombateActivo)
        {
            //*********************
            //  ESPERA ACCIÓN DEL JUGADOR
            //*********************
            UIControlador.Instancia.Dialogo.campoDeTexto.text = string.Empty;
            UIControlador.Instancia.Dialogo.ventanaDialogo.SetActive(false);
            menuEleccionesCombate.SetActive(true);
            while (proximaAccion == null)
            {
                yield return null;
            }
            menuEleccionesCombate.SetActive(false);

            //*********************
            //  COMPROBAR EL TIPO DE ACCIÓN
            //*********************
            switch (proximaAccion.TipoAccion)
            {
                case TipoAccion.Atacar:
                    if(DatosCombate.PokemonJugadorActivo().Pokemon.EstadisticaVelocidadCombate() >= DatosCombate.PokemonEnemigoActivo().Pokemon.EstadisticaVelocidadCombate())
                    {
                        Debug.Log("Combate: el jugador ataca primero");
                        turno1 = TurnoJugadorCorrutina();
                        turno2 = TurnoOponenteCorrutina();
                    }
                    else
                    {
                        Debug.Log("Combate: el enemigo ataca primero");
                        turno1 = TurnoOponenteCorrutina();
                        turno2 = TurnoJugadorCorrutina();
                    }                    
                    break;
                case TipoAccion.UsarItem:
                case TipoAccion.Escapar:
                case TipoAccion.CambiarPokemon:
                    turno1 = TurnoJugadorCorrutina();
                    turno2 = TurnoOponenteCorrutina();
                    break;
            }
            yield return StartCoroutine(turno1);
            yield return StartCoroutine(turno2);
            yield return StartCoroutine(ComprobarPokemonVivos());
            proximaAccion = null;
        }

        //*********************
        //  FIN DEL COMBATE, COMPROBAR SI HA GANADO
        //*********************
        if (DatosCombate.CombateGanado)
        {
            if (tipoCombate == TipoDeCombate.Entrenador)
            {
                string texto = Ajustes.Instancia.textoEntrenadorVencido
                                .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, entrenador.NombreNPC)
                                .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, entrenador.Conversacion.monedasRecompensa.ToString());
                yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, false, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));
            }            
        }
        else
        {
            ControladorDatos.Instancia.Datos.CombatePerdidoQuitarMitadMonedas();
            string texto = Ajustes.Instancia.textoCombatePerdido
                                .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, ControladorDatos.Instancia.Datos.Monedas.ToString());
            yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, false, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));
        }

        //*********************
        // TRANSICION A OSCURO
        //*********************
        yield return StartCoroutine(CorrutinasComunes.AlfaDeCeroAUno(pantallaDeCarga));

        //*********************
        // VALORES POR DEFECTO
        //*********************
        Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y - 180, Camera.main.transform.rotation.eulerAngles.z);
        UIControlador.Instancia.Dialogo.ventanaDialogo.SetActive(false);
        UIControlador.Instancia.Dialogo.campoDeTexto.text = string.Empty;
        panelPrincipalUICombate.SetActive(false);
        spritesCombate.gameObject.SetActive(false);

        //*********************
        // SI HA PERDIDO LO ENVIAMOS AL CENTRO POKÉMON DEFINIDO EN LA ZONA
        //*********************
        if (!DatosCombate.CombateGanado)
        {
            ControladorEventos.Instancia.LanzarEvento(new EventoTeletransportarseCentroPokemon());
        }

        //*********************
        //  TRANSICION A CLARO
        //*********************
        yield return StartCoroutine(CorrutinasComunes.AlfaDeUnoACero(pantallaDeCarga));
        ControladorDatos.Instancia.QuitarMusicaCombate();


        //*********************
        // TEXTO TRAS COMBATIR Y GANAR CONTRA EL ENTRENADOR, SE EJECUTA EL MÉTODO MostrarMensajeConversacion PARA QUE COMPRUEBE SI LA CONVERSACIÓN DA LOGROS E ITEMS
        //*********************
        if (tipoCombate == TipoDeCombate.Entrenador && DatosCombate.CombateGanado)
        {
            EventoMostrarMensajeConversacion e = new EventoMostrarMensajeConversacion(entrenador.Conversacion);
            yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarMensajeConversacion(e, true));
            if (e.Conversacion.curarEquipoPokemonDelJugador)
                yield return StartCoroutine(CorrutinasComunes.CurarEquipoPokemon());
        }

        //*********************
        // COMPROBAMOS SI ALGÚN POKÉMON HA EVOLUCIONADO
        //*********************
        if (DatosCombate.CombateGanado)
        {
            yield return StartCoroutine(ComprobarSiAlgunPokemonDelEquipoHaEvolucionado());
        }


        DatosCombate = null;
    }

    private void RellenarInterfazConDatosDelPokemonActivo(PokemonLuchador pokemon, Image barraSalud, Text nombreYNivel, Text textoSalud, bool pokemonDelJugador = true)
    {
        if (pokemon.Pokemon.DatosFijos.sprite != null)
        {
            if(pokemonDelJugador)
                spritesCombate.AsignarSpritePokemonJugador(pokemon.Pokemon.DatosFijos);
            else
                spritesCombate.AsignarSpritePokemonEnemigo(pokemon.Pokemon.DatosFijos);
        }
            
        barraSalud.fillAmount = pokemon.Pokemon.SaludEnEscalaDe1();
        if (pokemonDelJugador)
            barraExperienciaJugador.fillAmount = pokemon.Pokemon.ExperienciaEnEscalaDe1();

        nombreYNivel.text = string.Concat(pokemon.Pokemon.DatosFijos.nombre, " Lvl.", pokemon.Pokemon.Nivel);
        textoSalud.text = string.Concat(pokemon.Pokemon.Salud, "/", pokemon.Pokemon.EstadisticaSaludMaxima());
    }

    private IEnumerator Atacar(PokemonLuchador pokemonAtacante, PokemonLuchador pokemonDefensor, AtaqueCombate ataque, Image barraSaludAtacante, Image barraSaludDefensor, Text textoSaludPokemonAtacante, Text textoSaludPokemonDefensor, bool atacaElJugador)
    {
        //Si no ha fallado el ataque...
        if (!ataque.haFallado)
        {
            //... Imprime texto indicando el pokémon y ataque
            string texto = Ajustes.Instancia.textoPokemonAtaca
                        .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, pokemonAtacante.Pokemon.DatosFijos.nombre)
                        .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, pokemonAtacante.Pokemon.Ataque(ataque.id).DatosFijos.nombre);
            yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, false, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));

            if (atacaElJugador)
            {
                if(ataque.tipoAtaque == TipoDeAtaque.Fisico)
                {
                    while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.PokemonJugadorRealizaAtaqueFisico))
                        yield return null;
                }

                while (spritesCombate.MostrarAnimacionAtaque(ataque.id, Atacante.Jugador))
                    yield return null;
            }
            else
            {
                if (ataque.tipoAtaque == TipoDeAtaque.Fisico)
                {
                    while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.PokemonEnemigoRealizaAtaqueFisico))
                        yield return null;
                }

                while (spritesCombate.MostrarAnimacionAtaque(ataque.id, Atacante.Enemigo))
                    yield return null;
            }

            //... Comprobamos el incremento del daño del ataque según el tipo de elemento del pokémon enemigo
            float multiplicadorDanioPorElementos = tablaTipos.MultiplicadorElemento(ataque.elemento, pokemonDefensor.Pokemon.DatosFijos.tipoElemento1, pokemonDefensor.Pokemon.DatosFijos.tipoElemento2);
            //... Si ha devuelto 0 significa que es inmune y no se le hace daño...
            if (multiplicadorDanioPorElementos <= 0)
            {
                //... Por lo que imprime el texto en pantalla indicando que es inmune
                yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(Ajustes.Instancia.textoPokemonInmuneAlAtaque, false, false));
            }
            //... Si no es inmune comprobamos el daño final...
            else
            {
                //... Comprueba si el ataque ha sido crítico
                float multiplicadorCritico = 1;
                if (ataque.esCritico)
                    multiplicadorCritico = Ajustes.Instancia.danioCriticos;

                //... Calcula el daño final del ataque teniendo en cuenta los tipos de elementos y si ha sido crítico
                ataque.danio = (int)(ataque.danio * multiplicadorDanioPorElementos * multiplicadorCritico);

                //... El pokémon enemigo recibe el ataque
                pokemonDefensor.RecibirAtaque(ataque);

                //... Si el multiplicador no es cercano a 1 (1 sería un ataque normal que no es muy efectivo ni poco efectivo) imprimimos por pantalla un texto informando de la efectividad del ataque
                if (multiplicadorDanioPorElementos <= 0.9f || multiplicadorDanioPorElementos > 1.1f)
                {
                    if (multiplicadorDanioPorElementos < 0.3f)
                        texto = Ajustes.Instancia.textoPokemonAtaque25PorcientoDanio;
                    else if (multiplicadorDanioPorElementos < 0.7f)
                        texto = Ajustes.Instancia.textoPokemonAtaque50PorcientoDanio;
                    else if (multiplicadorDanioPorElementos > 1.9f)
                        texto = Ajustes.Instancia.textoPokemonAtaque200PorcientoDanio;
                    else
                        texto = Ajustes.Instancia.textoPokemonAtaque150PorcientoDanio;

                    yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, false, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));

                }

                if (atacaElJugador)
                {
                    while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.PokemonEnemigoRecibeAtaque))
                        yield return null;
                }
                else
                {
                    while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.PokemonJugadorRecibeAtaque))
                        yield return null;
                }

                //... Cambia el valor de la barra de salud con una animación
                float porcentajeDeSalud = pokemonDefensor.Pokemon.SaludEnEscalaDe1();
                yield return StartCoroutine(CorrutinasComunes.MoverBarraDeslizadora(barraSaludDefensor, porcentajeDeSalud, pokemonDefensor.Pokemon.EstadisticaSaludMaxima(), textoSaludPokemonDefensor, null));

            }

            //... Comprueba si el pokémon original se autoinflinge daño
            if (ataque.pokemonOriginalSeHaceDanio)
            {
                pokemonAtacante.RecibirAutoAtaque(ataque);
                //TODO: reproducir animacion recibir daño
                //... Cambia el valor de la barra de salud con una animación
                float porcentajeDeSalud = pokemonAtacante.Pokemon.SaludEnEscalaDe1();
                while (barraSaludAtacante.fillAmount != porcentajeDeSalud)
                {
                    barraSaludAtacante.fillAmount = Mathf.Lerp(barraSaludAtacante.fillAmount, porcentajeDeSalud, Ajustes.Instancia.velocidadDeMovimientoDeBarrasDeSaludEnCombate);
                    yield return null;
                }
            }
            //... Comprueba si el pokémon original se autocura
            if (ataque.pokemonOriginalSeCura)
            {
                pokemonAtacante.RecibirAutoSanacion(ataque);
                //TODO: reproducir animacion curar
                //... Cambia el valor de la barra de salud con una animación
                float porcentajeDeSalud = pokemonAtacante.Pokemon.SaludEnEscalaDe1();
                while (barraSaludAtacante.fillAmount != porcentajeDeSalud)
                {
                    barraSaludAtacante.fillAmount = Mathf.Lerp(barraSaludAtacante.fillAmount, porcentajeDeSalud, Ajustes.Instancia.velocidadDeMovimientoDeBarrasDeSaludEnCombate);
                    yield return null;
                }
            }          

            //... Resta PP al ataque usado
            pokemonAtacante.RestarPP(ataque.id);
        }
        else
        {
            yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(Ajustes.Instancia.textoPokemonFallaAtaque, false, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));
        }
    }
    

    private IEnumerator TurnoJugadorCorrutina()
    {
        if (DatosCombate.CombateActivo && DatosCombate.PokemonJugadorActivo().Pokemon.EstaVivo())
        {
            switch (proximaAccion.TipoAccion)
            {
                case TipoAccion.Atacar:
                    //Pokémon del jugador genera ataque (La comprobación de PP para ver si puede atacar se realizará en la interfaz)
                    //Si se queda sin PP en todos los ataques al seleccionar Atacar en la interfaz realizará un ataque básico
                    AtaqueCombate ataque = DatosCombate.PokemonJugadorActivo().Atacar(proximaAccion.Ataque);
                    yield return StartCoroutine(Atacar(DatosCombate.PokemonJugadorActivo(), DatosCombate.PokemonEnemigoActivo(), ataque, barraSaludPokemonJugador, barraSaludPokemonEnemigo, textoSaludPokemonJugador, textoSaludPokemonEnemigo, true));
                    break;
                case TipoAccion.UsarItem:
                    switch (proximaAccion.Item.tipoDeItem)
                    {
                        case TipoDeItem.Curacion:
                            //No hacer nada, la curación se hará desde la interfaz
                            //Actualizamos la barra de salud
                            barraSaludPokemonJugador.fillAmount = DatosCombate.PokemonJugadorActivo().Pokemon.SaludEnEscalaDe1();
                            textoSaludPokemonJugador.text = string.Concat((DatosCombate.PokemonJugadorActivo().Pokemon.EstadisticaSaludMaxima() * barraSaludPokemonJugador.fillAmount).ToString("0"), "/", DatosCombate.PokemonJugadorActivo().Pokemon.EstadisticaSaludMaxima());
                            break;
                        case TipoDeItem.Pokeball:
                            yield return StartCoroutine(LanzarPokeball(proximaAccion.Item));
                            break;
                    }   
                    break;
                case TipoAccion.Escapar:
                    if(UnityEngine.Random.Range(0f,1f) > 0.5f)
                    {
                        DatosCombate.CombateActivo = false;
                        DatosCombate.CombateGanado = true;
                        //... Imprime texto indicando que ha podido escapar
                        yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(
                            Ajustes.Instancia.textoConsigueEscaparPokemonSalvaje, true, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));
                    }
                    else
                    {
                        //... Imprime texto indicando que no ha podido escapar
                        yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(
                            Ajustes.Instancia.textoNoConsigueEscaparPokemonSalvaje, true, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));
                    }
                    break;
                case TipoAccion.CambiarPokemon:

                    //... Oculta la barra de salud
                    panelBarraSaludPokemonJugador.SetActive(false);

                    //... Imprime texto indicando el cambio de pokémon
                    string texto = Ajustes.Instancia.textoRetirarPokemonJugador
                        .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, DatosCombate.PokemonJugadorActivo().Pokemon.DatosFijos.nombre);
                    yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, true, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));

                    //... Cambia el pokémon activo por el nuevo pokémon seleccionado
                    DatosCombate.CambiarPokemonActivoDelJugador(proximaAccion.ProximoPokemon);

                    //... Muestra la animación
                    while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.JugadorRetirarPokemon))
                        yield return null;

                    //... Asigna los valores del nuevo pokémon a la interfaz y activa el panel de vida
                    RellenarInterfazConDatosDelPokemonActivo(DatosCombate.PokemonJugadorActivo(), barraSaludPokemonJugador, nombreYNivelPokemonJugador, textoSaludPokemonJugador);

                    //... Imprime mensaje de enviar nuevo pokémon
                    string texto2 = Ajustes.Instancia.textoJugadorEnviaPokemon.Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, DatosCombate.PokemonJugadorActivo().Pokemon.DatosFijos.nombre);
                    yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto2, true, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));

                    //... Muestra la animación de enviar nuevo pokémon
                    while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.JugadorEnviaPokemon))
                        yield return null;

                    //... Activa la barra de salud
                    panelBarraSaludPokemonJugador.SetActive(true);
                    
                    break;
            }
        }
    }

    private IEnumerator TurnoOponenteCorrutina()
    {
        //Si el pokemon enemigo está vivo, lanza un ataque...
        if (DatosCombate.CombateActivo && DatosCombate.PokemonEnemigoActivo().Pokemon.EstaVivo())
        {
            //Pokémon del enemigo realiza un ataque aleatorio
            //TODO: comprobar PP del ataque antes de realizarlo
            AtaqueCombate Ataque = DatosCombate.PokemonEnemigoActivo().PokemonEnemigoAtaqueAleatorio(); 
            yield return StartCoroutine(Atacar(DatosCombate.PokemonEnemigoActivo(), DatosCombate.PokemonJugadorActivo(), Ataque, barraSaludPokemonEnemigo, barraSaludPokemonJugador, textoSaludPokemonEnemigo, textoSaludPokemonJugador, false));
            
        }
    }


    private IEnumerator ComprobarPokemonVivos()
    {
        //Si el pokémon enemigo actual no está vivo...
        if (!DatosCombate.PokemonEnemigoActivo().Pokemon.EstaVivo())
        {
            //... Imprime mensaje de pokémon derrotado
            string texto2 = Ajustes.Instancia.textoPokemonDerrotado.Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, DatosCombate.PokemonEnemigoActivo().Pokemon.DatosFijos.nombre);
            yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto2, false, true, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));

            //... Muestra la animación de pokémon derrotado
            while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.PokemonEnemigoDerrotado))
                yield return null;

            //... Desactivamos el panel de vida
            panelBarraSaludPokemonEnemigo.SetActive(false);

            //... Conceder XP al pokémon del jugador
            int experienciaSinAsignar = DatosCombate.PokemonEnemigoDerrotadoCalcularXP();
            string texto = Ajustes.Instancia.textoPokemonEnemigoDerrotado
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, DatosCombate.PokemonEnemigoActivo().Pokemon.DatosFijos.nombre)
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, experienciaSinAsignar.ToString());
            yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, false));
            
            float experienciaTrasIncrementarEnEscala1 = 0;

            //... Mientras al aumentar experiencia al pokémon del jugador retorne un valor igual o superior a 0 (lo que indica que ha subido de nivel y hay experiencia sobrante)
            do
            {
                //... Aumentamos la experiencia del pokémon y obtenemos el sobrante
                experienciaSinAsignar = DatosCombate.PokemonJugadorActivo().Pokemon.IncrementarXP(experienciaSinAsignar);

                //... Si la experiencia es mayor a -1 ha subido de nivel y la barra de experiencia se animará hasta rellenarse completamente
                if (experienciaSinAsignar >= 0)
                    experienciaTrasIncrementarEnEscala1 = 1;
                //... Si no la barra se rellenará hasta el valor actual
                else
                    experienciaTrasIncrementarEnEscala1 = DatosCombate.PokemonJugadorActivo().Pokemon.ExperienciaEnEscalaDe1();

                //... Rellenamos la barra de experiencia con una animación
                yield return StartCoroutine(CorrutinasComunes.MoverBarraDeslizadora(barraExperienciaJugador, experienciaTrasIncrementarEnEscala1, 0, null, null));

                //... Si subió de nivel...
                if (experienciaSinAsignar >= 0)
                {
                    ControladorDatos.Instancia.ReproducirSonido(SonidoID.SonidoSubirNivel);

                    //... Mostramos el incremento de estadísticas respecto al nivel anterior
                    UIControlador.Instancia.Combate.ventanaIncrementoEstadisticas.MostrarIncrementoEstadisticas(DatosCombate.PokemonJugadorActivo().Pokemon);
                    //... Mostramos el mensaje indicando que ha subido de nivel
                    texto = Ajustes.Instancia.textoPokemonSubeNivel
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, DatosCombate.PokemonJugadorActivo().Pokemon.DatosFijos.nombre)
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, DatosCombate.PokemonJugadorActivo().Pokemon.Nivel.ToString());
                    yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, false));
                    //... Desactivamos la ventana de estadísticas
                    UIControlador.Instancia.Combate.ventanaIncrementoEstadisticas.gameObject.SetActive(false);
                    //... Restablecemos la barra a 0
                    barraExperienciaJugador.fillAmount = 0;

                    //... Comprobamos si tiene que aprender algún nuevo ataque
                    yield return StartCoroutine(SubeDeNivelComprobarAtaquesAprendidos(DatosCombate.PokemonJugadorActivo().Pokemon));
                }
                
                yield return null;
            } while (experienciaSinAsignar > 0); //Si la experiencia sobrante es igual a 0 no es necesario repetir el bucle, ya que ha subido justo la experiencia necesaria

            yield return new WaitForSeconds(0.5f);

            //... Si el enemigo tiene algún pokémon vivo (para peleas contra entrenadores)...
            if (DatosCombate.ElEnemigoTieneAlgunPokemonVivo())
            {
                //... Buscamos el pokémon que queda vivo para establecerlo como el actual
                DatosCombate.PokemonEnemigoDerrotadoCambiarPorUnoVivo();

                //... Mostramos el texto indicando a qué pokémon va a enviar
                texto = Ajustes.Instancia.textoEntrenadorEnviaPokemon
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, entrenador.nombreEvento)
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, DatosCombate.PokemonEnemigoActivo().Pokemon.DatosFijos.nombre);
                yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, false, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));
                
                UIControlador.Instancia.Dialogo.campoDeTexto.text = string.Empty;

                //... Asigna los valores del nuevo pokémon a la interfaz y activa el panel de vida
                RellenarInterfazConDatosDelPokemonActivo(DatosCombate.PokemonEnemigoActivo(), barraSaludPokemonEnemigo, nombreYNivelPokemonEnemigo, textoSaludPokemonEnemigo, false);
                panelBarraSaludPokemonEnemigo.SetActive(true);

                //... Mostramos la animación de sacar nuevo pokémon
                while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.EntrenadorEnviaPokemon))
                    yield return null;
            }
            //... Si no tiene más pokémon finaliza el combate
            else
            {
                DatosCombate.CombateGanado = true;
                DatosCombate.CombateActivo = false;
            }
        }

        //Si el pokémon del jugador actual no está vivo...
        if (!DatosCombate.PokemonJugadorActivo().Pokemon.EstaVivo())
        {
            while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.PokemonJugadorDerrotado))
                yield return null;

            //... Desactivamos el panel de vida
            panelBarraSaludPokemonJugador.SetActive(false);
            
            //... Si el jugador tiene más pokémon vivos...
            if (DatosCombate.ElJugadorTieneAlgunPokemonVivo())
            {
                //... Imprime mensaje de pokémon derrotado
                string texto2 = Ajustes.Instancia.textoPokemonDerrotado.Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, DatosCombate.PokemonJugadorActivo().Pokemon.DatosFijos.nombre);
                yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto2, true, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));
                
                //... Muestra ventana de selección de pokémon y espera
                UIControlador.Instancia.EquipoPokemon.ventanaPrincipal.SetActive(true);
                UIControlador.Instancia.EquipoPokemon.botonCerrarVentana.SetActive(false);
                while (UIControlador.Instancia.EquipoPokemon.ventanaPrincipal.activeSelf)
                    yield return null;
                //... Desde la ventana se ha cambiado el pokémon activo, volvemos a activar el botón de cerrar la ventana
                UIControlador.Instancia.EquipoPokemon.botonCerrarVentana.SetActive(true);

                //... Cambia el pokémon activo por el nuevo pokémon seleccionado
                DatosCombate.CambiarPokemonActivoDelJugador(proximaAccion.ProximoPokemon);

                //... Asigna los valores del nuevo pokémon a la interfaz y activa el panel de vida
                RellenarInterfazConDatosDelPokemonActivo(DatosCombate.PokemonJugadorActivo(), barraSaludPokemonJugador, nombreYNivelPokemonJugador, textoSaludPokemonJugador);

                //... Imprime mensaje de enviar nuevo pokémon
                texto2 = Ajustes.Instancia.textoJugadorEnviaPokemon.Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, DatosCombate.PokemonJugadorActivo().Pokemon.DatosFijos.nombre);
                yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto2, true, false, Ajustes.Instancia.tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado));

                //... Muestra la animación de enviar nuevo pokémon
                while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.JugadorEnviaPokemon))
                    yield return null;

                //... Activa la barra de salud
                panelBarraSaludPokemonJugador.SetActive(true);
            }
            //... Si no tiene más pokémon finaliza el combate
            else
            {
                DatosCombate.CombateGanado = false;
                DatosCombate.CombateActivo = false;
            }

        }
        
    }
    
    private IEnumerator LanzarPokeball(Item it)
    {
        bool capturado = false;
        int ticks = 3;
        int random = UnityEngine.Random.Range(0, 101);
        if (random <= it.posibilidadCaptura)
            capturado = true;
        if (!capturado)
            ticks = UnityEngine.Random.Range(1, 4);

        while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.LanzarPokeball))
            yield return null;

        while (ticks > 0)
        {
            while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.PokeballSeAgita))
                yield return null;
            ticks--;
            yield return new WaitForSeconds(1f);
        }

        if (capturado)
        {
            //TODO: mostrar animación captura
            //... Mostramos el texto indicando que ha capturado al pokémon
            string texto = Ajustes.Instancia.textoPokemonCapturado
                                .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, DatosCombate.PokemonEnemigoActivo().Pokemon.DatosFijos.nombre);
            yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, false, true));
            

            if (ControladorDatos.Instancia.Datos.NumeroDePokemonEnElEquipo() == 6)
            {
                texto = Ajustes.Instancia.textoPokemonCapturadoEnviadoAlPC
                                .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, DatosCombate.PokemonEnemigoActivo().Pokemon.DatosFijos.nombre);
                yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, false, true));
            }
            ControladorDatos.Instancia.Datos.AniadirNuevoPokemonCapturado(DatosCombate.PokemonEnemigoActivo().Pokemon);
            DatosCombate.CombateActivo = false;
            DatosCombate.CombateGanado = true;
        }
        else
        {
            while (spritesCombate.MostrarAnimacionCombate(AnimacionCombate.PokeballSeRompe))
                yield return null;
        }
    }    

    private bool TeclaInteraccionPulsada()
    {
        if (Input.GetKeyDown(Ajustes.Instancia.teclaInteractuar) || Input.GetMouseButtonDown(0))
        {
            return true;
        }
        return false;
    }


    public IEnumerator ComprobarSiAlgunPokemonDelEquipoHaEvolucionado()
    {
        //Establecemos los valores y referencias
        PokemonModelo[] equipoPokemon = ControladorDatos.Instancia.Datos.ObtenerEquipoPokemon();

        //Recorremos cada pokémon del equipo...
        for (int i = 0; i < equipoPokemon.Length; i++)
        {
            //... Comprobación para evitar errores...
            if (equipoPokemon[i] != null)
            {
                //... Si el pokémon actual del equipo ha subido de nivel y está vivo...
                if (equipoPokemon[i].HaSubidoDeNivel && equipoPokemon[i].EstaVivo())
                {
                    //... Y tiene asignada una evolución y su nivel actual está por encima o es igual al nivel en el que evoluciona...
                    if (equipoPokemon[i].DatosFijos.evolucion != PokemonID.NINGUNO && equipoPokemon[i].DatosFijos.nivelEvolucion <= equipoPokemon[i].Nivel)
                    {              
                        //... Mostramos un mensaje indicando que el pokémon va a evolucionar
                        string texto = Ajustes.Instancia.textoPokemonVaEvolucionar
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, equipoPokemon[i].DatosFijos.nombre);
                        yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, false));

                        //... Oscurecemos la pantalla
                        yield return StartCoroutine(CorrutinasComunes.AlfaDeCeroAUno(pantallaDeCarga));

                        //... Rotamos la cámara para evitar que otros sprites del escenario se visualizen por encima
                        Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y + 180, Camera.main.transform.rotation.eulerAngles.z);

                        //... Ponemos a false la subida de nivel para volver a comprobar la evolución la próxima vez que suba nivel
                        equipoPokemon[i].HaSubidoDeNivel = false;

                        //... Asignamos los sprites de los pokémon
                        UIControlador.Instancia.ventanaSpriteEvolucion.AsignarSprites(equipoPokemon[i].DatosFijos);
                        
                        //... Aclaramos la pantalla
                        yield return StartCoroutine(CorrutinasComunes.AlfaDeUnoACero(pantallaDeCarga));

                        //... Reproducimos animación evolución
                        UIControlador.Instancia.ventanaSpriteEvolucion.ReproducirAnimacion();

                        //... Esperamos a que termine la animación
                        while (!UIControlador.Instancia.ventanaSpriteEvolucion.EvolucionFinalizada())
                            yield return null;

                        //... Imprimimos mensaje indicando que ha evolucionado
                        texto = Ajustes.Instancia.textoPokemonHaEvolucionado
                                .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, equipoPokemon[i].DatosFijos.nombre)
                                .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, ControladorDatos.Instancia.ObtenerPokemon(equipoPokemon[i].DatosFijos.ID).nombre);
                        yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, true));

                        //... Cambiamos el ID del pokémon por la de su evolución
                        equipoPokemon[i].Evolucionar();

                        //... Oscurecemos la pantalla
                        yield return StartCoroutine(CorrutinasComunes.AlfaDeCeroAUno(pantallaDeCarga));

                        //... Desactivamos la ventana de evolución
                        UIControlador.Instancia.ventanaSpriteEvolucion.gameObject.SetActive(false);

                        //... Restablecemos la rotación de la cámara
                        Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y - 180, Camera.main.transform.rotation.eulerAngles.z);

                        //... Aclaramos la pantalla
                        yield return StartCoroutine(CorrutinasComunes.AlfaDeUnoACero(pantallaDeCarga));
                    }
                }
            }
            yield return null;
        }
    }
    
    public IEnumerator SubeDeNivelComprobarAtaquesAprendidos(PokemonModelo pokemon)
    {        
        for (int i = 0; i < pokemon.DatosFijos.listaDeAtaques.Count; i++)
        {
            if(pokemon.DatosFijos.listaDeAtaques[i].nivelAprender == pokemon.Nivel && pokemon.DatosFijos.listaDeAtaques[i].ataque != AtaqueID.NINGUNO)
            {
                Ataque ataque = ControladorDatos.Instancia.ObtenerAtaque(pokemon.DatosFijos.listaDeAtaques[i].ataque);

                //... Si tiene menos de 4 ataques aprendidos lo asignamos directamente
                if (pokemon.Ataques().Count(x => x.ID == AtaqueID.NINGUNO) > 0)
                {
                    pokemon.AprenderNuevoAtaque(AtaqueID.NINGUNO, pokemon.DatosFijos.listaDeAtaques[i].ataque);

                    string texto = Ajustes.Instancia.textoNuevoAtaqueAprendido
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, pokemon.DatosFijos.nombre)
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, ataque.nombre);
                    yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, false));
                }
                else
                {
                    string texto = Ajustes.Instancia.textoPreguntarAprenderNuevoAtaque
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, pokemon.DatosFijos.nombre)
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, ataque.nombre);
                    yield return StartCoroutine(UIControlador.Instancia.Dialogo.componentePrincipal.MostrarTextoCorrutina(texto, false, true));
                    UIControlador.Instancia.AprenderAtaque.MostrarVentana(pokemon, pokemon.DatosFijos.listaDeAtaques[i].ataque);
                    while (UIControlador.Instancia.AprenderAtaque.gameObject.activeSelf)
                        yield return null;
                }
                break;

            }
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
    }
    
}