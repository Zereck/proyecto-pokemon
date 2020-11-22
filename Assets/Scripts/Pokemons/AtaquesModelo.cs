using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AtaquesModelo {

    [SerializeField]
    private AtaqueID id;
    [SerializeField]
    private int pp;

    public AtaquesModelo(AtaqueID id)
    {
        this.id = id;
        this.pp = DatosFijos.ppMaximos;
    }

    public AtaqueID ID
    {
        get
        {
            return id;
        }
    }

    public Ataque DatosFijos
    {
        get
        {
            return ControladorDatos.Instancia.ObtenerAtaque(id);
        }
    }
    
    public string TextoPPActualYMaximo()
    {
        return string.Concat(pp, "/", DatosFijos.ppMaximos);
    }

    public void CentroPokemon()
    {
        pp = DatosFijos.ppMaximos;
    }

    public bool TieneSuficientesPP()
    {
        return pp > 0;
    }

    public void RestarPP()
    {
        pp--;
        if (pp < 0)
            pp = 0;
    }
    
}
