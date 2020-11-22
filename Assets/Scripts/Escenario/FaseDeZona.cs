using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FaseDeZona : MonoBehaviour {

    [Header("Si tiene los logros...")]
    public Logro[] logrosConseguidos;
    [Header("Y NO tiene los logros...")]
    public Logro[] logrosNoConseguidos;
    [Header("Activa...")]
    public GameObject[] activarObjetos;
    [Header("Y desactiva...")]
    public GameObject[] desactivarObjetos;

    public bool CumpleLasCondiciones()
    {
        for (int i = 0; i < logrosConseguidos.Length; i++)
        {
            if (logrosConseguidos[i] == Logro.NINGUNO)
                continue;
            if (!ControladorDatos.Instancia.Datos.ContieneLogro(logrosConseguidos[i]))
                return false;
        }

        for (int i = 0; i < logrosNoConseguidos.Length; i++)
        {
            if (logrosNoConseguidos[i] == Logro.NINGUNO)
                continue;
            if (ControladorDatos.Instancia.Datos.ContieneLogro(logrosNoConseguidos[i]))
                return false;
        }

        return true;
    }

    public void ActivarFase()
    {
        for (int i = 0; i < activarObjetos.Length; i++)
        {
            activarObjetos[i].SetActive(true);
        }
        for (int i = 0; i < desactivarObjetos.Length; i++)
        {
            desactivarObjetos[i].SetActive(false);
        }
    }
}
