using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutoAjustarAlGrid))]
public class AutoAjustarAlGridCustomInspector : Editor {

    private AutoAjustarAlGrid scriptPrincipal;
    private float restoX;
    private float restoY;
    private float newPositionX;
    private float newPositionY;
    private float centroCasilla;

    private void OnEnable()
    {
        scriptPrincipal = (AutoAjustarAlGrid)target;
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

    void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            AutoAjustar();
        }
    }
}
