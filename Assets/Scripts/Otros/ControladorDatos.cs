using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ControladorDatos : MonoBehaviour {

    public static ControladorDatos Instancia { get; private set; }

    public AudioSource audioMusica;
    public AudioSource audioSonidos;

    private Datos datos;
    private VinculadorResources vinculadorResources;
    private Dictionary<ItemID, Item> contenedorItems = new Dictionary<ItemID, Item>();
    private Dictionary<PokemonID, Pokemon> contenedorPokemons = new Dictionary<PokemonID, Pokemon>();
    private Dictionary<AtaqueID, Ataque> contenedorAtaques = new Dictionary<AtaqueID, Ataque>();
    private Dictionary<SonidoID, Sonido> contenedorSonidos = new Dictionary<SonidoID, Sonido>();
    private Dictionary<MusicaID, Musica> contenedorMusica = new Dictionary<MusicaID, Musica>();
    private MusicaID proximaMusica = MusicaID.NINGUNO;
    private MusicaID musicaActual = MusicaID.NINGUNO;
    private Queue<ColaCorrutina> colaCorrutinas = new Queue<ColaCorrutina>();
    private Dictionary<string, IEnumerator> corrutinasParables = new Dictionary<string, IEnumerator>();
    private ColaCorrutina corrutinaActual;
    private IEnumerator corrutinaMusica;

    private void Awake()
    {
        Singleton();
        CargarVinculadorFicherosResources();
        CargarPartida();
    }

    private void OnEnable()
    {
        StartCoroutine(EncoladorCorrutinas());
    }
    
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Singleton()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void CargarPartida()
    {
        string rutaFichero = Path.Combine(Application.persistentDataPath, Ajustes.nombreFicheroPartida + ".json");
        if (File.Exists(rutaFichero))
        {
            string datosEnJson = File.ReadAllText(rutaFichero);
            datos = JsonUtility.FromJson<Datos>(datosEnJson);
        }
        else
        {
            datos = new Datos();
            //datos = new Datos(Ajustes.Instancia.posicionInicioPersonaje);
        }
    }
    
    public void GuardarPartida()
    {
        string jsonDatosPartida = JsonUtility.ToJson(datos);
        string rutaFichero = Path.Combine(Application.persistentDataPath, Ajustes.nombreFicheroPartida + ".json");
        File.WriteAllText(rutaFichero, jsonDatosPartida);
    }

    public Datos Datos
    {
        get
        {
            return datos;
        }
    }

    public void AniadirCorrutinaACola(IEnumerator corrutina, bool detenerHastaQueFinalice = true, string idParaDetener = "")
    {
        colaCorrutinas.Enqueue(new ColaCorrutina() { corrutina = corrutina, detenerHastaQueFinalice = detenerHastaQueFinalice, idParaDetener = idParaDetener });
    }

    public void DetenerCorrutina(string id)
    {
        if (corrutinasParables.ContainsKey(id))
        {
            StopCoroutine(corrutinasParables[id]);
            corrutinasParables.Remove(id);
        }
    }

    public void DetenerCorrutinaActual()
    {
        if (corrutinaActual.corrutina != null)
            StopCoroutine(corrutinaActual.corrutina);
    }

    public int CorrutinasEnCola()
    {
        return colaCorrutinas.Count;
    }

    private IEnumerator EncoladorCorrutinas()
    {
        while (true)
        {
            if(colaCorrutinas.Count > 0)
            {
                Personaje.PuedeMoverse = false;
                while (!Personaje.BloquearMovimiento)
                    yield return null;

                corrutinaActual = colaCorrutinas.Dequeue();
                if (!string.IsNullOrEmpty(corrutinaActual.idParaDetener) && !corrutinasParables.ContainsKey(corrutinaActual.idParaDetener))
                    corrutinasParables.Add(corrutinaActual.idParaDetener, corrutinaActual.corrutina);
                if (corrutinaActual.detenerHastaQueFinalice)
                    yield return StartCoroutine(corrutinaActual.corrutina);
                else
                    StartCoroutine(corrutinaActual.corrutina);
            }
            else if(!Personaje.UIAbierta)
            {
                Personaje.PuedeMoverse = true;
            }

            corrutinaActual.corrutina = null;
            yield return new WaitForFixedUpdate();
        }
    }
    

    //**********************************
    // FUNCIONES PARA CARGAR RESOURCES
    //**********************************
    private void CargarVinculadorFicherosResources()
    {
        vinculadorResources = (VinculadorResources)Resources.Load("VinculadorResources");
        if (vinculadorResources == null)
        {
            Debug.LogError("No se ha encontrado el fichero VinculadorResources en la carpeta de Resources");
        }
    }

    public Item ObtenerItem(ItemID itemID)
    {
        if (!contenedorItems.ContainsKey(itemID))
        {
            string nombreFichero = vinculadorResources.ObtenerNombreFicheroItem(itemID);
            if (!string.IsNullOrEmpty(nombreFichero))
            {
                Item item = (Item)Resources.Load(nombreFichero);
                contenedorItems.Add(item.ID, item);
            }
        }
        return contenedorItems[itemID];
    }

    public Pokemon ObtenerPokemon(PokemonID pokemonID)
    {
        if (!contenedorPokemons.ContainsKey(pokemonID))
        {
            string nombreFichero = vinculadorResources.ObtenerNombreFicheroPokemon(pokemonID);
            if (!string.IsNullOrEmpty(nombreFichero))
            {
                Pokemon pokemon = (Pokemon)Resources.Load(nombreFichero);
                contenedorPokemons.Add(pokemon.ID, pokemon);
            }
        }
        return contenedorPokemons[pokemonID];
    }

    public Ataque ObtenerAtaque(AtaqueID ataqueID)
    {
        if (!contenedorAtaques.ContainsKey(ataqueID))
        {
            string nombreFichero = vinculadorResources.ObtenerNombreFicheroAtaque(ataqueID);
            if (!string.IsNullOrEmpty(nombreFichero))
            {
                Ataque ataque = (Ataque)Resources.Load(nombreFichero);
                contenedorAtaques.Add(ataque.ID, ataque);
            }
        }
        return contenedorAtaques[ataqueID];
    }

    public void ReproducirSonido(SonidoID sonidoID)
    {
        if (!contenedorSonidos.ContainsKey(sonidoID))
        {
            string nombreFichero = vinculadorResources.ObtenerNombreFicheroSonido(sonidoID);
            if (!string.IsNullOrEmpty(nombreFichero))
            {
                Sonido sonido = (Sonido)Resources.Load(nombreFichero);
                contenedorSonidos.Add(sonido.ID, sonido);
            }
        }

        audioSonidos.clip = contenedorSonidos[sonidoID].audio;
        audioSonidos.volume = contenedorSonidos[sonidoID].volumen;
        audioSonidos.Play();
    }

    public void AsignarProximaMusicaDeZona(MusicaID musicaID)
    {
        proximaMusica = musicaID;
        if (musicaActual == MusicaID.NINGUNO)
        {
            musicaActual = musicaID;
            IniciarCorrutinaMusica(musicaActual);
        }
    }

    public void ReproducirMusicaZonaActual(MusicaID musicaDeLaZonaDeLaQueHaSalido)
    {
        if(musicaDeLaZonaDeLaQueHaSalido != proximaMusica)
        {
            musicaActual = proximaMusica;
            IniciarCorrutinaMusica(musicaActual);
        }
    }

    public void ReproducirMusicaCombate()
    {
        ObtenerFicheroMusica(MusicaID.MusicaCombate);
        audioMusica.clip = contenedorMusica[MusicaID.MusicaCombate].audio;
        audioMusica.volume = contenedorMusica[MusicaID.MusicaCombate].volumen;
        audioMusica.Play();
    }

    public void QuitarMusicaCombate()
    {
        IniciarCorrutinaMusica(proximaMusica);
    }

    public void IniciarCorrutinaMusica(MusicaID musicaID)
    {
        ObtenerFicheroMusica(musicaID);
        if(corrutinaMusica != null)
            StopCoroutine(corrutinaMusica);
        corrutinaMusica = CambiarMusica(musicaID);
        StartCoroutine(corrutinaMusica);
    }

    private void ObtenerFicheroMusica(MusicaID musicaID)
    {
        if (!contenedorMusica.ContainsKey(musicaID))
        {
            string nombreFichero = vinculadorResources.ObtenerNombreFicheroMusica(musicaID);
            Musica nuevaMusica = (Musica)Resources.Load(nombreFichero);
            contenedorMusica.Add(nuevaMusica.ID, nuevaMusica);
        }
    }

    private IEnumerator CambiarMusica(MusicaID nuevaMusica)
    {
        float velocidadTransicion = 0;
        float volumenActual = audioMusica.volume;
        if(audioMusica.clip != null)
        {
            while (audioMusica.volume > 0.1f)
            {
                velocidadTransicion += Time.deltaTime * Ajustes.Instancia.velocidadDecrementoVolumenAntiguaMusica;
                audioMusica.volume = Mathf.Lerp(volumenActual, 0, velocidadTransicion);
                yield return null;
            }
        }       

        audioMusica.volume = 0.1f;
        audioMusica.clip = contenedorMusica[nuevaMusica].audio;
        audioMusica.Play();

        velocidadTransicion = 0;
        while (audioMusica.volume < contenedorMusica[nuevaMusica].volumen)
        {
            velocidadTransicion += Time.deltaTime * Ajustes.Instancia.velocidadIncrementoVolumenNuevaMusica;
            audioMusica.volume = Mathf.Lerp(0.1f, contenedorMusica[nuevaMusica].volumen, velocidadTransicion);
            yield return null;
        }

        audioMusica.volume = contenedorMusica[nuevaMusica].volumen;
    }

    private struct ColaCorrutina
    {
        public IEnumerator corrutina;
        public bool detenerHastaQueFinalice;
        public string idParaDetener;
    }
}
