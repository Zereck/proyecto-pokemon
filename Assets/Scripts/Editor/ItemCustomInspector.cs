using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Item))]
public class ItemCustomInspector : Editor {

    private Item scriptPrincipal;

    private void OnEnable()
    {
        scriptPrincipal = (Item)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if(scriptPrincipal.tipoDeItem == TipoDeItem.Curacion)
        {
            scriptPrincipal.cantidadSanacion = EditorGUILayout.IntField("Cantidad de Salud Resturada", scriptPrincipal.cantidadSanacion);
            scriptPrincipal.restaurarEstadoAlterado = (EstadoAlterado)EditorGUILayout.EnumPopup("Estado Alterado Restaurado", scriptPrincipal.restaurarEstadoAlterado);
        }
        else if (scriptPrincipal.tipoDeItem == TipoDeItem.Pokeball)
        {
            scriptPrincipal.posibilidadCaptura = EditorGUILayout.IntSlider(new GUIContent("Posibilidad Captura", "Posibilidad de capturar un pokemon del mismo nivel, incrementa o decrementa según el nivel"), scriptPrincipal.posibilidadCaptura, 20, 100);
        }
        //else if(scriptPrincipal.tipoDeItem == TipoDeItem.MT)
        //{
        //    scriptPrincipal.enseñaAtaque = (AtaqueID)EditorGUILayout.EnumPopup("Enseña Ataque", scriptPrincipal.enseñaAtaque);
        //}
        EditorUtility.SetDirty(target);
    }    
}
