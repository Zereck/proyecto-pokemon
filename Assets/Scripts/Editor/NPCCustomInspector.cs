using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;

[CustomEditor(typeof(NPC))]
public class NPCCUstomInspector : Editor {

    private ReorderableList reorderableList;
    private Direccion direccionPorDefecto = Direccion.Abajo;

    private NPC npc;
    private GUIStyle style;

    private void OnEnable()
    {
        npc = (NPC)target;
        if (npc.listaConversaciones == null)
            npc.listaConversaciones = new System.Collections.Generic.List<Conversacion>();
        reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("listaConversaciones"), true, true, true, true);
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight + (18 * 18);
        style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontStyle = FontStyle.Bold;
        // This could be used aswell, but I only advise this your class inherrits from UnityEngine.Object or has a CustomPropertyDrawer
        // Since you'll find your item using: serializedObject.FindProperty("list").GetArrayElementAtIndex(index).objectReferenceValue
        // which is a UnityEngine.Object
        // reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("list"), true, true, true, true);

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

        GUI.Label(rect, "Lista de conversaciones por logro");
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
        Conversacion item = npc.listaConversaciones[index];

        EditorGUI.BeginChangeCheck();

        item.logroConseguido = (Logro)EditorGUI.EnumPopup(new Rect(rect.x, rect.y, rect.width, 18), new GUIContent("Logro Requerido", "Si ha cumplido este logro mostrará este diálogo"), item.logroConseguido);
        EditorGUI.HelpBox(new Rect(rect.x, rect.y + (18 * 2), rect.width, (18)), "Mensaje NPC. Si es un combate lo dirá antes de combatir", MessageType.Info);
        item.texto = EditorGUI.TextArea(new Rect(rect.x, rect.y + (18 * 3), rect.width, (18 * 4)), item.texto);
        item.tipoConversacion = (TipoConversacion)EditorGUI.EnumPopup(new Rect(rect.x, rect.y + 18, rect.width, 18), new GUIContent("Tipo Conversación"), item.tipoConversacion);
        item.darLogroPorTerminarConversacion = (Logro)EditorGUI.EnumPopup(new Rect(rect.x, rect.y + (18 * 7), rect.width, 18), new GUIContent("Logro al terminar conversacion/combate", "Al terminar la conversación el jugador recibirá este logro"), item.darLogroPorTerminarConversacion);
        item.curarEquipoPokemonDelJugador = EditorGUI.Toggle(new Rect(rect.x, rect.y + (18 * 8), rect.width, 18), "Curar Equipo Pokemon del Jugador", item.curarEquipoPokemonDelJugador);
        if (item.darLogroPorTerminarConversacion != Logro.NINGUNO)
        {
            item.darItemPorTerminarConversacion = (ItemID)EditorGUI.EnumPopup(new Rect(rect.x, rect.y + (18 * 9), rect.width, 18), new GUIContent("Item al terminar conversación/combate", "Al terminar la conversación recibirá este objeto"), item.darItemPorTerminarConversacion);
            if (item.darItemPorTerminarConversacion != ItemID.NINGUNO)
                item.cantidadDeItems = EditorGUI.IntField(new Rect(rect.x, rect.y + (18 * 10), rect.width, 18), "Cantidad del item anterior", item.cantidadDeItems);
        }


        if (item.tipoConversacion == TipoConversacion.Luchar && item.darLogroPorTerminarConversacion != Logro.NINGUNO)
        {
            EditorGUI.HelpBox(new Rect(rect.x, rect.y + (18 * 11), rect.width, (18)), "Mensaje tras ganar el combate", MessageType.Info);
            item.texto2 = EditorGUI.TextArea(new Rect(rect.x, rect.y + (18 * 12), rect.width, (18 * 4)), item.texto2);
            item.monedasRecompensa = EditorGUI.IntField(new Rect(rect.x, rect.y + (18 * 16), rect.width, 18), "Dinero Recompensa", item.monedasRecompensa);
            item.equipoPokemon = (EquipoPokemonEntrenador)EditorGUI.ObjectField(new Rect(rect.x, rect.y + (18 * 17), rect.width, 18), item.equipoPokemon, typeof(EquipoPokemonEntrenador), true);

        }
        else if (item.tipoConversacion == TipoConversacion.Luchar)
        {

            EditorGUI.HelpBox(new Rect(rect.x, rect.y + (18 * 11), rect.width, (18 * 3)), "Un NPC que lucha siempre debe dar un logro al terminar, si no se podrá luchar con él repetidas veces", MessageType.Error);
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }

    }

    private void AddItem(ReorderableList list)
    {
        npc.listaConversaciones.Add(new Conversacion());        
        EditorUtility.SetDirty(target);        
    }

    private void RemoveItem(ReorderableList list)
    {
        npc.listaConversaciones.RemoveAt(list.index);
        EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        direccionPorDefecto = (Direccion)EditorGUILayout.EnumPopup("Direccion Sprite", direccionPorDefecto);
        if (EditorGUI.EndChangeCheck())
            AsignarDireccionPorDefectoSprite();
        // Actually draw the list in the inspector
        reorderableList.DoLayoutList();       
    }

    private void AsignarDireccionPorDefectoSprite()
    {
        switch (direccionPorDefecto)
        {
            case Direccion.PorDefecto:
            case Direccion.Abajo:
                npc.MirarDireccionEditor(Vector2.down);
                break;
            case Direccion.Arriba:
                npc.MirarDireccionEditor(Vector2.up);
                break;
            case Direccion.Izquierda:
                npc.MirarDireccionEditor(Vector2.left);
                break;
            case Direccion.Derecha:
                npc.MirarDireccionEditor(Vector2.right);
                break;
        }

    }
}