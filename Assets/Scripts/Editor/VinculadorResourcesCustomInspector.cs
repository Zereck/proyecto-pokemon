using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

[CustomEditor(typeof(VinculadorResources))]
public class VinculadorResourcesCustomInspector : Editor {

    private VinculadorResources scriptPrincipal;

    private void OnEnable()
    {
        scriptPrincipal = (VinculadorResources)target;
        VincularResources();
    }

    public override void OnInspectorGUI()
    {
        GUI.color = Color.cyan;
        if (GUILayout.Button("Actualizar Resources", GUILayout.MinHeight(50)))
        {
            VincularResources();
        }
        GUI.color = Color.white;
        ComprobarDuplicadosItem();
        ComprobarDuplicadosPokemon();
        ComprobarDuplicadosAtaques();
        ComprobarDuplicadosSonidos();
        ComprobarDuplicadosMusica();
        base.OnInspectorGUI();
        EditorUtility.SetDirty(target);
    }

    public void VincularResources()
    {
        scriptPrincipal.listaItem.Clear();
        scriptPrincipal.listaPokemon.Clear();
        scriptPrincipal.listaAtaque.Clear();
        scriptPrincipal.listaSonidos.Clear();
        scriptPrincipal.listaMusica.Clear();

        Object[] items = Resources.LoadAll("Items", typeof(Item));
        Object[] pokemons = Resources.LoadAll("Pokemons", typeof(Pokemon));
        Object[] ataques = Resources.LoadAll("Ataques", typeof(Ataque));
        Object[] sonidos = Resources.LoadAll("Audios", typeof(Sonido));
        Object[] musica = Resources.LoadAll("Audios", typeof(Musica));

        for (int i = 0; i < items.Length; i++)
        {
            Item item = (Item)items[i];
            scriptPrincipal.listaItem.Add(new ItemVinculado(item.ID, Path.Combine("Items", items[i].name)));
        }

        for (int i = 0; i < pokemons.Length; i++)
        {
            Pokemon pokemon = (Pokemon)pokemons[i];
            scriptPrincipal.listaPokemon.Add(new PokemonVinculado(pokemon.ID, Path.Combine("Pokemons", pokemons[i].name)));
        }

        for (int i = 0; i < ataques.Length; i++)
        {
            Ataque ataque = (Ataque)ataques[i];
            scriptPrincipal.listaAtaque.Add(new AtaqueVinculado(ataque.ID, Path.Combine("Ataques", ataques[i].name)));
        }

        for (int i = 0; i < sonidos.Length; i++)
        {
            Sonido audio = (Sonido)sonidos[i];
            scriptPrincipal.listaSonidos.Add(new SonidoVinculado(audio.ID, Path.Combine("Audios", sonidos[i].name)));
        }

        for (int i = 0; i < musica.Length; i++)
        {
            Musica mus = (Musica)musica[i];
            scriptPrincipal.listaMusica.Add(new MusicaVinculada(mus.ID, Path.Combine("Audios", musica[i].name)));
        }
    }

    private void ComprobarDuplicadosItem()
    {
        List<ItemID> query = scriptPrincipal.listaItem.GroupBy(x => x.ID)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

        if (query.Count > 0)
        {
            string idDuplicados = string.Empty;
            for (int i = 0; i < query.Count; i++)
            {
                idDuplicados = string.Concat(idDuplicados, "\n-", query[i].ToString());
            }
            EditorGUILayout.HelpBox(string.Concat("HAY VARIOS ITEMS CON EL MISMO ID:", idDuplicados), MessageType.Error);
        }
    }

    private void ComprobarDuplicadosPokemon()
    {
        List<PokemonID> query = scriptPrincipal.listaPokemon.GroupBy(x => x.ID)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

        if (query.Count > 0)
        {
            string idDuplicados = string.Empty;
            for (int i = 0; i < query.Count; i++)
            {
                idDuplicados = string.Concat(idDuplicados, "\n-", query[i].ToString());
            }
            EditorGUILayout.HelpBox(string.Concat("HAY VARIOS POKÉMON CON EL MISMO ID:", idDuplicados), MessageType.Error);
        }
    }

    private void ComprobarDuplicadosAtaques()
    {
        List<AtaqueID> query = scriptPrincipal.listaAtaque.GroupBy(x => x.ID)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

        if (query.Count > 0)
        {
            string idDuplicados = string.Empty;
            for (int i = 0; i < query.Count; i++)
            {
                idDuplicados = string.Concat(idDuplicados, "\n-", query[i].ToString());
            }
            EditorGUILayout.HelpBox(string.Concat("HAY VARIOS ATAQUES CON EL MISMO ID:", idDuplicados), MessageType.Error);
        }
    }

    private void ComprobarDuplicadosSonidos()
    {
        List<SonidoID> query = scriptPrincipal.listaSonidos.GroupBy(x => x.ID)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

        if (query.Count > 0)
        {
            string idDuplicados = string.Empty;
            for (int i = 0; i < query.Count; i++)
            {
                idDuplicados = string.Concat(idDuplicados, "\n-", query[i].ToString());
            }
            EditorGUILayout.HelpBox(string.Concat("HAY VARIOS SONIDOS CON EL MISMO ID:", idDuplicados), MessageType.Error);
        }
    }

    private void ComprobarDuplicadosMusica()
    {
        List<MusicaID> query = scriptPrincipal.listaMusica.GroupBy(x => x.ID)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

        if (query.Count > 0)
        {
            string idDuplicados = string.Empty;
            for (int i = 0; i < query.Count; i++)
            {
                idDuplicados = string.Concat(idDuplicados, "\n-", query[i].ToString());
            }
            EditorGUILayout.HelpBox(string.Concat("HAY VARIOS ARCHIVOS DE MUSICA CON EL MISMO ID:", idDuplicados), MessageType.Error);
        }
    }
}
