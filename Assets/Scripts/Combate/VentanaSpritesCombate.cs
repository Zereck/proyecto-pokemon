using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentanaSpritesCombate : MonoBehaviour {

    public SpriteRenderer spritePokemonJugador;
    public SpriteRenderer spritePokemonEnemigo;
    public SpriteRenderer spriteAtaque;
    public GameObject contenedorSpritePokemonJugador;
    public GameObject contenedorSpritePokemonEnemigo;
    public GameObject contenedorAnimadorAtaque;
    public AnimadorAtaqueCombate animadorAtaque;

    private Vector3 tamanioContenedorPorDefecto = Vector3.zero;
    private bool animacionAsignada;
    private Animator animaciones;
    private string ultimaAnimacionCombate = string.Empty;

    private void OnEnable()
    {
        spritePokemonJugador.gameObject.SetActive(false); //Se activará desde el clip de animación
        spritePokemonEnemigo.gameObject.SetActive(false); //Se activará desde el clip de animación

        if (tamanioContenedorPorDefecto == Vector3.zero)
            tamanioContenedorPorDefecto = contenedorSpritePokemonEnemigo.transform.localScale;

        if (animaciones == null)
            animaciones = GetComponent<Animator>();        
    }

    private void OnDisable()
    {
        spritePokemonJugador.gameObject.SetActive(false);
        spritePokemonEnemigo.gameObject.SetActive(false);
        ultimaAnimacionCombate = string.Empty;
    }

    public bool MostrarAnimacionAtaque(AtaqueID id, Atacante atacante)
    {
        Ataque ataque = ControladorDatos.Instancia.ObtenerAtaque(id);

        if (ataque.animacionAtaque == null)
            return false;

        if (!animacionAsignada)
        {
            animacionAsignada = true;

            if (atacante == Atacante.Enemigo)
                contenedorAnimadorAtaque.transform.localScale = new Vector3(-1, 1, 1);
            else
                contenedorAnimadorAtaque.transform.localScale = new Vector3(1, 1, 1);
        }

        if (!animadorAtaque.ReproducirClipAnimacionAtaque(ataque.animacionAtaque))
        {
            spriteAtaque.sprite = null;
            animacionAsignada = false;
            return false;
        }
        return true;

    }

    public void AsignarSpritePokemonJugador(Pokemon pokemon)
    {
        spritePokemonJugador.gameObject.SetActive(false); //Se activará desde el clip de animación
        spritePokemonJugador.sprite = pokemon.sprite;
        contenedorSpritePokemonJugador.transform.localScale = tamanioContenedorPorDefecto * pokemon.tamanioSpriteEnCombate;

        if (pokemon.elSpriteEstaMirandoALaDerecha)
            spritePokemonJugador.flipX = false;
        else
            spritePokemonJugador.flipX = true;
    }

    public void AsignarSpritePokemonEnemigo(Pokemon pokemon)
    {
        spritePokemonEnemigo.gameObject.SetActive(false); //Se activará desde el clip de animación
        spritePokemonEnemigo.sprite = pokemon.sprite;
        contenedorSpritePokemonEnemigo.transform.localScale = tamanioContenedorPorDefecto * pokemon.tamanioSpriteEnCombate;

        if (pokemon.elSpriteEstaMirandoALaDerecha)
            spritePokemonEnemigo.flipX = true;
        else
            spritePokemonEnemigo.flipX = false;
    }

    public bool MostrarAnimacionCombate(AnimacionCombate animacion)
    {
        if (string.IsNullOrEmpty(ultimaAnimacionCombate))
        {
            switch (animacion)
            {
                case AnimacionCombate.PokemonSalvajeAparece:
                    ultimaAnimacionCombate = "CombatePokemonSalvaje";
                    break;
                case AnimacionCombate.EntrenadorEnviaPokemon:
                    ultimaAnimacionCombate = "CombateEntrenadorEnviaPokemon";
                    break;
                case AnimacionCombate.JugadorEnviaPokemon:
                    ultimaAnimacionCombate = "CombateJugadorEnviaPokemon";
                    break;
                case AnimacionCombate.LanzarPokeball:
                    ultimaAnimacionCombate = "CombateLanzarPokeball";
                    break;
                case AnimacionCombate.PokeballSeAgita:
                    ultimaAnimacionCombate = "CombatePokeballAgitarse";
                    break;
                case AnimacionCombate.PokeballSeRompe:
                    ultimaAnimacionCombate = "CombatePokeballRomperse";
                    break;
                case AnimacionCombate.PokemonJugadorRecibeAtaque:
                    ultimaAnimacionCombate = "CombatePokemonJugadorRecibeAtaque";
                    break;
                case AnimacionCombate.PokemonEnemigoRecibeAtaque:
                    ultimaAnimacionCombate = "CombatePokemonEnemigoRecibeAtaque";
                    break;
                case AnimacionCombate.PokemonJugadorRealizaAtaqueFisico:
                    ultimaAnimacionCombate = "CombatePokemonJugadorAtaqueFisico";
                    break;
                case AnimacionCombate.PokemonEnemigoRealizaAtaqueFisico:
                    ultimaAnimacionCombate = "CombatePokemonEnemigoAtaqueFisico";
                    break;
                case AnimacionCombate.PokemonJugadorDerrotado:
                    ultimaAnimacionCombate = "CombatePokemonJugadorDerrotado";
                    break;
                case AnimacionCombate.PokemonEnemigoDerrotado:
                    ultimaAnimacionCombate = "CombatePokemonEnemigoDerrotado";
                    break;
                case AnimacionCombate.JugadorRetirarPokemon:
                    ultimaAnimacionCombate = "CombateJugadorRetirarPokemon";
                    break;
            }

            animaciones.Play(ultimaAnimacionCombate);
        }

        if (animaciones.GetCurrentAnimatorStateInfo(0).IsName(ultimaAnimacionCombate) && animaciones.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            ultimaAnimacionCombate = string.Empty;
            animaciones.Play("PorDefecto");
            return false;
        }
        else
        {
            return true;
        }

    }

    public void ReproducirSonido(SonidoID sonido)
    {
        ControladorDatos.Instancia.ReproducirSonido(sonido);
    }

}
