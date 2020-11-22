using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAprenderNuevoAtaqueVentana : MonoBehaviour {

    public UIAprenderNuevoAtaqueDetalles[] detallesAtaques;
    public Text nuevoAtaqueNombre;
    public Text nuevoAtaqueElemento;
    public Text nuevoAtaquePoderYTipo;

    public Ataque AtaqueSeleccionado { get; private set; }

    private Ataque nuevoAtaqueAprender;
    private PokemonModelo pokemon;

    public void MostrarVentana(PokemonModelo pokemon, AtaqueID nuevoAtaque)
    {
        this.pokemon = pokemon;
        nuevoAtaqueAprender = ControladorDatos.Instancia.ObtenerAtaque(nuevoAtaque);
        AtaqueSeleccionado = null;

        nuevoAtaqueNombre.text = nuevoAtaqueAprender.nombre;
        nuevoAtaqueElemento.text = Herramientas.TextoAtaqueElemento(nuevoAtaqueAprender.ataqueElemento);
        nuevoAtaquePoderYTipo.text = string.Concat(nuevoAtaqueAprender.poder, " ", nuevoAtaqueAprender.TextoTipoAtaque());

        for (int i = 0; i < detallesAtaques.Length; i++)
        {
            detallesAtaques[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < pokemon.Ataques().Length; i++)
        {
            detallesAtaques[i].MostrarAtaque(pokemon.Ataques()[i].DatosFijos, this);
        }

        gameObject.SetActive(true);
    }

    public void AtaquePulsado(Ataque ataque)
    {
        if (UIControlador.Instancia.Dialogo.ventanConfirmacion.gameObject.activeSelf)
            return;

        AtaqueSeleccionado = ataque;

        string textoSolicitarConfirmacion = Ajustes.Instancia.textoConfirmarAprenderNuevoAtaque
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, AtaqueSeleccionado.nombre)
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, nuevoAtaqueAprender.nombre);

        string textoNuevoAtaqueAprendido = Ajustes.Instancia.textoNuevoAtaqueAprendido
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, pokemon.DatosFijos.nombre)
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, nuevoAtaqueAprender.nombre);

        string textoSolicitarSeleccionarAtaque = Ajustes.Instancia.textoPreguntarAprenderNuevoAtaque
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, pokemon.DatosFijos.nombre)
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, AtaqueSeleccionado.nombre);

        ControladorEventos.Instancia.LanzarEvento(new EventoMostrarVentanaConfirmacion(AccionConfirmarAprenderNuevoAtaque, AccionCancelarAprenderNuevoAtaque, textoSolicitarConfirmacion, textoNuevoAtaqueAprendido, textoSolicitarSeleccionarAtaque, PokemonID.NINGUNO, false));
    }

    private void AccionConfirmarAprenderNuevoAtaque()
    {
        pokemon.AprenderNuevoAtaque(AtaqueSeleccionado.ID, nuevoAtaqueAprender.ID);
        gameObject.SetActive(false);
    }

    private void AccionCancelarAprenderNuevoAtaque()
    {
        AtaqueSeleccionado = null;
    }

    public void NoAprenderNuevoAtaque()
    {
        if (UIControlador.Instancia.Dialogo.ventanConfirmacion.gameObject.activeSelf)
            return;

        string textoSolicitarConfirmacion = Ajustes.Instancia.textoConfirmarNoAprenderNuevoAtaque
                                    .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, nuevoAtaqueAprender.nombre);

        string textoSolicitarSeleccionarAtaque = Ajustes.Instancia.textoPreguntarAprenderNuevoAtaque
                            .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto1, pokemon.DatosFijos.nombre)
                            .Replace(Ajustes.Instancia.palabraParaReemplazarEnLosTexto2, nuevoAtaqueAprender.nombre);

        ControladorEventos.Instancia.LanzarEvento(new EventoMostrarVentanaConfirmacion(AccionConfirmarCancelarAprenderAtaque, null, textoSolicitarConfirmacion, string.Empty, textoSolicitarSeleccionarAtaque, PokemonID.NINGUNO, false));
    }

    private void AccionConfirmarCancelarAprenderAtaque()
    {
        gameObject.SetActive(false);
    }
   
}
