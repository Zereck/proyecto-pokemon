using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

[CustomEditor(typeof(PokemonEnLaZona))]
public class PokemonEnLaZonaCustomInspector : Editor {

    private ReorderableList reorderableList;

    private PokemonEnLaZona zona;
    private VinculadorResources vinculador;

    private void OnEnable()
    {
        vinculador = (VinculadorResources)Resources.Load("VinculadorResources");
        zona = (PokemonEnLaZona)target;
        if (zona.pokemons == null)
            zona.pokemons = new System.Collections.Generic.List<PokemonSalvaje>();
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
        GUI.Label(rect, "Lista de Pokémon Salvajes");
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
        PokemonSalvaje pokemon = zona.pokemons[index];
        EditorGUI.BeginChangeCheck();
        List<string> ataquesTexto = new List<string>();
        Pokemon p = (Pokemon)Resources.Load(vinculador.ObtenerNombreFicheroPokemon(pokemon.id));
        pokemon.id = (PokemonID)EditorGUI.EnumPopup(new Rect(rect.x, rect.y, rect.width, 18), new GUIContent("Pokemon ID"), pokemon.id);
        if (p != null)
        {
            List<AtaqueID> ataques = p.listaDeAtaques.Where(x => x.nivelAprender <= zona.nivelMaximoPokemonSalvaje).Select(y => y.ataque).ToList();

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
                EditorGUI.HelpBox(new Rect(rect.x, rect.y + 18, rect.width, 18 * 3), "El pokémon no tiene ningún ataque definido para el nivel máximo de la zona", MessageType.Error);
                return;
            }            
        }
        else
        {
            EditorGUI.HelpBox(new Rect(rect.x, rect.y + 18, rect.width, 18 * 3), "No se ha encontrado el pokémon en la carpeta de Resources", MessageType.Error);
            return;
        }
        




        //for (int i = 0; i < p.ataquesAprendidosPorMToMO.Count; i++)
        //{
        //    if (!ataquesTexto.Contains(p.ataquesAprendidosPorMToMO[i].ToString()))
        //    {
        //        ataquesTexto.Add(p.ataquesAprendidosPorMToMO[i].ToString());
        //    }
        //}
       
        pokemon.posibilidadAparicion = EditorGUI.IntSlider(new Rect(rect.x, rect.y + (18), rect.width, 18), new GUIContent("% de aparición"), pokemon.posibilidadAparicion, 1, 100);
        pokemon.ataque1 = Herramientas.ParseEnum<AtaqueID>(ataquesTexto[EditorGUI.Popup(new Rect(rect.x, rect.y + (18 * 2), rect.width, 18), "Ataque 1", ataquesTexto.IndexOf(pokemon.ataque1.ToString()), ataquesTexto.ToArray())]);
        if(pokemon.ataque1 != AtaqueID.NINGUNO && ataquesTexto.Count > 2)
        {
            pokemon.ataque2 = Herramientas.ParseEnum<AtaqueID>(ataquesTexto[EditorGUI.Popup(new Rect(rect.x, rect.y + (18 * 3), rect.width, 18), "Ataque 2", ataquesTexto.IndexOf(pokemon.ataque2.ToString()), ataquesTexto.ToArray())]);
            if(pokemon.ataque2 != AtaqueID.NINGUNO && pokemon.ataque2 != pokemon.ataque1 && ataquesTexto.Count > 3)
            {
                pokemon.ataque3 = Herramientas.ParseEnum<AtaqueID>(ataquesTexto[EditorGUI.Popup(new Rect(rect.x, rect.y + (18 * 4), rect.width, 18), "Ataque 3", ataquesTexto.IndexOf(pokemon.ataque3.ToString()), ataquesTexto.ToArray())]);
                if (pokemon.ataque3 != AtaqueID.NINGUNO && pokemon.ataque3 != pokemon.ataque1 && pokemon.ataque3 != pokemon.ataque2 && ataquesTexto.Count > 4)
                {
                    pokemon.ataque4 = Herramientas.ParseEnum<AtaqueID>(ataquesTexto[EditorGUI.Popup(new Rect(rect.x, rect.y + (18 * 5), rect.width, 18), "Ataque 4", ataquesTexto.IndexOf(pokemon.ataque4.ToString()), ataquesTexto.ToArray())]);

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
        
        

        
        //item.logroConseguido = (Logro)EditorGUI.EnumPopup(new Rect(rect.x, rect.y, rect.width, 18), new GUIContent("Logro Requerido", "Si ha cumplido este logro mostrará este diálogo"), item.logroConseguido);
        //EditorGUI.HelpBox(new Rect(rect.x, rect.y + (18 * 2), rect.width, (18)), "Mensaje NPC. Si es un combate lo dirá antes de combatir", MessageType.Info);
        //item.texto = EditorGUI.TextArea(new Rect(rect.x, rect.y + (18 * 3), rect.width, (18 * 4)), item.texto);
        //item.tipoConversacion = (TipoConversacion)EditorGUI.EnumPopup(new Rect(rect.x, rect.y + 18, rect.width, 18), new GUIContent("Tipo Conversación"), item.tipoConversacion);
        //item.darLogroPorTerminarConversacion = (Logro)EditorGUI.EnumPopup(new Rect(rect.x, rect.y + (18 * 7), rect.width, 18), new GUIContent("Logro al terminar conversacion/combate", "Al terminar la conversación el jugador recibirá este logro"), item.darLogroPorTerminarConversacion);
        //if (item.darLogroPorTerminarConversacion != Logro.NINGUNO)
        //{
        //    item.darItemPorTerminarConversacion = (ItemID)EditorGUI.EnumPopup(new Rect(rect.x, rect.y + (18 * 8), rect.width, 18), new GUIContent("Item al terminar conversación/combate", "Al terminar la conversación recibirá este objeto"), item.darItemPorTerminarConversacion);
        //    if (item.darItemPorTerminarConversacion != ItemID.NINGUNO)
        //        item.cantidadDeItems = EditorGUI.IntField(new Rect(rect.x, rect.y + (18 * 9), rect.width, 18), "Cantidad del item anterior", item.cantidadDeItems);
        //}


        //if (item.tipoConversacion == TipoConversacion.Luchar && item.darLogroPorTerminarConversacion != Logro.NINGUNO)
        //{
        //    EditorGUI.HelpBox(new Rect(rect.x, rect.y + (18 * 10), rect.width, (18)), "Mensaje tras ganar el combate", MessageType.Info);
        //    item.texto2 = EditorGUI.TextArea(new Rect(rect.x, rect.y + (18 * 11), rect.width, (18 * 4)), item.texto2);
        //    item.dineroRecompensa = EditorGUI.IntField(new Rect(rect.x, rect.y + (18 * 15), rect.width, 18), "Dinero Recompensa", item.dineroRecompensa);
        //    item.equipoPokemon = (EquipoPokemon)EditorGUI.ObjectField(new Rect(rect.x, rect.y + (18 * 16), rect.width, 18), item.equipoPokemon, typeof(EquipoPokemon), true);

        //}
        //else if (item.tipoConversacion == TipoConversacion.Luchar)
        //{

        //    EditorGUI.HelpBox(new Rect(rect.x, rect.y + (18 * 10), rect.width, (18 * 3)), "Un NPC que lucha siempre debe dar un logro al terminar, si no se podrá luchar con él repetidas veces", MessageType.Error);
        //}

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }


        //item.logroConseguido = (Logro)EditorGUI.EnumPopup(new Rect(rect.x, rect.y, ((rect.width / 3) * 1), new GUIContent("Logro cumplido", "Si ha cumplido este logro mostrará este diálogo"), rect.height), item.logroConseguido);
        //item.texto = EditorGUI.TextArea(new Rect(rect.x + ((rect.width / 3) * 1), rect.y, ((rect.width / 3) * 2), rect.height), item.texto);
        // If you are using a custom PropertyDrawer, this is probably better
        // EditorGUI.PropertyField(rect, serializedObject.FindProperty("list").GetArrayElementAtIndex(index));
        // Although it is probably smart to cach the list as a private variable ;)
    }

    private void AddItem(ReorderableList list)
    {
        zona.pokemons.Add(new PokemonSalvaje());
        EditorUtility.SetDirty(target);
    }

    private void RemoveItem(ReorderableList list)
    {
        zona.pokemons.RemoveAt(list.index);
        EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // Actually draw the list in the inspector
        reorderableList.DoLayoutList();
    }
}
