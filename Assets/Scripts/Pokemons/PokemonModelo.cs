using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class PokemonModelo
{
    [SerializeField]
    private string _identificardorUnico;
    [SerializeField]
    private PokemonID _id;
    [SerializeField]
    private int _nivel;
    [SerializeField]
    private int _calidadDelPokemon;
    [SerializeField]
    private int _salud;
    [SerializeField]
    private int _experiencia;
    [SerializeField]
    private AtaquesModelo[] _ataques;
    [SerializeField]
    private EstadoAlterado _estadoAlterado;
    
    public bool HaSubidoDeNivel { get; set; }

    public PokemonModelo(PokemonID pokemonID, int nivel, int calidadDelPokemon, AtaqueID[] ataques, float saludEnEscaleDe1)
    {
        _identificardorUnico = Guid.NewGuid().ToString();
        _id = pokemonID;
        _nivel = nivel;
        _experiencia = 0;
        _calidadDelPokemon = Mathf.Clamp(calidadDelPokemon, Ajustes.calidadMinimaPokemon, Ajustes.calidadMaximaPokemon);
        saludEnEscaleDe1 = Mathf.Clamp(saludEnEscaleDe1, 0, 1);
        _salud = (int)((float)EstadisticaSaludMaxima() * (float)saludEnEscaleDe1);
        _ataques = new AtaquesModelo[4];
        AsignarAtaques(ataques);
    }

    private void AsignarAtaques(AtaqueID[] ataques)
    {
        _ataques[0] = null;
        _ataques[1] = null;
        _ataques[2] = null;
        _ataques[3] = null;

        int contador = 0;
        for (int i = 0; i < ataques.Length; i++)
        {
            if (ataques[i] != AtaqueID.NINGUNO && contador < 4)
            {
                _ataques[contador] = new AtaquesModelo(ataques[i]);
                contador++;
            }
        }
    }

    public void AprenderNuevoAtaque(AtaqueID ataqueAnterior, AtaqueID nuevoAtaque)
    {        
        for (int i = 0; i < _ataques.Length; i++)
        {
            if (_ataques[i].ID == ataqueAnterior)
            {
                _ataques[i] = new AtaquesModelo(nuevoAtaque);
                break;
            }
        }
    }

    public void CentroPokemon()
    {
        _estadoAlterado = EstadoAlterado.NINGUNO;
        _salud = EstadisticaSaludMaxima();
        for (int i = 0; i < _ataques.Length; i++)
        {
            if(_ataques[i] != null && _ataques[i].ID != AtaqueID.NINGUNO)
                _ataques[i].CentroPokemon();
        }
    }
    
    public int EstadisticaSaludMaxima()
    {
        return CalcularEstadisticaSalud();
    }

    public int EstadisticaAtaqueFisico()
    {
        return CalcularEstadistica(DatosFijos.ataqueFisicoBase);
    }

    public int EstadisticaDefensaFisica()
    {
        return CalcularEstadistica(DatosFijos.defensaFisicaBase);
    }

    public int EstadisticaAtaqueMagico()
    {
        return CalcularEstadistica(DatosFijos.ataqueMagicoBase);
    }

    public int EstadisticaDefensaMagica()
    {
        return CalcularEstadistica(DatosFijos.defensaMagicaBase);
    }

    public int EstadisticaVelocidadCombate()
    {
        int velocidad = CalcularEstadistica(DatosFijos.velocidadBase);
        if (_estadoAlterado == EstadoAlterado.Paralizado)
            velocidad /= 2;
        return velocidad;
    }

    public int EstadisticaVelocidad()
    {
        return CalcularEstadistica(DatosFijos.velocidadBase);
    }

    private int CalcularEstadistica(int valorBaseEstadistica)
    {
        return (((valorBaseEstadistica + _calidadDelPokemon) * 2 * _nivel) / Ajustes.nivelMaximo) + 5;
    }

    private int CalcularEstadisticaSalud()
    {
        return (((DatosFijos.saludMaxima + _calidadDelPokemon) * 2 * _nivel) / Ajustes.nivelMaximo) + _nivel + 10;
    }

    public AtaquesModelo Ataque(AtaqueID id)
    {
        for (int i = 0; i < _ataques.Length; i++)
        {
            if (_ataques[i].ID == id)
                return _ataques[i];
        }
        return null;
    }

    public AtaquesModelo[] Ataques()
    {
        return _ataques;
    }

    public PokemonID ID
    {
        get
        {
            return _id;
        }
    }

    public bool PuedeEvolucionar()
    {
        if (DatosFijos.evolucion != PokemonID.NINGUNO && DatosFijos.nivelEvolucion < Nivel)
            return true;
        return false;
    }

    public int IncrementarXP(int cantidad)
    {
        int xpSobrante = -1; //Para realizar la animación de incrementar el nivel 1 a 1, si sube de nivel se retornará los puntos de experiencia sobrantes para volver a asignarselos
        int xpMaximoNivelActual = (int)PuntosExperienciaMaximosNivel();
        int cantidadXPConIncremento = _experiencia + cantidad;
        if (cantidadXPConIncremento > xpMaximoNivelActual)
        {
            _experiencia = 0;
            xpSobrante = cantidadXPConIncremento - xpMaximoNivelActual;
            IncrementarNivel();
        }
        else
        {
            _experiencia = cantidadXPConIncremento;
        }
        return xpSobrante;
    }

    public float ExperienciaEnEscalaDe1()
    {
        return Mathf.Clamp((float)_experiencia / PuntosExperienciaMaximosNivel(), 0, 1);
    }

    private void IncrementarNivel()
    {
        _nivel = Mathf.Clamp(Nivel + 1, Ajustes.nivelMinimo, Ajustes.nivelMaximo);
        HaSubidoDeNivel = true;
    }

    private float PuntosExperienciaMaximosNivel()
    {
        return Mathf.Pow(Nivel, 3);
    }

    public void ReducirSalud(int danio)
    {
        _salud = Mathf.Clamp(_salud - danio, 0, EstadisticaSaludMaxima());
        if (_salud <= 0)
            _estadoAlterado = EstadoAlterado.Derrotado;
    }

    public void RestaurarSalud(int cantidadSanada)
    {
        if(EstaVivo())
            _salud = Mathf.Clamp(_salud + cantidadSanada, 0, EstadisticaSaludMaxima());
    }

    public bool EstaVivo()
    {
        return _estadoAlterado != EstadoAlterado.Derrotado;
    }

    public float SaludEnEscalaDe1()
    {
        return Mathf.Clamp((float)Salud / (float)EstadisticaSaludMaxima(), 0, 1);
    }


    public bool TieneLaSaludCompleta()
    {
        return Salud == EstadisticaSaludMaxima();
    }

    public EstadoAlterado EstadoAlterado
    {
        get
        {
            return _estadoAlterado;
        }
        set
        {
            _estadoAlterado = value;
        }
    }

    public string IdentificardorUnico
    {
        get
        {
            return _identificardorUnico;
        }
    }

    public Pokemon DatosFijos
    {
        get
        {
            return ControladorDatos.Instancia.ObtenerPokemon(_id);
        }
    }

    public int Nivel
    {
        get
        {
            return _nivel;
        }
    }

    public int Salud
    {
        get
        {
            return _salud;
        }
    }

    public override string ToString()
    {
        string ataquesID = string.Empty;
        for (int i = 0; i < _ataques.Length; i++)
        {
            if(_ataques[i] != null)
                ataquesID = string.Concat(ataquesID, _ataques[i].ID.ToString());
        }
        return string.Concat(" - ID ", ID.ToString(), " - Nivel ", Nivel.ToString(), " - Calidad ", _calidadDelPokemon.ToString(), " - Salud ", Salud, "/", EstadisticaSaludMaxima(),
            " - Ataque Físico ", EstadisticaAtaqueFisico(), " - Defensa Física ", EstadisticaDefensaFisica(), " - Ataque Mágico ", EstadisticaAtaqueMagico(), " - Defensa Mágica ", EstadisticaDefensaMagica(), 
            " - Velocidad ", EstadisticaVelocidad(), " - Ataques ", ataquesID, " - GUID: ", _identificardorUnico);
    }

    public IncrementoEstadisticas IncrementoDeEstadisticasRespectoAlNivelAnterior()
    {
        return new IncrementoEstadisticas
        {
            saludIncremento = CalcularEstadisticaSalud() - CalcularEstadisticaSaludNivelAnterior(),
            ataqueFisicoIncremento = CalcularEstadistica(DatosFijos.ataqueFisicoBase) - CalcularEstadistincaNivelAnterior(DatosFijos.ataqueFisicoBase),
            defensaFisicaIncremento = CalcularEstadistica(DatosFijos.defensaFisicaBase) - CalcularEstadistincaNivelAnterior(DatosFijos.defensaFisicaBase),
            ataqueMagicoIncremento = CalcularEstadistica(DatosFijos.ataqueMagicoBase) - CalcularEstadistincaNivelAnterior(DatosFijos.ataqueMagicoBase),
            defensaMagicaIncremento = CalcularEstadistica(DatosFijos.defensaMagicaBase) - CalcularEstadistincaNivelAnterior(DatosFijos.defensaMagicaBase),
            velocidadIncremento = CalcularEstadistica(DatosFijos.velocidadBase) - CalcularEstadistincaNivelAnterior(DatosFijos.velocidadBase)
        };
    }

    private int CalcularEstadistincaNivelAnterior(int estadisticaBase)
    {
        return (((DatosFijos.ataqueFisicoBase + _calidadDelPokemon) * 2 * (_nivel - 1)) / Ajustes.nivelMaximo) + 5;
    }

    private int CalcularEstadisticaSaludNivelAnterior()
    {
        return (((DatosFijos.saludMaxima + _calidadDelPokemon) * 2 * (_nivel - 1)) / Ajustes.nivelMaximo) + (_nivel - 1) + 10;
    }

    public void Evolucionar()
    {
        if (DatosFijos.evolucion != PokemonID.NINGUNO && DatosFijos.nivelEvolucion <= Nivel)
        {
            _id = DatosFijos.evolucion;
        }
    }
}

public struct IncrementoEstadisticas
{
    public int saludIncremento;
    public int ataqueFisicoIncremento;
    public int defensaFisicaIncremento;
    public int ataqueMagicoIncremento;
    public int defensaMagicaIncremento;
    public int velocidadIncremento;

}