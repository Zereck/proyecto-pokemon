using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;

public class SecuenciaDeAcciones : IListaAcciones {

    [Header("Condiciones")]
    public Logro[] logrosQueDebeTener;
    public Logro[] logrosQueNoDebeTener;

    public bool CumpleSusCondiciones()
    {
        for (int i = 0; i < logrosQueDebeTener.Length; i++)
        {
            if (logrosQueDebeTener[i] == Logro.NINGUNO)
                continue;
            if (!ControladorDatos.Instancia.Datos.ContieneLogro(logrosQueDebeTener[i]))
                return false;
        }

        for (int i = 0; i < logrosQueNoDebeTener.Length; i++)
        {
            if (logrosQueNoDebeTener[i] == Logro.NINGUNO)
                continue;
            if (ControladorDatos.Instancia.Datos.ContieneLogro(logrosQueNoDebeTener[i]))
                return false;
        }

        return true;
    }
}