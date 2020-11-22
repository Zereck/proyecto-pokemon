using System.Collections.Generic;

[System.Serializable]
public class Conversacion {

    public Logro logroConseguido = Logro.NINGUNO;
    public TipoConversacion tipoConversacion = TipoConversacion.Hablar;
    public string texto;
    public string texto2;
    public Logro darLogroPorTerminarConversacion = Logro.NINGUNO;
    public ItemID darItemPorTerminarConversacion = ItemID.NINGUNO;
    public int cantidadDeItems = 0;
    public int monedasRecompensa = 0;
    public bool curarEquipoPokemonDelJugador = false;
    public EquipoPokemonEntrenador equipoPokemon;
}
