using UnityEngine;

[CreateAssetMenu()]
public class Ajustes : ScriptableObject
{
    [Space(10)]
    [Header("Movimiento")]
    [Range(0.3f, 2f)]
    public float velocidadAndar = 0.5f;
    [Range(0.5f, 2f)]
    public float velocidadCorrer = 0.8f;
    public float tamanioCasilla = 0.16f;
    public string nombreParametroAnimacionSaltar = "Saltar";
    [Range(1,15)]
    public int pasosMinimosHastaElProximoCombate = 4;
    [Range(16, 35)]
    public int pasosMaximosHastaElProximoCombate = 20;
    //public Vector2 posicionInicioPersonaje = Vector2.zero;

    [Space(10)]
    [Header("Teclado")]
    public KeyCode teclaCorrer = KeyCode.LeftShift;
    public KeyCode teclaInteractuar = KeyCode.Space;
    public KeyCode teclaAbrirMenu = KeyCode.C;

    [Space(10)]
    [Header("Tags y Layers")]
    public string tagInteraccion = "Interactivo";
    public LayerMask layerColision;
    public string tagPersonaje = "Player";
    public LayerMask layerPersonaje;
    public string tagSueloSaltable = "SueloSaltable";
    public LayerMask layerHierba;

    [Space(10)]
    [Header("Interfaz")]
    [Tooltip("Si el valor es 0 se mostrará el texto instantáneamente")]
    [Range(0, 0.1f)]
    public float velocidadMensajeDeDialogo = 0.1f;
    [Range(50, 200)]
    public int maximasLetrasPorVentanaDeDialogo = 140;
    [Range(0.01f, 0.1f)]
    public float velocidadOscurecerPantallaTeletransporte = 0.04f;
    [Range(0.01f, 0.1f)]
    public float velocidadAclararPantallaTeletransporte = 0.04f;


    [Space(10)]
    [Header("Sonido")]
    [Range(0.5f, 3f)]
    public float velocidadIncrementoVolumenNuevaMusica = 0.5f;
    [Range(0.5f, 3f)]
    public float velocidadDecrementoVolumenAntiguaMusica = 2f;

    [Space(10)]
    [Header("Combate")]
    public float danioCriticos = 1.5f;
    [Range(0, 100)]
    public int probabilidadDeDespertarseAlAtacar = 50;
    [Range(0, 100)]
    public int probabilidadDeAutoGolpearseAlEstarConfuso = 30;
    [Range(0, 100)]
    public int probabilidadDeEliminarseLaConfusionPorTurno = 30;
    [Range(0, 100)]
    public int porcentajeDeSaludPorEstarConfuso = 5;
    [Range(0, 100)]
    public int probabilidadDeQuedarseParalizado = 50;
    [Range(0, 100)]
    public int porcentajeDeSaludPorEstarEnvenenado = 5;
    [Range(0.001f, 0.2f)]
    public float velocidadDeMovimientoDeBarrasDeSaludEnCombate = 0.05f;
    [Range(0, 2f)]
    public float tiempoDeEsperaUltimaVentanaDialogoEnCombateSiNoSeLeeElTeclado = 1f;
    [Range(20, 300)]
    public float multiplicadorExperienciaBase = 50;

    [Space(10)]
    [Header("NPCs")]
    [Range(0.5f, 10f)]
    public float segundosParaMoverNPC = 3f;
    [Range(0.01f, 1f)]
    public float velocidadAnimacionNPC = 0.3f;

    [Space(10)]
    [Header("Textos Diálogos")]
    public string palabraParaReemplazarEnLosTexto1 = "<remplazo1>";
    public string palabraParaReemplazarEnLosTexto2 = "<remplazo2>";
    public string textoPokemonInicialMensajeConfirmacion = "¿Quieres a <remplazo1>?";
    public string textoPokemonInicialMensajeTrasConfirmar = "¡Enhorabuena! Has conseguido a <remplazo1>";
    public string textoPokemonSalvajeAparecio = "Un <remplazo1> salvaje apareció";
    public string textoEntrenadorTeDesafia = "<remplazo1> te desafía a un duelo";
    public string textoJugadorEnviaPokemon = "Adelante <remplazo1>";
    public string textoEntrenadorEnviaPokemon = "<remplazo1> envía a <remplazo2>";
    public string textoPokemonAtaca = "<remplazo1> usa <remplazo2>";
    public string textoPokemonFallaAtaque = "Ha fallado";
    public string textoPokemonInmuneAlAtaque = "No le afecta";
    public string textoPokemonAtaque25PorcientoDanio = "No es muy efectivo";
    public string textoPokemonAtaque50PorcientoDanio = "Es muy poco efectivo";
    public string textoPokemonAtaque150PorcientoDanio = "Es muy efectivo";
    public string textoPokemonAtaque200PorcientoDanio = "Es super efectivo";
    public string textoEntrenadorVencido = "Has derrotado a <remplazo1> obtienes <remplazo2> monedas";
    public string textoCuandoConsigueItem = "Has conseguido <remplazo1> x <remplazo2>";
    public string textoCombatePerdido = "Tus monstruos han sido derrotados. Has perdido <remplazo1> monedas";
    public string textoPokemonEnemigoDerrotado = "Has derrotado a <remplazo1> obtienes <remplazo2> puntos de experiencia";
    public string textoPokemonSubeNivel = "<remplazo1> ha subido al nivel <remplazo2>";
    public string textoPokemonVaEvolucionar = "<remplazo1> está evolucionando";
    public string textoPokemonHaEvolucionado = "<remplazo1> ha evolucionado a <remplazo2>";
    public string textoPokemonCapturado = "Has capturado a <remplazo1>";
    public string textoPokemonCapturadoEnviadoAlPC = "No hay espacio en tu equipo, <remplazo1> ha sido enviado al PC";
    public string textoConsigueEscaparPokemonSalvaje = "Has escapado del combate";
    public string textoNoConsigueEscaparPokemonSalvaje = "No has podido escapar del combate";
    public string textoRetirarPokemonJugador = "¡Vuelve <remplazo1>!";
    public string textoPokemonDerrotado = "<remplazo1> se ha debilitado";
    public string textoGuardarPartida = "¿Quieres guardar la partida?";
    public string textoPartidaGuardada = "Los datos de la partida han sido guardados";
    public string textoNuevoAtaqueAprendido = "<remplazo1> ha aprendido <remplazo2>";
    public string textoPreguntarAprenderNuevoAtaque = "<remplazo1> quiere aprender <remplazo2> pero ya tiene 4 ataques. Selecciona un ataque para sustituirlo";
    public string textoConfirmarAprenderNuevoAtaque= "¿Quieres sustituir <remplazo1> por <remplazo2>?";
    public string textoConfirmarNoAprenderNuevoAtaque = "¿Seguro que no quieres aprender el ataque <remplazo1>?";

    [Space(10)]
    [Header("Pokedex")]
    public string PokedexNombrePokemonCuandoNoLoHavistoNiCapturado = "???????";
    public Sprite iconoPokemonVisto;
    public Sprite iconoPokemonCapturado;
       

    //CONSTANTES
    public const int estadisticaSaludBaseMaxima = 255;
    public const int estadisticasBaseMaximas = 230;
    public const int poderMaximoAtaquePokemon = 120;
    public const int usosMaximoAtaquesPokemon = 40;
    public const float tiempoHastaLiberarResources = 180;
    public const int nivelMinimo = 1;
    public const int nivelMaximo = 100;
    public const int calidadMinimaPokemon = 1;
    public const int calidadMaximaPokemon = 16;
    public const float tiempoDeEsperaUltimaVentanaDialogoSiNoSeLeeElTeclado = 1.5f;
    public const int cantidadMaximaItems = 99;
    public const string nombreFicheroPartida = "DatosPartida";

    //PROPIEDADES RUNTIME
    private Vector2 _tamanioAreaColisiones = Vector2.zero;
    public Vector2 TamanioAreaColisiones
    {
        get
        {
            if (_tamanioAreaColisiones == Vector2.zero)
                _tamanioAreaColisiones = new Vector2(tamanioCasilla * 0.7f, tamanioCasilla * 0.7f);
            return _tamanioAreaColisiones;
        }
    }
    private float _quintaParteTamanioCasilla = -1;
    public float QuintaParteTamanioCasilla
    {
        get
        {
            if (_quintaParteTamanioCasilla < 0)
                _quintaParteTamanioCasilla = tamanioCasilla / 5;
            return _quintaParteTamanioCasilla;
        }

    }
    private float _terceraParteTamanioCasilla = -1;
    public float TerceraParteTamanioCasilla
    {
        get
        {
            if (_terceraParteTamanioCasilla < 0)
                _terceraParteTamanioCasilla = tamanioCasilla / 3;
            return _terceraParteTamanioCasilla;
        }

    }

    //SINGLETON
    private static Ajustes _instancia;

    public static Ajustes Instancia
    {
        get
        {
            if (_instancia == null)
                _instancia = (Ajustes)Resources.Load("Ajustes");
            return _instancia;
        }
    }

}