using UnityEngine;

[System.Serializable]
public class PokemonSalvaje {

    [HideInInspector]
    public PokemonID id = PokemonID.Pokemon1;
    [HideInInspector]
    public int posibilidadAparicion;
    [HideInInspector]
    public AtaqueID ataque1 = AtaqueID.NINGUNO;
    [HideInInspector]
    public AtaqueID ataque2 = AtaqueID.NINGUNO;
    [HideInInspector]
    public AtaqueID ataque3 = AtaqueID.NINGUNO;
    [HideInInspector]
    public AtaqueID ataque4 = AtaqueID.NINGUNO;
}
