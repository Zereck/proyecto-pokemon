using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AjustarAlGrid))]
public class AjustarAlGridCustomInspector : Editor {
    
    private AjustarAlGrid scriptPrincipal;
    private float restoX;
    private float restoY;
    private float newPositionX;
    private float newPositionY;
    private float centroCasilla;

    private void OnEnable()
    {
        scriptPrincipal = (AjustarAlGrid)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Ajustar al Grid"))
        {
            AutoAjustar();
        }
    }

    private void AutoAjustar()
    {
        centroCasilla = Ajustes.Instancia.tamanioCasilla / 2;
        restoX = scriptPrincipal.gameObject.transform.position.x % Ajustes.Instancia.tamanioCasilla;
        restoY = scriptPrincipal.gameObject.transform.position.y % Ajustes.Instancia.tamanioCasilla;
        
        if (restoX != centroCasilla)
        {
            newPositionX = (scriptPrincipal.gameObject.transform.position.x - restoX) - centroCasilla;
        }
        else
        {
            newPositionX = scriptPrincipal.transform.position.x;
        }
        
        if (restoY != centroCasilla)
        {
            newPositionY = (scriptPrincipal.gameObject.transform.position.y - restoY) - centroCasilla;
        }
        else
        {
            newPositionY = scriptPrincipal.transform.position.y;
        }
        
        scriptPrincipal.gameObject.transform.position = new Vector2(newPositionX, newPositionY);

    }
}
