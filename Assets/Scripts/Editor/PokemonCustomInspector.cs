using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditorInternal;

[CustomEditor(typeof(Pokemon))]
public class PokemonCustomInspector : Editor {

    private Pokemon scriptPrincipal;
    private ReorderableList reorderableList;

    private void OnEnable()
    {
        scriptPrincipal = (Pokemon)target;
        if (scriptPrincipal.listaDeAtaques == null)
            scriptPrincipal.listaDeAtaques = new List<AtaqueReferencia>();
        reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("listaDeAtaques"), true, true, true, true);
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight + 36;
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

    private void DrawHeader(Rect rect)
    {
        GUI.Label(rect, "Lista de Ataques");
    }

    private void DrawElement(Rect rect, int index, bool active, bool focused)
    {
        AtaqueReferencia item = scriptPrincipal.listaDeAtaques[index];

        EditorGUI.BeginChangeCheck();

        item.ataque = (AtaqueID)EditorGUI.EnumPopup(new Rect(rect.x, rect.y, rect.width, 18), "Ataque", item.ataque);
        item.nivelAprender = EditorGUI.IntSlider(new Rect(rect.x, rect.y + 18, rect.width, 18), "Nivel", item.nivelAprender, 0, Ajustes.nivelMaximo);
        
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void AddItem(ReorderableList list)
    {
        scriptPrincipal.listaDeAtaques.Add(new AtaqueReferencia());
        EditorUtility.SetDirty(target);
    }

    private void RemoveItem(ReorderableList list)
    {
        scriptPrincipal.listaDeAtaques.RemoveAt(list.index);
        EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI()
    {
        if (scriptPrincipal.ID == PokemonID.NINGUNO)
            EditorGUILayout.HelpBox("No has asignado un ID", MessageType.Error);
        if (string.IsNullOrEmpty(scriptPrincipal.nombre))
            EditorGUILayout.HelpBox("No has asignado un nombre", MessageType.Error);
        if (string.IsNullOrEmpty(scriptPrincipal.descripcion))
            EditorGUILayout.HelpBox("No has asignado una descripción", MessageType.Error);
        if (scriptPrincipal.tipoElemento1 == Elemento.NINGUNO && scriptPrincipal.tipoElemento2 == Elemento.NINGUNO)
            EditorGUILayout.HelpBox("El Pokémon debe tener asignado 1 elemento como mínimo", MessageType.Error);
        if (scriptPrincipal.saludMaxima < 100 || scriptPrincipal.ataqueFisicoBase < 100 || scriptPrincipal.ataqueMagicoBase < 100 || scriptPrincipal.defensaFisicaBase < 100 || scriptPrincipal.defensaMagicaBase < 100 || scriptPrincipal.velocidadBase < 100)
            EditorGUILayout.HelpBox("Las estadísticas máximas son muy bajas", MessageType.Warning);
        if (scriptPrincipal.evolucion != PokemonID.NINGUNO && scriptPrincipal.nivelEvolucion < 5)
            EditorGUILayout.HelpBox("El nivel de evolución del Pokémon es muy bajo", MessageType.Error);
        if (scriptPrincipal.listaDeAtaques != null && scriptPrincipal.listaDeAtaques.Count == 0)
            EditorGUILayout.HelpBox("No se han asignado ataques", MessageType.Error);
        if (scriptPrincipal.sprite == null)
            EditorGUILayout.HelpBox("No se ha asignado un sprite al Pokémon", MessageType.Error);

        base.OnInspectorGUI();
        // Actually draw the list in the inspector
        reorderableList.DoLayoutList();

    }
}
