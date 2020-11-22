using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Ataque))]
public class AtaqueCustomInspector : Editor {

    private Ataque scriptPrincipal;

    private void OnEnable()
    {
        scriptPrincipal = (Ataque)target;
    }

    public override void OnInspectorGUI()
    {
        if (scriptPrincipal.ID == AtaqueID.NINGUNO)
            EditorGUILayout.HelpBox("No se ha asignado un ID", MessageType.Error);
        if(string.IsNullOrEmpty(scriptPrincipal.nombre))
            EditorGUILayout.HelpBox("No se ha asignado un nombre", MessageType.Error);
        if (scriptPrincipal.ataqueElemento == Elemento.NINGUNO)
            EditorGUILayout.HelpBox("No se ha asignado un elemento", MessageType.Error);

        base.OnInspectorGUI();
    }    
}
