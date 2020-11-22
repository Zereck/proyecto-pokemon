using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPCControlador : MonoBehaviour {

	public Button botonMoverAPC;
	public Button botonMoverAEquipo;

	public UIPCPokemonPC plantillaPokemonPC;

	public List<UIPCPokemonEquipo> pokemonsEnEquipo;
	List<UIPCPokemonPC> pokemonsEnPC = new List<UIPCPokemonPC>();

	PokemonModelo ultimoPokemonPulsado;
	bool ultimoPokemonPulsadoEsDelPC;

	public void Mostrar()
	{
		Personaje.UIAbierta = true;
		Personaje.PuedeMoverse = false;
		Rebuild();
		gameObject.SetActive(true);
	}

	private void Rebuild()
	{
		print("Reconstruyendo pokemons. En PC: " + ControladorDatos.Instancia.Datos.ObtenerPokemonEnPC().Length.ToString());
		pokemonsEnEquipo.ForEach((p) => p.gameObject.SetActive(false));

		if(pokemonsEnPC != null & pokemonsEnPC.Count > 0)
		{
			pokemonsEnPC.ForEach((p) => Destroy(p.gameObject));
			pokemonsEnPC.Clear();
		}

		var equipoPokemon = ControladorDatos.Instancia.Datos.ObtenerEquipoPokemon();
		for (int i = 0; i < equipoPokemon.Length; i++)
		{
			pokemonsEnEquipo[i].MostrarPokemon(equipoPokemon[i], this);
		}

		var pcPokemon = ControladorDatos.Instancia.Datos.ObtenerPokemonEnPC();

		for (int i = 0; i < pcPokemon.Length; i++)
		{
			var go = Instantiate(plantillaPokemonPC.gameObject, plantillaPokemonPC.gameObject.transform.parent);
			UIPCPokemonPC goComponente = go.GetComponent<UIPCPokemonPC>();
			goComponente.MostrarPokemon(pcPokemon[i], this);
			pokemonsEnPC.Add(goComponente);
		}
	}


	private void OnDisable()
	{
		Personaje.UIAbierta = false;
		Personaje.PuedeMoverse = true;
		if (pokemonsEnPC != null & pokemonsEnPC.Count > 0)
		{
			pokemonsEnPC.ForEach((p) => Destroy(p.gameObject));
			pokemonsEnPC.Clear();
		}
	}

	public void PokemonPCPulsado(PokemonModelo pokemon)
	{
		ultimoPokemonPulsado = pokemon;
		ultimoPokemonPulsadoEsDelPC = true;
		PokemonPulsado(pokemon);
		if (ControladorDatos.Instancia.Datos.NumeroDePokemonEnElEquipo() == 6) return;
		botonMoverAEquipo.enabled = true;
	}

	public void PokemonEquipoPulsado(PokemonModelo pokemon)
	{
		ultimoPokemonPulsado = pokemon;
		ultimoPokemonPulsadoEsDelPC = false;
		PokemonPulsado(pokemon);
		if (ControladorDatos.Instancia.Datos.NumeroDePokemonEnElEquipo() == 6) return;
		botonMoverAPC.enabled = true;

	}

	private void PokemonPulsado(PokemonModelo pokemon)
	{
		if (pokemonsEnPC != null && pokemonsEnPC.Count != 0)
			pokemonsEnPC.ForEach((p) => p.PokemonPulsado(pokemon));
		if (pokemonsEnEquipo != null && pokemonsEnEquipo.Count != 0)
			pokemonsEnEquipo.ForEach((p) => p.PokemonPulsado(pokemon));
	}

	public void BotonMoverAPcPulsado()
	{
		if (ultimoPokemonPulsado == null) return;
		print("Equipo " + ControladorDatos.Instancia.Datos.NumeroDePokemonEnElEquipo().ToString());
		if (ControladorDatos.Instancia.Datos.NumeroDePokemonEnElEquipo() == 1) return;

		bool resultado = ControladorDatos.Instancia.Datos.DejarPokemonDelEquipoEnPC(ultimoPokemonPulsado);

		if (resultado)
		{
			ultimoPokemonPulsado = null;
			PokemonPulsado(null);


			Rebuild();
		}

	}

	public void BotonMoverAEquipoPulsado()
	{
		if (ultimoPokemonPulsado == null) return;

		bool resultado = ControladorDatos.Instancia.Datos.SacarPokemonDelPC(ultimoPokemonPulsado);

		if (resultado)
		{
			ultimoPokemonPulsado = null;
            PokemonPulsado(null);
			Rebuild();
		}

	}
}
