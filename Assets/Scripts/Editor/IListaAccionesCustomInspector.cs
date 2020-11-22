using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(IListaAcciones),true)]
public class IListaAccionesCustomInspector : Editor {

    private ReorderableList reorderableList;

    private IListaAcciones scriptPrincipal;
    private GUIStyle style;
    private int indiceAnimacion = 0;

    private void OnEnable()
    {
        scriptPrincipal = (IListaAcciones)target;
        if (scriptPrincipal.secuenciaDeAcciones == null)
            scriptPrincipal.secuenciaDeAcciones = new List<Accion>();
        reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("secuenciaDeAcciones"), true, true, true, true);
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight + (18 * 3);
        style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontStyle = FontStyle.Bold;

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
        Accion item = scriptPrincipal.secuenciaDeAcciones[index];

        EditorGUI.BeginChangeCheck();
        item.TipoDeAccion = (AccionSequencia)EditorGUI.EnumPopup(new Rect(rect.x, rect.y, rect.width, 18), new GUIContent("Tipo de Accion", "Se ejecutará nada más entrar el personaje en el área y cumplir las condiciones"), item.TipoDeAccion);

        switch (item.TipoDeAccion)
        {
            case AccionSequencia.NPC_MoverAPosicionEspecifica:
                item.npc = (MovimientoNPC)EditorGUI.ObjectField(new Rect(rect.x, rect.y + 18, rect.width, 18), "NPC", item.npc, typeof(MovimientoNPC), true);
                item.posicion = (Transform)EditorGUI.ObjectField(new Rect(rect.x, rect.y + (18 * 2), rect.width, 18), "Posición", item.posicion, typeof(Transform), true);
                break;
            case AccionSequencia.NPC_MoverASuPosicionInicial:
                item.npc = (MovimientoNPC)EditorGUI.ObjectField(new Rect(rect.x, rect.y + 18, rect.width, 18), "NPC", item.npc, typeof(MovimientoNPC), true);
                break;
            case AccionSequencia.NPC_MoverHastaElPersonaje:
                item.npc = (MovimientoNPC)EditorGUI.ObjectField(new Rect(rect.x, rect.y + 18, rect.width, 18), "NPC", item.npc, typeof(MovimientoNPC), true);
                break;
            case AccionSequencia.NPC_MostrarConversacion:
                item.npc = (MovimientoNPC)EditorGUI.ObjectField(new Rect(rect.x, rect.y + 18, rect.width, 18), "NPC", item.npc, typeof(MovimientoNPC), true);
                break;
            case AccionSequencia.NPC_Ocultar:
                item.npc = (MovimientoNPC)EditorGUI.ObjectField(new Rect(rect.x, rect.y + 18, rect.width, 18), "NPC", item.npc, typeof(MovimientoNPC), true);
                break;
            case AccionSequencia.Personaje_MoverAPosicionEspecífica:
                item.posicion = (Transform)EditorGUI.ObjectField(new Rect(rect.x, rect.y + 18, rect.width, 18), "Posición", item.posicion, typeof(Transform), true);
                break;
            case AccionSequencia.Personaje_TeletransportarAPosicionEspecifica:
                item.posicion = (Transform)EditorGUI.ObjectField(new Rect(rect.x, rect.y + 18, rect.width, 18), "Posición", item.posicion, typeof(Transform), true);
                break;
            case AccionSequencia.Personaje_SeguirNPC:
                item.npc = (MovimientoNPC)EditorGUI.ObjectField(new Rect(rect.x, rect.y + 18, rect.width, 18), "NPC", item.npc, typeof(MovimientoNPC), true);
                break;
            case AccionSequencia.Personaje_DejarDeSeguirNPC:
                break;
            case AccionSequencia.EjecutarMetodos:
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + 18, rect.width, 18), serializedObject.FindProperty("eventoUnity"));
                break;
            case AccionSequencia.EsperarTiempo:
                item.tiempoDeEspera = EditorGUI.FloatField(new Rect(rect.x, rect.y + 18, rect.width, 18), "Tiempo de Espera tras Ejecutar el Evento", item.tiempoDeEspera);
                break;
            case AccionSequencia.ReproducirAnimacion:
                item.animator = (Animator)EditorGUI.ObjectField(new Rect(rect.x, rect.y + 18, rect.width, 18), "Animator Controller", item.animator, typeof(Animator), true);
                if (item.animator != null && item.animator.runtimeAnimatorController.animationClips.Length > 0)
                {
                    indiceAnimacion = EditorGUI.Popup(new Rect(rect.x, rect.y + (18 * 2), rect.width, 18), "Nombre Animación", indiceAnimacion, item.animator.runtimeAnimatorController.animationClips.Select(x => x.name).ToArray());
                    item.nombreAnimacionODialogo = item.animator.runtimeAnimatorController.animationClips[indiceAnimacion].name;
                }
                break;
            case AccionSequencia.MostrarDialogoEspecifico:
                if (string.IsNullOrEmpty(item.nombreAnimacionODialogo))
                    item.nombreAnimacionODialogo = string.Empty;
                item.nombreAnimacionODialogo = EditorGUI.TextArea(new Rect(rect.x, rect.y + 18, rect.width, 18 * 2), item.nombreAnimacionODialogo);
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }

    }

    private void AddItem(ReorderableList list)
    {
        scriptPrincipal.secuenciaDeAcciones.Add(new Accion());        
        EditorUtility.SetDirty(target);        
    }

    private void RemoveItem(ReorderableList list)
    {
        scriptPrincipal.secuenciaDeAcciones.RemoveAt(list.index);
        EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI()
    {
        //if(scriptPrincipal.secuenciaDeAcciones != null)
        //{
        //    bool tieneAccionSeguirPersonaje = false;
        //    bool tieneAccionPararPersonaje = false;

        //    for (int i = 0; i < scriptPrincipal.secuenciaDeAcciones.Count; i++)
        //    {
        //        switch (scriptPrincipal.secuenciaDeAcciones[i].TipoDeAccion)
        //        {
        //            case AccionSequencia.Personaje_SeguirNPC:
        //                tieneAccionSeguirPersonaje = true;
        //                break;
        //            case AccionSequencia.Personaje_DejarDeSeguirNPC:
        //                tieneAccionPararPersonaje = true;
        //                break;
        //        }
        //    }
        //    GUI.backgroundColor = Color.red;
        //    if (tieneAccionSeguirPersonaje && !tieneAccionPararPersonaje)
        //        EditorGUILayout.HelpBox("Se ha especificado una accion de personaje_seguirnpc pero no se ha añadido una de personaje_dejarseguirnpc lo que provocará que el personaje se quede bloqueado sin poder moverse", MessageType.Error);


        //}

        
        GUI.backgroundColor = Color.white;
        scriptPrincipal.tamanioGizmos = EditorGUILayout.Slider("Tamaño Gizmos", scriptPrincipal.tamanioGizmos, 0.01f, 1f);
        scriptPrincipal.mostrarGizmos = EditorGUILayout.Toggle("Ver Gizmos", scriptPrincipal.mostrarGizmos);
        base.OnInspectorGUI();
        GUI.backgroundColor = Color.cyan;
        EditorGUILayout.HelpBox("Secuencia de Acciones", MessageType.Info);
        GUI.backgroundColor = Color.white;
        reorderableList.DoLayoutList();
    }

}