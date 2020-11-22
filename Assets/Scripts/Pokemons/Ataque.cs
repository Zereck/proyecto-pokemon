using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ataque : ScriptableObject {
    
    public AtaqueID ID;
    public string nombre;
    public Elemento ataqueElemento;
    public TipoDeAtaque tipoDeAtaque;
    [Range(5, Ajustes.usosMaximoAtaquesPokemon)]
    public int ppMaximos;
    [Range(10, Ajustes.poderMaximoAtaquePokemon)]
    public int poder;
    [Range(20, 100)]
    public int precision;
    [Range(0, 100)]
    public int posibilidadCritico;
    public EstadoAlterado provocaEstadoAlterado;
    public int probabilidadDeProvocarEstadoAlterado;
    public AnimationClip animacionAtaque;
    public bool seAutoInflingeDanio;
    public int probabilidadDeAutoInflingirseDanio;
    public int porcentajeDeDanioAutoInflingido;
    public bool seAutoCura;
    public int probabilidadDeCurarse;
    public int porcentajeDeCuracion;

    public string TextoTipoAtaque()
    {
        switch (tipoDeAtaque)
        {
            case TipoDeAtaque.Magico:
                return "Mágico";
            case TipoDeAtaque.Fisico:
                return "Físico";
        }
        return string.Empty;
    }
}