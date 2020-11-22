using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu]
public class VinculadorResources : ScriptableObject {
    
    public List<ItemVinculado> listaItem;
    public List<PokemonVinculado> listaPokemon;
    public List<AtaqueVinculado> listaAtaque;
    public List<SonidoVinculado> listaSonidos;
    public List<MusicaVinculada> listaMusica;


    public string ObtenerNombreFicheroItem(ItemID itemID)
    {
        for (int i = 0; i < listaItem.Count; i++)
        {
            if (listaItem[i].ID == itemID)
            {
                return listaItem[i].nombreFichero;
            }                
        }
        if(itemID != ItemID.NINGUNO)
            Debug.LogWarning(string.Concat("No se ha encontrado ningún Item con el ID ", itemID.ToString()));
        return string.Empty;
    }

    public string ObtenerNombreFicheroPokemon(PokemonID pokemon)
    {
        for (int i = 0; i < listaPokemon.Count; i++)
        {
            if (listaPokemon[i].ID == pokemon)
            {
                return listaPokemon[i].nombreFichero;
            }
        }
        if(pokemon != PokemonID.NINGUNO)
            Debug.LogWarning(string.Concat("No se ha encontrado ningún Pokémon con el ID ", pokemon.ToString()));
        return string.Empty;
    }

    public string ObtenerNombreFicheroAtaque(AtaqueID ataque)
    {
        for (int i = 0; i < listaAtaque.Count; i++)
        {
            if (listaAtaque[i].ID == ataque)
            {
                return listaAtaque[i].nombreFichero;
            }
        }
        if(ataque != AtaqueID.NINGUNO)
            Debug.LogWarning(string.Concat("No se ha encontrado ningún Ataque con el ID ", ataque.ToString()));
        return string.Empty;
    }

    public string ObtenerNombreFicheroSonido(SonidoID audio)
    {
        for (int i = 0; i < listaSonidos.Count; i++)
        {
            if (listaSonidos[i].ID == audio)
            {
                return listaSonidos[i].nombreFichero;
            }
        }
        return string.Empty;
    }

    public string ObtenerNombreFicheroMusica(MusicaID musica)
    {
        for (int i = 0; i < listaMusica.Count; i++)
        {
            if (listaMusica[i].ID == musica)
            {
                return listaMusica[i].nombreFichero;
            }
        }
        return string.Empty;
    }
}

[System.Serializable]
public class ItemVinculado
{
    public ItemID ID;
    public string nombreFichero;

    public ItemVinculado(ItemID item, string nombreFichero)
    {
        this.ID = item;
        this.nombreFichero = nombreFichero;
    }
}

[System.Serializable]
public class PokemonVinculado
{
    public PokemonID ID;
    public string nombreFichero;

    public PokemonVinculado(PokemonID pokemon, string nombreFichero)
    {
        this.ID = pokemon;
        this.nombreFichero = nombreFichero;
    }
}

[System.Serializable]
public class AtaqueVinculado
{
    public AtaqueID ID;
    public string nombreFichero;

    public AtaqueVinculado(AtaqueID ataque, string nombreFichero)
    {
        this.ID = ataque;
        this.nombreFichero = nombreFichero;
    }
}

[System.Serializable]
public class SonidoVinculado
{
    public SonidoID ID;
    public string nombreFichero;

    public SonidoVinculado(SonidoID id, string nombreFichero)
    {
        this.ID = id;
        this.nombreFichero = nombreFichero;
    }
}

[System.Serializable]
public class MusicaVinculada
{
    public MusicaID ID;
    public string nombreFichero;

    public MusicaVinculada(MusicaID id, string nombreFichero)
    {
        this.ID = id;
        this.nombreFichero = nombreFichero;
    }
}