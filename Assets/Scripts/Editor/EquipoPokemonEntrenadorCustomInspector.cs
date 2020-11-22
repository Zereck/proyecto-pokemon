using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

[CustomEditor(typeof(EquipoPokemonEntrenador))]
public class EquipoPokemonEntrenadorCustomInspector : Editor {

    private ReorderableList reorderableList;

    private EquipoPokemonEntrenador entrenador;
    private VinculadorResources vinculador;

    private void OnEnable()
    {
        vinculador = (VinculadorResources)Resources.Load("VinculadorResources");
        entrenador = (EquipoPokemonEntrenador)target;
        if (entrenador.pokemons == null)
            entrenador.pokemons = new System.Collections.Generic.List<PokemonEntrenador>();
        reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("pokemons"), true, true, true, true);
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight + (18 * 7);

        // Add listeners to draw events
        reorderableList.drawHeaderCallback += DrawHeader;
        reorderableList.drawElementCallback += DrawElement;

        reorderableList.onAddCallback += AddItem;
        reorderableList.onRemoveCallback += RemoveItem;
    }

    private void OnDisable()
    {
        // Make sure we don't get memory leaks etc.
        reorderableList.drawHeaderCallback -= DrawHeader;
        reorderableList.drawElementCallback -= DrawElement;

        reorderableList.onAddCallback -= AddItem;
        reorderableList.onRemoveCallback -= RemoveItem;
    }

    /// <summary>
    /// Draws the header of the list
    /// </summary>
    /// <param name="rect"></param>
    private void DrawHeader(Rect rect)
    {
        GUI.Label(rect, "Lista de Pokémon del Entrenador");
    }

    /// <summary>
    /// Draws one element of the list (ListItemExample)
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="active"></param>
    /// <param name="focused"></param>
    private void DrawElement(Rect rect, int index, bool active, bool focused)
    {
        PokemonEntrenador pokemon = entrenador.pokemons[index];
        EditorGUI.BeginChangeCheck();
        List<string> ataquesTexto = new List<string>();
        Pokemon p = (Pokemon)Resources.Load(vinculador.ObtenerNombreFicheroPokemon(pokemon.id));
        pokemon.id = (PokemonID)EditorGUI.EnumPopup(new Rect(rect.x, rect.y, rect.width, 18), new GUIContent("Pokemon ID"), pokemon.id);
        pokemon.nivel = EditorGUI.IntSlider(new Rect(rect.x, rect.y + (18), rect.width, 18), new GUIContent("Nivel"), pokemon.nivel, Ajustes.nivelMinimo, Ajustes.nivelMaximo);
        if (p != null)
        {
            List<AtaqueID> ataques = p.listaDeAtaques.Where(x => x.nivelAprender <= pokemon.nivel).Select(y => y.ataque).ToList();

            if(ataques.Count > 0)
            {
                if (!ataquesTexto.Contains(AtaqueID.NINGUNO.ToString()))
                    ataquesTexto.Add(AtaqueID.NINGUNO.ToString());
                for (int i = 0; i < ataques.Count; i++)
                {
                    if (!ataquesTexto.Contains(ataques[i].ToString()))
                    {
                        ataquesTexto.Add(ataques[i].ToString());
                    }
                }
            }
            else
            {
                EditorGUI.HelpBox(new Rect(rect.x, rect.y + (18 * 2), rect.width, 18 * 3), "El pokémon no tiene ningún ataque definido para el nivel", MessageType.Error);
                return;
            }            
        }
        else
        {
            EditorGUI.HelpBox(new Rect(rect.x, rect.y + (18 * 2), rect.width, 18 * 3), "No se ha encontrado el pokémon en la carpeta de Resources", MessageType.Error);
            return;
        }
                     
       
        pokemon.calidad = EditorGUI.IntSlider(new Rect(rect.x, rect.y + (18 * 2), rect.width, 18), new GUIContent("Calidad"), pokemon.calidad, Ajustes.calidadMinimaPokemon, Ajustes.calidadMaximaPokemon);
        pokemon.ataque1 = Herramientas.ParseEnum<AtaqueID>(ataquesTexto[EditorGUI.Popup(new Rect(rect.x, rect.y + (18 * 3), rect.width, 18), "Ataque 1", ataquesTexto.IndexOf(pokemon.ataque1.ToString()), ataquesTexto.ToArray())]);
        if(pokemon.ataque1 != AtaqueID.NINGUNO && ataquesTexto.Count > 2)
        {
            pokemon.ataque2 = Herramientas.ParseEnum<AtaqueID>(ataquesTexto[EditorGUI.Popup(new Rect(rect.x, rect.y + (18 * 4), rect.width, 18), "Ataque 2", ataquesTexto.IndexOf(pokemon.ataque2.ToString()), ataquesTexto.ToArray())]);
            if(pokemon.ataque2 != AtaqueID.NINGUNO && pokemon.ataque2 != pokemon.ataque1 && ataquesTexto.Count > 3)
            {
                pokemon.ataque3 = Herramientas.ParseEnum<AtaqueID>(ataquesTexto[EditorGUI.Popup(new Rect(rect.x, rect.y + (18 * 5), rect.width, 18), "Ataque 3", ataquesTexto.IndexOf(pokemon.ataque3.ToString()), ataquesTexto.ToArray())]);
                if (pokemon.ataque3 != AtaqueID.NINGUNO && pokemon.ataque3 != pokemon.ataque1 && pokemon.ataque3 != pokemon.ataque2 && ataquesTexto.Count > 4)
                {
                    pokemon.ataque4 = Herramientas.ParseEnum<AtaqueID>(ataquesTexto[EditorGUI.Popup(new Rect(rect.x, rect.y + (18 * 6), rect.width, 18), "Ataque 4", ataquesTexto.IndexOf(pokemon.ataque4.ToString()), ataquesTexto.ToArray())]);

                    if(pokemon.ataque4 == pokemon.ataque1 || pokemon.ataque4 == pokemon.ataque2 || pokemon.ataque4 == pokemon.ataque3)
                    {
                        EditorGUI.HelpBox(new Rect(rect.x, rect.y + (18 * 6), rect.width, (18)), "El ataque 4 está repetido", MessageType.Error);
                    }
                }
                else if(pokemon.ataque3 == pokemon.ataque1 || pokemon.ataque3 == pokemon.ataque2)
                {
                    pokemon.ataque4 = AtaqueID.NINGUNO;
                    EditorGUI.HelpBox(new Rect(rect.x, rect.y + (18 * 6), rect.width, (18)), "El ataque 3 está repetido", MessageType.Error);
                }
            }
            else if(pokemon.ataque2 != pokemon.ataque1)
            {
                pokemon.ataque3 = AtaqueID.NINGUNO;
                pokemon.ataque4 = AtaqueID.NINGUNO;
                EditorGUI.HelpBox(new Rect(rect.x, rect.y + (18 * 6), rect.width, (18)), "El ataque 2 está repetido", MessageType.Error);
            }
        }
        else
        {
            pokemon.ataque2 = AtaqueID.NINGUNO;
            pokemon.ataque3 = AtaqueID.NINGUNO;
            pokemon.ataque4 = AtaqueID.NINGUNO;
        }
               

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void AddItem(ReorderableList list)
    {
        if(entrenador.pokemons.Count < 6)
        {
            entrenador.pokemons.Add(new PokemonEntrenador());
            EditorUtility.SetDirty(target);
        }
        else
        {
            Debug.LogWarning("El entrenador ya tiene 6 pokémons");
        }
        
    }

    private void RemoveItem(ReorderableList list)
    {
        entrenador.pokemons.RemoveAt(list.index);
        EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // Actually draw the list in the inspector
        reorderableList.DoLayoutList();
    }
}
