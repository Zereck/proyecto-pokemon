using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentanaSpritesEvolucion : MonoBehaviour {

    public SpriteRenderer pokemonOriginal;
    public SpriteRenderer pokemonEvolucion;
    public GameObject contenedorPokemonOriginal;
    public GameObject contenedorPokemonEvolucion;
    public Animator animacionEvolucion;

    private Vector3 tamanioOriginalContenedor = Vector3.zero;
    
    public void AsignarSprites(Pokemon pokemon)
    {

        if (tamanioOriginalContenedor == Vector3.zero)
            tamanioOriginalContenedor = contenedorPokemonEvolucion.transform.localScale;

        pokemonEvolucion.gameObject.SetActive(false);
        pokemonOriginal.gameObject.SetActive(true);

        //Reiniciamos los valores del sprite para que en futuras reproduccciones no se inicie la animación con los valores del final de la anterior animación
        pokemonOriginal.transform.localScale = Vector3.one;
        pokemonOriginal.color = Color.white;

        pokemonOriginal.sprite = pokemon.sprite;
        Pokemon evolucion = ControladorDatos.Instancia.ObtenerPokemon(pokemon.evolucion);
        pokemonEvolucion.sprite = evolucion.sprite;

        contenedorPokemonOriginal.transform.localScale = tamanioOriginalContenedor * pokemon.tamanioSpriteEnCombate;
        contenedorPokemonEvolucion.transform.localScale = tamanioOriginalContenedor * evolucion.tamanioSpriteEnCombate;

        gameObject.SetActive(true);
    }

    public void ReproducirAnimacion()
    {
        animacionEvolucion.Play("Evolucionar");
    }


    public bool EvolucionFinalizada()
    {
        if (animacionEvolucion.GetCurrentAnimatorStateInfo(0).IsName("Evolucionar") && animacionEvolucion.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            return true;
        }
        return false;
    }
}
