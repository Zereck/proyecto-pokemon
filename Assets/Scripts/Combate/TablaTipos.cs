using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablaTipos {

    public float MultiplicadorElemento(Elemento ataque, Elemento elemento1Pokemon, Elemento elemento2Pokemon)
    {
        float multiplicadorDanio = 1;
        if (elemento1Pokemon != Elemento.NINGUNO)
            multiplicadorDanio *= MultiplicadorTipos(ataque, elemento1Pokemon);
        if(elemento2Pokemon != Elemento.NINGUNO)
            multiplicadorDanio *= MultiplicadorTipos(ataque, elemento2Pokemon);
        return multiplicadorDanio;
    }

    private float MultiplicadorTipos(Elemento ataque, Elemento pokemon)
    {
        switch (ataque)
        {
            case Elemento.Agua:
                switch (pokemon)
                {
                    //DOBLE
                    case Elemento.Roca:
                    case Elemento.Tierra:
                    case Elemento.Fuego:
                        return 2;
                    //MITAD
                    case Elemento.Agua:
                    case Elemento.Dragon:
                    case Elemento.Planta:
                        return 0.5f;
                    //NORMAL
                    default:
                        return 1;
                }
            case Elemento.Fuego:
                switch (pokemon)
                {
                    //DOBLE
                    case Elemento.Bicho:
                    case Elemento.Hielo:
                    case Elemento.Planta:
                        return 2;
                    //MITAD
                    case Elemento.Agua:
                    case Elemento.Dragon:
                    case Elemento.Roca:
                    case Elemento.Fuego:
                        return 0.5f;
                    //NORMAL
                    default:
                        return 1;
                }
            case Elemento.Planta:
                switch (pokemon)
                {
                    //DOBLE
                    case Elemento.Roca:
                    case Elemento.Tierra:
                    case Elemento.Agua:
                        return 2;
                    //MITAD
                    case Elemento.Fuego:
                    case Elemento.Volador:
                    case Elemento.Bicho:
                    case Elemento.Veneno:
                    case Elemento.Planta:
                    case Elemento.Dragon:
                        return 0.5f;
                    //NORMAL
                    default:
                        return 1;
                }
            case Elemento.Fantasma:
                switch (pokemon)
                {
                    //DOBLE
                    case Elemento.Fantasma:
                    case Elemento.Psiquico:
                        return 2;
                    //INMUNE
                    case Elemento.Normal:
                        return 0;
                    //NORMAL
                    default:
                        return 1;
                }
            case Elemento.Psiquico:
                switch (pokemon)
                {
                    //DOBLE
                    case Elemento.Lucha:
                    case Elemento.Veneno:
                        return 2;
                    //MITAD
                    case Elemento.Psiquico:
                        return 0.5f;
                    //NORMAL
                    default:
                        return 1;
                }
            case Elemento.Electrico:
                switch (pokemon)
                {
                    //DOBLE
                    case Elemento.Volador:
                    case Elemento.Agua:
                        return 2;
                    //MITAD
                    case Elemento.Electrico:
                    case Elemento.Planta:
                    case Elemento.Dragon:
                        return 0.5f;
                    //INMUNE
                    case Elemento.Tierra:
                        return 0;
                    //NORMAL
                    default:
                        return 1;
                }
            case Elemento.Volador:
                switch (pokemon)
                {
                    //DOBLE
                    case Elemento.Lucha:
                    case Elemento.Bicho:
                    case Elemento.Planta:
                        return 2;
                    //MITAD
                    case Elemento.Roca:
                    case Elemento.Electrico:
                        return 0.5f;
                    //NORMAL
                    default:
                        return 1;
                }
            case Elemento.Normal:
                switch (pokemon)
                {
                    //MITAD
                    case Elemento.Roca:
                        return 0.5f;
                    //INMUNE
                    case Elemento.Fantasma:
                        return 0;
                    //NORMAL
                    default:
                        return 1;
                }
            case Elemento.Roca:
                switch (pokemon)
                {
                    //DOBLE
                    case Elemento.Volador:
                    case Elemento.Bicho:
                    case Elemento.Fuego:
                    case Elemento.Hielo:
                        return 2;
                    //MITAD
                    case Elemento.Lucha:
                    case Elemento.Tierra:
                        return 0.5f;
                    //NORMAL
                    default:
                        return 1;
                }
            case Elemento.Tierra:
                switch (pokemon)
                {
                    //DOBLE
                    case Elemento.Veneno:
                    case Elemento.Roca:
                    case Elemento.Fuego:
                    case Elemento.Electrico:
                        return 2;
                    //MITAD
                    case Elemento.Planta:
                    case Elemento.Bicho:
                        return 0.5f;
                    //INMUNE
                    case Elemento.Volador:
                        return 0;
                    //NORMAL
                    default:
                        return 1;
                }
            case Elemento.Veneno:
                switch (pokemon)
                {
                    //DOBLE
                    case Elemento.Planta:
                        return 2;
                    //MITAD
                    case Elemento.Veneno:
                    case Elemento.Tierra:
                    case Elemento.Roca:
                    case Elemento.Fantasma:
                        return 0.5f;
                    //NORMAL
                    default:
                        return 1;
                }
            case Elemento.Hielo:
                switch (pokemon)
                {
                    //DOBLE
                    case Elemento.Volador:
                    case Elemento.Tierra:
                    case Elemento.Planta:
                    case Elemento.Dragon:
                        return 2;
                    //MITAD
                    case Elemento.Fuego:
                    case Elemento.Agua:
                    case Elemento.Hielo:
                        return 0.5f;
                    //NORMAL
                    default:
                        return 1;
                }
            case Elemento.Dragon:
                switch (pokemon)
                {
                    //DOBLE
                    case Elemento.Dragon:
                        return 2;                    
                    //NORMAL
                    default:
                        return 1;
                }
            case Elemento.Lucha:
                switch (pokemon)
                {
                    //DOBLE
                    case Elemento.Normal:
                    case Elemento.Roca:
                    case Elemento.Hielo:
                        return 2;
                    //MITAD
                    case Elemento.Volador:
                    case Elemento.Veneno:
                    case Elemento.Bicho:
                    case Elemento.Psiquico:
                        return 0.5f;
                    //INMUNE
                    case Elemento.Fantasma:
                        return 0;
                    //NORMAL
                    default:
                        return 1;
                }
            case Elemento.Bicho:
                switch (pokemon)
                {
                    //DOBLE
                    case Elemento.Planta:
                    case Elemento.Psiquico:
                        return 2;
                    //MITAD
                    case Elemento.Lucha:
                    case Elemento.Volador:
                    case Elemento.Veneno:
                    case Elemento.Fantasma:
                    case Elemento.Fuego:
                        return 0.5f;
                    //NORMAL
                    default:
                        return 1;
                }
        }
        return 1;
    }
    

}
