using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CustomDiccionario<Clave, Valor> 
{
    [SerializeField]
    private List<Clave> claves = new List<Clave>();

    [SerializeField]
    private List<Valor> valores = new List<Valor>();

    public bool ContieneClave(Clave clave)
    {
        return claves.Contains(clave);
    }

    public void Aniadir(Clave clave, Valor valor)
    {
        if (claves.Contains(clave))
            return;

        claves.Add(clave);
        valores.Add(valor);
    }

    public void Eliminar(Clave clave)
    {
        if (!claves.Contains(clave))
            return;

        int index = claves.IndexOf(clave);

        claves.RemoveAt(index);
        valores.RemoveAt(index);
    }

    public bool IntentarObtenerValor(Clave clave, out Valor valor)
    {
        if (claves.Count != valores.Count)
        {
            claves.Clear();
            valores.Clear();
            valor = default(Valor);
            return false;
        }

        if (!claves.Contains(clave))
        {
            valor = default(Valor);
            return false;
        }

        int index = claves.IndexOf(clave);
        valor = valores[index];

        return true;
    }
    
    public void CambiarValor(Clave clave, Valor valor)
    {
        if (!claves.Contains(clave))
            return;

        int index = claves.IndexOf(clave);

        valores[index] = valor;
    }

    public Dictionary<Clave, Valor> ConvertirADiccionario()
    {
        Dictionary<Clave, Valor> diccionario = new Dictionary<Clave, Valor>();
        for (int i = 0; i < claves.Count; i++)
        {
            diccionario.Add(claves[i], valores[i]);
        }
        return diccionario;
    }
}
