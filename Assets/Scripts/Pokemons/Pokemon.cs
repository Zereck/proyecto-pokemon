using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class Pokemon : ScriptableObject {

    public PokemonID ID;
    public string nombre;
    public string descripcion;
    public int numeroEnPokedex;
    public Elemento tipoElemento1;
    public Elemento tipoElemento2;
    public bool elSpriteEstaMirandoALaDerecha = true;
    [Range(0.3f, 1f)]
    public float tamanioSpriteEnCombate = 0.5f;
    public Sprite sprite;
    [Range(1, Ajustes.estadisticaSaludBaseMaxima)]
    public int saludMaxima = Ajustes.estadisticaSaludBaseMaxima / 2;
    [Range(1, Ajustes.estadisticasBaseMaximas)]
    public int ataqueFisicoBase = Ajustes.estadisticasBaseMaximas / 2;
    [Range(1, Ajustes.estadisticasBaseMaximas)]
    public int defensaFisicaBase = Ajustes.estadisticasBaseMaximas / 2;
    [Range(1, Ajustes.estadisticasBaseMaximas)]
    public int ataqueMagicoBase = Ajustes.estadisticasBaseMaximas / 2;
    [Range(1, Ajustes.estadisticasBaseMaximas)]
    public int defensaMagicaBase = Ajustes.estadisticasBaseMaximas / 2;
    [Range(1, Ajustes.estadisticasBaseMaximas)]
    public int velocidadBase = Ajustes.estadisticasBaseMaximas / 2;
    public PokemonID evolucion;
    [Range(5, Ajustes.nivelMaximo)]
    public int nivelEvolucion;
    [HideInInspector]
    public List<AtaqueReferencia> listaDeAtaques;
    
    
}