using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Linq;

[CustomEditor(typeof(UIAsignadorAutomaticoPokemonPokedex))]
public class UIAsignadorAutomaticoPokemonPokedexCustomInspector : Editor {

    private UIAsignadorAutomaticoPokemonPokedex scriptPrincipal;
    private VinculadorResources vinculador;
    private List<UIPokemonPokedex> pokemonPokedexAniadidos;
    private Dictionary<int, Pokemon> posicionesPokemon;

    public override void OnInspectorGUI()
    {
        GUI.color = Color.cyan;        
        if (GUILayout.Button("Actualizar Pokedex", GUILayout.MinHeight(50)))
        {
            if (scriptPrincipal == null)
                scriptPrincipal = (UIAsignadorAutomaticoPokemonPokedex)target;
            if (vinculador == null)
                vinculador = (VinculadorResources)Resources.Load("VinculadorResources");
            if (pokemonPokedexAniadidos == null)
                pokemonPokedexAniadidos = new List<UIPokemonPokedex>();
            if (posicionesPokemon == null)
                posicionesPokemon = new Dictionary<int, Pokemon>();
            
            pokemonPokedexAniadidos.Clear();
            posicionesPokemon.Clear();

            if (scriptPrincipal.contenedorPokedex.transform.childCount > 1)
            {
                //El borrado desde custom inspector no funciona correctamente, añadiendo un segundo bucle se fuerza a eliminar todos de una vez
                while (scriptPrincipal.contenedorPokedex.transform.childCount > 1)
                {
                    for (int i = 0; i < scriptPrincipal.contenedorPokedex.transform.childCount; i++)
                    {
                        if (scriptPrincipal.plantilla.gameObject.GetInstanceID() != scriptPrincipal.contenedorPokedex.transform.GetChild(i).gameObject.GetInstanceID())
                            DestroyImmediate(scriptPrincipal.contenedorPokedex.transform.GetChild(i).gameObject);
                    }
                }
                pokemonPokedexAniadidos.Add(scriptPrincipal.plantilla);
            }
            
            for (int i = 0; i < vinculador.listaPokemon.Count; i++)
            {
                Pokemon p = (Pokemon)Resources.Load(vinculador.ObtenerNombreFicheroPokemon(vinculador.listaPokemon[i].ID));
                if(p != null)
                {
                    if (posicionesPokemon.ContainsKey(p.numeroEnPokedex))
                        Debug.LogWarning(string.Concat("El pokémon ", p.nombre, " tiene un número de pokédex ya asignado al pokémon ", posicionesPokemon[p.numeroEnPokedex].nombre));
                    else
                        posicionesPokemon.Add(p.numeroEnPokedex, p);
                }
            }

            if(posicionesPokemon.Count > 0)
            {
                foreach (var item in posicionesPokemon.OrderBy(i => i.Key))
                {
                    UIPokemonPokedex go = Instantiate(scriptPrincipal.plantilla.gameObject).GetComponent<UIPokemonPokedex>();
                    go.transform.SetParent(scriptPrincipal.contenedorPokedex.transform);
                    go.gameObject.transform.SetSiblingIndex(item.Key);
                    go.numero.text = item.Key.ToString("D3");
                    go.nombre.text = item.Value.nombre;
                    go.pokemon = item.Value.ID;
                    go.GetComponent<RectTransform>().localScale = Vector3.one;
                    go.gameObject.name = string.Concat(go.numero.text, " ", item.Value.nombre);
                    go.gameObject.SetActive(true);
                    pokemonPokedexAniadidos.Add(go);
                }
            }
                       

            //Calculamos la altura actual eliminando el elemento plantilla de la lista de pokémons
            float alturaPanel = (pokemonPokedexAniadidos.Count - 1) * scriptPrincipal.plantilla.GetComponent<RectTransform>().rect.height;
            RectTransform rt = scriptPrincipal.contenedorPokedex.GetComponent<RectTransform>();
            VerticalLayoutGroup vl = scriptPrincipal.contenedorPokedex.GetComponent<VerticalLayoutGroup>();
            //Calculamos el margin y el padding eliminando 2 elementos para el spacing (la plantilla y uno extra)
            alturaPanel += vl.padding.top + vl.padding.bottom + ((pokemonPokedexAniadidos.Count - 2) * vl.spacing);
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, alturaPanel);
        }
        GUI.color = Color.white;
        base.OnInspectorGUI();
    }

}
