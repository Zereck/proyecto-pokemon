public enum Direccion
{
    PorDefecto,
    Arriba,
    Abajo,
    Izquierda,
    Derecha
}

public enum TipoMovimientoNPC
{
    NINGUNO,
    QuietoObservandoAreaDeDeteccion,
    MoversePorZona,
    GirarParaDetectarPersonaje
}


public enum TipoColision
{
    NINGUNO,
    Personaje,
    ObjetoColision
}

//FORMATO: ZONA_NPC_DESCRIPCIÓN
public enum Logro
{
    NINGUNO,
    CasaMadre_Madre_PrimeraConversacion,
    LaboratorioProfesor_PrimerPokemonObtenido,
    LaboratorioProfesor_Rival_RivalVencido,
    LaboratorioProfesor_Profesor_PrimeraConversacion,
    LaboratorioProfesor_PokemonInicial1,
    LaboratorioProfesor_PokemonInicial2,
    LaboratorioProfesor_PokemonInicial3,
    Ruta1_Entrenador1,
    Ruta1_Entrenador2,
    Ruta1_Entrenador3,
    Medalla_Gimnasion_1,
    Medalla_Gimnasion_2,
    Medalla_Gimnasion_3,
    Medalla_Gimnasion_4,
    Medalla_Gimnasion_5,
    Medalla_Gimnasion_6,
    Medalla_Gimnasion_7,
    Medalla_Gimnasion_8,
}

public enum ItemID
{
    NINGUNO,
    Pokeball,
    Pocion,
}

public enum TipoDeItem
{
    Curacion,
    Pokeball,
    //MT,
    Otros,
}


public enum EstadoAlterado
{
    NINGUNO,
    Paralizado,
    Confuso,
    Dormido,
    Envenedado,
    Derrotado
}

//public enum EstadisticaPokemon
//{
//    SaludMaxima,
//    Velocidad,
//    AtaqueFisico,
//    DefensaFisica,
//    AtaqueMagico,
//    DefensaMagica
//}

public enum TipoConversacion
{
    Hablar,
    Luchar
}

public enum PokemonID
{
    NINGUNO,
    Pokemon2,
    Pokemon3,
    Pokemon4,
    Pokemon1,
}

public enum AtaqueID
{
    NINGUNO,
    Placaje,
    LatigoCepa,
    Mordisco,
    Trueno,
    HojaAfilada
}

public enum Elemento
{
    NINGUNO,
    Agua,
    Fuego,
    Planta,
    Fantasma,
    Psiquico,
    Electrico,
    Volador,
    Normal,
    Roca,
    Tierra,
    Veneno,
    Hielo,
    Dragon,
    Lucha,
    Bicho,
}

public enum EstadoPokemonEnCombate
{
    TodaviaNoGenerado,
    Esperando,
    Ataque,
    RecibeDanio,
    Derrotado
}

public enum TipoDeAtaque
{
    Magico,
    Fisico
}

public enum PokedexTipoAvistamiento
{
    NINGUNO,
    Visto,
    Capturado,    
}

public enum AnimacionPokemon
{
    Normal,
    Ataque
}

public enum TipoDeCombate
{
    PokemonSalvaje,
    Entrenador
}

public enum TipoAccion
{
    Atacar,
    UsarItem,
    Escapar,
    CambiarPokemon
}

public enum Eleccion
{
    EnEspera,
    Si,
    No
}

public enum DanioElemento
{
    Normal,
    Doble,
    Mitad,
    Inmune
}

public enum AccionSequencia
{
    NPC_MoverAPosicionEspecifica,
    NPC_MoverASuPosicionInicial,
    NPC_MoverHastaElPersonaje,
    NPC_MostrarConversacion,
    NPC_Ocultar,
    Personaje_MoverAPosicionEspecífica,
    Personaje_TeletransportarAPosicionEspecifica,
    Personaje_SeguirNPC,
    Personaje_DejarDeSeguirNPC,
    EjecutarMetodos,
    EsperarTiempo,
    ReproducirAnimacion,
    CurarEquipoPokemon,
    MostrarDialogoEspecifico,
}

public enum Atacante
{
    Jugador,
    Enemigo
}

public enum AnimacionCombate
{
    PokemonSalvajeAparece,
    EntrenadorEnviaPokemon,
    JugadorEnviaPokemon,
    LanzarPokeball,
    PokeballSeAgita,
    PokeballSeRompe,
    PokemonJugadorRecibeAtaque,
    PokemonEnemigoRecibeAtaque,
    PokemonJugadorRealizaAtaqueFisico,
    PokemonEnemigoRealizaAtaqueFisico,
    PokemonJugadorDerrotado,
    PokemonEnemigoDerrotado,
    JugadorRetirarPokemon
}

public enum MusicaID
{
    NINGUNO,
    MusicaPueblo1,
    MusicaRuta1,
    MusicaCombate,
    MusicaCiudad1,
}

public enum SonidoID
{
    SonidoEntrarPuerta,
    SonidoPulsarBotonUI,
    SonidoHablarNPC,
    SonidoCurarPokemon,
    SonidoSueloSaltable,
    SonidoCombateSacarPokemon,
    SonidoAtaque1,
    SonidoAtaque2,
    SonidoAtaque3,
    SonidoSubirNivel,
}
