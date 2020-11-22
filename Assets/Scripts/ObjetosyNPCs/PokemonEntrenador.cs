using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokemonEntrenador {

    public PokemonID id;
    public int nivel;
    public int calidad;
    public AtaqueID ataque1 = AtaqueID.NINGUNO;
    public AtaqueID ataque2 = AtaqueID.NINGUNO;
    public AtaqueID ataque3 = AtaqueID.NINGUNO;
    public AtaqueID ataque4 = AtaqueID.NINGUNO;

}
