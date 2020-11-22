using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zona : MonoBehaviour
{
    public MusicaID musica;
    public CentroPokemon centroPokemon;
    private bool haEntrado;

    private static int ultimaZonaVisitada;

    private void OnEnable()
    {
        ControladorEventos.Instancia.SubscribirseEvento(typeof(EventoTeletransportarseCentroPokemon), TeletranportarCentroPokemon);
    }

    private void OnDisable()
    {
        ControladorEventos.Instancia.DesubscribirseEvento(typeof(EventoTeletransportarseCentroPokemon), TeletranportarCentroPokemon);
    }

    private void Start()
    {
        if (!haEntrado)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Ajustes.Instancia.tagPersonaje))
        {
            ControladorDatos.Instancia.AsignarProximaMusicaDeZona(musica);
            ultimaZonaVisitada = gameObject.GetInstanceID();
            haEntrado = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Ajustes.Instancia.tagPersonaje))
        {
            ControladorDatos.Instancia.ReproducirMusicaZonaActual(musica);
            haEntrado = false;
            StartCoroutine(DesactivarObjetosAlSalir());
        }
    }

    private IEnumerator DesactivarObjetosAlSalir()
    {
        yield return new WaitForSeconds(4);
        if (!haEntrado)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                yield return null;
            }
        }
    }   

    private void TeletranportarCentroPokemon(EventoBase mensaje)
    {
        if (ultimaZonaVisitada == 0)
            ultimaZonaVisitada = gameObject.GetInstanceID();

        if (ultimaZonaVisitada == gameObject.GetInstanceID())
        {
            Vector2 direccion = Herramientas.ObtenerDireccion(centroPokemon.transform.position, centroPokemon.npc.transform.position);
            Personaje.TeletransportarPersonaje.CambiarPosicion(centroPokemon.transform.position, direccion);
            centroPokemon.npc.MostrarDialogo();
            ControladorDatos.Instancia.Datos.CentroPokemon();
        }
        
    }

}
