using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Datos {

    [SerializeField]
    private int _monedas;
    [SerializeField]
    private List<Logro> _logrosConseguidos = new List<Logro>();
    [SerializeField]
    private List<string> _itemDeMundoObtenidos = new List<string>();
    //Clase Personalizada para poder guardar diccionarios en Json
    [SerializeField]
    private CustomDiccionario<ItemID, int> _inventario = new CustomDiccionario<ItemID, int>();
    //Clase Personalizada para poder guardar diccionarios en Json
    [SerializeField]
    private CustomDiccionario<PokemonID, PokedexTipoAvistamiento> _pokedex = new CustomDiccionario<PokemonID, PokedexTipoAvistamiento>();
    [SerializeField]
    public Vector2 posicion;
    [SerializeField]
    private PokemonModelo[] _equipoPokemon = new PokemonModelo[6];
    [SerializeField]
    private List<PokemonModelo> _pokemonAlmacenadosEnPC = new List<PokemonModelo>();

    //public Datos(Vector2 posicionInicial)
    //{
    //    posicion = posicionInicial;
    //}

    public void AniadirLogro(Logro logro)
    {
        if (!_logrosConseguidos.Contains(logro))
        {
            Debug.Log(string.Concat("Añadido logro ", logro.ToString()));
            _logrosConseguidos.Add(logro);
            ControladorEventos.Instancia.LanzarEvento(new EventoNuevoLogroConseguido(logro));
        }
        else
            Debug.LogWarning(string.Concat("El logro ", logro.ToString(), " ya había sido añadido"));
    }

    public bool ContieneLogro(Logro logro)
    {
        if (_logrosConseguidos.Contains(logro))
            return true;
        else
            return false;
    }

    public void ItemDeMundoConseguido(string nombre)
    {
        if (!_itemDeMundoObtenidos.Contains(nombre))
        {
            Debug.Log(string.Concat("Item de mundo conseguido ", nombre));
            _itemDeMundoObtenidos.Add(nombre);
        }
        else
            Debug.LogWarning(string.Concat("El item de mundo ", nombre, " ya había sido añadido"));
    }

    public bool ItemDeMUndoYaConseguido(string nombre)
    {
        if (_itemDeMundoObtenidos.Contains(nombre))
            return true;
        else
            return false;
    }


    public void AniadirNuevoPokemonCapturado(PokemonModelo pokemon)
    {
        if (_pokedex.ContieneClave(pokemon.ID))
            _pokedex.CambiarValor(pokemon.ID, PokedexTipoAvistamiento.Capturado);
        else
            _pokedex.Aniadir(pokemon.ID, PokedexTipoAvistamiento.Capturado);
        
        if (HayEspacioEnElEquipo())
        {
            AniadirPokemonAlEquipo(pokemon);
        }
        else
        {
            AniadirPokemonAlPC(pokemon);
        }
    }

    private bool HayEspacioEnElEquipo()
    {
        for (int i = 0; i < _equipoPokemon.Length; i++)
        {
            if (_equipoPokemon[i] == null || _equipoPokemon[i].ID == PokemonID.NINGUNO)
                return true;
        }
        return false;
    }

    private void AniadirPokemonAlEquipo(PokemonModelo pokemon)
    {
        for (int i = 0; i < _equipoPokemon.Length; i++)
        {
            if (_equipoPokemon[i] == null || _equipoPokemon[i].ID == PokemonID.NINGUNO)
            {
                _equipoPokemon[i] = pokemon;
                break;
            }
        }
    }

    public int NumeroDePokemonEnElEquipo()
    {
        int numero = 0;
        for (int i = 0; i < _equipoPokemon.Length; i++)
        {
            if (_equipoPokemon[i] != null && _equipoPokemon[i].ID != PokemonID.NINGUNO)
            {
                numero++;
            }
        }
        return numero;
    }


    public bool SacarPokemonDelPC(PokemonModelo pokemon)
    {
        if (HayEspacioEnElEquipo())
        {
            bool existe = false;
            if (_pokemonAlmacenadosEnPC != null && _pokemonAlmacenadosEnPC.Count > 0)
            {
                for (int i = 0; i < _pokemonAlmacenadosEnPC.Count; i++)
                {
                    if (_pokemonAlmacenadosEnPC[i].IdentificardorUnico == pokemon.IdentificardorUnico)
                    {
                        _pokemonAlmacenadosEnPC.RemoveAt(i);
                        existe = true;
                        break;
                    }
                }

            }

            if (!existe) return false;
            AniadirPokemonAlEquipo(pokemon);
            return true;
        }
        else
        {
            return false;
        }
    }


    public bool DejarPokemonDelEquipoEnPC(PokemonModelo pokemon)
    {
        if (NumeroDePokemonEnElEquipo() < 2)
        {
            return false;
        }
        else
        {
            if (!EliminarPokemonDelEquipo(pokemon)) return false;
            if (!AniadirPokemonAlPC(pokemon)) return false;
            return true;
        }
    }

    private bool EliminarPokemonDelEquipo(PokemonModelo pokemon)
    {
        if (pokemon == null) return false;


        for (int i = 0; i < _equipoPokemon.Length; i++)
        {
            if(_equipoPokemon[i] != null && _equipoPokemon[i].IdentificardorUnico == pokemon.IdentificardorUnico)
            {
                _equipoPokemon[i] = null;
                return true;
            }
        }
        Debug.Log("No existe el pokemon en el equipo");
        return false;
    }

    private bool AniadirPokemonAlPC(PokemonModelo pokemon)
    {
        if (_pokemonAlmacenadosEnPC != null && _pokemonAlmacenadosEnPC.Count > 0)
        {
            for (int i = 0; i < _pokemonAlmacenadosEnPC.Count; i++)
            {
                if (_pokemonAlmacenadosEnPC[i].IdentificardorUnico == pokemon.IdentificardorUnico)
                {
                    Debug.Log("El pokemon ya existe en el PC");
                    return false;
                }
            }
        }

        _pokemonAlmacenadosEnPC.Add(pokemon);
        return true;
    }

    public void ReorganizarEquipoPokemon(PokemonModelo[] nuevoOrdenPokemon)
    {
        _equipoPokemon = nuevoOrdenPokemon;
    }

    public void AniadirItemEncontradoAlInventario(ItemID item, int cantidad)
    {
        Debug.Log(string.Concat("Añadido al inventario el objeto ", item.ToString(), " x ", cantidad));
        int cantidadActual = 0;
        if (_inventario.IntentarObtenerValor(item, out cantidadActual))
        {
            cantidadActual += cantidad;
            _inventario.CambiarValor(item, cantidadActual);
        }
        else
        {
            _inventario.Aniadir(item, cantidad);
        }
    }


    public PokemonModelo[] ObtenerEquipoPokemon()
    {
        return _equipoPokemon;
    }

    public PokemonModelo[] ObtenerPokemonEnPC()
    {
        return _pokemonAlmacenadosEnPC.ToArray();
    }

    public void AniadirMonedas(int cantidad)
    {
        _monedas = Mathf.Clamp(_monedas + cantidad, 0, 9999);
    }

    //public bool TieneDineroSuficiente(ItemID item, int cantidad)
    //{
    //    int precioTotal = ControladorDatos.Instancia.ObtenerItem(item).precioEnTienda * cantidad;
    //    if (_monedas >= precioTotal)
    //        return true;
    //    return false;
    //}

    public bool TieneNumeroMaximoItem(ItemID item)
    {
        int cantidadActual = 0;
        _inventario.IntentarObtenerValor(item, out cantidadActual);
        if (cantidadActual < 99)
            return false;
        return true;
    }

    public void ComprarItem(Item item, int cantidad)
    {
        cantidad = Mathf.Clamp(cantidad, 0, 99);
        if (_inventario.ContieneClave(item.ID))
        {
            int cantidadActual = 0;
            _inventario.IntentarObtenerValor(item.ID, out cantidadActual);
            _inventario.CambiarValor(item.ID, Mathf.Clamp(cantidadActual + cantidad, 0, 99));
        }
        else
        {
            _inventario.Aniadir(item.ID, cantidad);
        }

        int precioTotal = item.precioEnTienda * cantidad;
        _monedas = Mathf.Clamp(_monedas - precioTotal, 0, 9999);
    }

    public void VenderItem(Item item, int cantidad)
    {
        cantidad = Mathf.Clamp(cantidad, 0, 99);
        if (_inventario.ContieneClave(item.ID))
        {
            int cantidadActual = 0;
            _inventario.IntentarObtenerValor(item.ID, out cantidadActual);
            _inventario.CambiarValor(item.ID, Mathf.Clamp(cantidadActual - cantidad, 0, 99));
            int precioTotal = item.precioEnTienda * cantidad;
            _monedas = Mathf.Clamp(_monedas + precioTotal, 0, 9999);
        }

    }

    public void ItemUsado(ItemID item)
    {
        if (_inventario.ContieneClave(item))
        {
            int cantidadActual = 0;
            _inventario.IntentarObtenerValor(item, out cantidadActual);
            _inventario.CambiarValor(item, Mathf.Clamp(cantidadActual - 1, 0, 99));
        }
    }

    public bool TieneItem(ItemID item)
    {
        if (_inventario.ContieneClave(item))
        {
            int cantidadActual = 0;
            _inventario.IntentarObtenerValor(item, out cantidadActual);
            if (cantidadActual > 0)
                return true;
        }
        return false;
    }

    public void CombatePerdidoQuitarMitadMonedas()
    {
        _monedas /= 2;
    }

    public int Monedas { get { return _monedas; } }

    public PokedexTipoAvistamiento Pokedex(PokemonID id)
    {
        PokedexTipoAvistamiento tipo = PokedexTipoAvistamiento.NINGUNO;

        if(_pokedex.ContieneClave(id))
            _pokedex.IntentarObtenerValor(id, out tipo);
        return tipo;
    }

    public void PokemonVisto(PokemonID id)
    {
        if (!_pokedex.ContieneClave(id))
        {
            _pokedex.Aniadir(id, PokedexTipoAvistamiento.Visto);
        }
    }

    public Dictionary<ItemID, int> ObtenerInventario()
    {
        return _inventario.ConvertirADiccionario();
    }

    public void CentroPokemon()
    {
        for (int i = 0; i < _equipoPokemon.Length; i++)
        {
            if(_equipoPokemon[i] != null && _equipoPokemon[i].ID != PokemonID.NINGUNO)
            {
                _equipoPokemon[i].CentroPokemon();
            }
        }
    }    
}
