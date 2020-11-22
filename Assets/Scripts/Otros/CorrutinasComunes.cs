using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CorrutinasComunes
{

    public static IEnumerator AlfaDeCeroAUno(Image imgen)
    {
        imgen.gameObject.SetActive(true);
        Color c = imgen.color;
        c.a = 0.0f;

        //Mientras no esté totalmente visible va aumentando su visibilidad
        while (c.a < 1)
        {
            imgen.color = c;
            c.a += Ajustes.Instancia.velocidadOscurecerPantallaTeletransporte;
            yield return null;
        }
    }

    public static IEnumerator AlfaDeUnoACero(Image imgen)
    {
        Color c = imgen.color;

        //Mientras la imagen de carga siga visible va desvaneciéndola
        while (c.a > 0)
        {
            imgen.color = c;
            c.a -= Ajustes.Instancia.velocidadAclararPantallaTeletransporte;
            yield return null;
        }
        imgen.gameObject.SetActive(false);
    }

    public static IEnumerator MoverBarraDeslizadora(Image barra, float porcentajeValorFinal, int saludMaximaPokemon, Text barraTexto, Action callback)
    {
        float valorActual = barra.fillAmount;
        float velocidadTransicion = 0;

        while (Mathf.Abs(barra.fillAmount - porcentajeValorFinal) > float.Epsilon)
        {
            velocidadTransicion += Time.deltaTime * Ajustes.Instancia.velocidadDeMovimientoDeBarrasDeSaludEnCombate;
            barra.fillAmount = Mathf.Lerp(valorActual, porcentajeValorFinal, velocidadTransicion);
            if (barraTexto != null && barraTexto.gameObject.activeSelf)
            {
                barraTexto.text = string.Concat((saludMaximaPokemon * barra.fillAmount).ToString("0"), "/", saludMaximaPokemon);
            }

            yield return null;
        }

        barra.fillAmount = porcentajeValorFinal;
        if (barraTexto != null && barraTexto.gameObject.activeSelf)
            barraTexto.text = string.Concat((saludMaximaPokemon * barra.fillAmount).ToString("0"), "/", saludMaximaPokemon);

        if (callback != null)
            callback();
    }

    public static IEnumerator SeguirObjeto(IMovible perseguidor, IMovible objetivo)
    {
        Vector2 siguienteDireccion = objetivo.SiguienteDireccion();
        Vector2 siguientePosicion = perseguidor.Transform().position;
        bool posicionObjetivoRecuperada = false;
        perseguidor.Transform().gameObject.GetComponent<Collider2D>().enabled = false;

        while (perseguidor.Transform().gameObject.activeSelf && objetivo.Transform().gameObject.activeSelf)
        {
            if (!objetivo.EstaCercaDeLaSiguienteCasilla())
            {
                if (!posicionObjetivoRecuperada)
                {
                    posicionObjetivoRecuperada = true;
                    //... Invertimos la dirección para calcular la casilla que hay detrás del objetivo
                    siguienteDireccion = objetivo.SiguienteDireccion() * -1;
                    siguientePosicion = objetivo.SiguientePosicion() + (Ajustes.Instancia.tamanioCasilla * siguienteDireccion);

                    if (Vector2.Distance(siguientePosicion, perseguidor.Transform().position) > Ajustes.Instancia.tamanioCasilla * 3)
                        perseguidor.Transform().position = siguientePosicion;
                }
            }
            else
            {
                posicionObjetivoRecuperada = false;
            }

            //... Como invertimos la dirección para calcular la casilla detrás del objetivo, la volvemos a invertir para asignar la dirección en la que el perseguidor tendrá que mirar
            perseguidor.MoverExternamente(siguientePosicion, siguienteDireccion * -1, true);
            yield return null;
        }
    }

    public static IEnumerator MoverHastaPosicion(IMovible objeto, Vector2 destino, Action ejecutarAlFinalizar = null)
    {
        //Esperamos a que el personaje quede quieto para saber su dirección
        while (!Personaje.BloquearMovimiento && !Personaje.PuedeMoverse)
            yield return null;

        //destino = AjustarPosicionAlTilemap(destino);
        Vector2 direccion = Herramientas.ObtenerDireccion(objeto.Transform().position, destino);
        bool destinoAlcanzado = false;
        bool finTramo = false;
        Vector2 direccionRayo;
        Vector2 tramoActual;
        Vector2 siguienteCasilla;
        Vector2 distancia;
        int tramosTotales = 0;


        //Mientras no alcance el destino...
        while (!destinoAlcanzado && tramosTotales < 10)
        {
            tramosTotales++;
            //... Calculamos la distancia entre el destino y la posición actual
            distancia = destino - (Vector2)objeto.Transform().position;
            //... Si la distancia horizontal es mayor que la vertical o hay un obstáculo en vertical, se moverá en horizontal hacia el destino...
            if (Mathf.Abs(distancia.x) > Mathf.Abs(distancia.y) || objeto.DetectarColisionesEnfrente(new Vector2(0, direccion.y)) == TipoColision.ObjetoColision)
            {
                //... La dirección del rayo será horizontal para comprobar colisiones en la dirección que se mueve
                direccionRayo = new Vector2(direccion.x, 0);
                //... El nuevo tramo será la posición horizontal del destino y la posición vertical actual del objeto
                tramoActual = new Vector2(destino.x, objeto.Transform().position.y);
            }
            //... Si la posición vertical es mayor o no hay obstáculos verticales...
            else
            {
                //... La dirección del rayo será vertical para comprobar colisiones en la dirección que se mueve
                direccionRayo = new Vector2(0, direccion.y);
                //... El nuevo tramo será la posición vertical del destino y la posición horizontal actual del objeto
                tramoActual = new Vector2(objeto.Transform().position.x, destino.y);
            }

            //... Si el jugador está justo enfrente del objeto ha llegado a su destino
            if (objeto.DetectarColisionesEnfrente(direccionRayo) == TipoColision.Personaje)
            {
                objeto.MoverExternamente(objeto.Transform().position, direccionRayo);
                finTramo = true;
                destinoAlcanzado = true;
            }

            //... Mientras no haya chocado con algún obstáculo en el tramo y no haya llegado al final de este...
            while (!finTramo && Vector2.Distance(objeto.Transform().position, tramoActual) > Ajustes.Instancia.QuintaParteTamanioCasilla)
            {
                //... Calculamos la siguiente casilla a la que se tiene que mover en el tilemap
                siguienteCasilla = new Vector2(objeto.Transform().position.x + (direccionRayo.x * Ajustes.Instancia.tamanioCasilla), objeto.Transform().position.y + (direccionRayo.y * Ajustes.Instancia.tamanioCasilla));
                //... Comprobamos si hay algún colisionador antes de moverlo y de qué tipo es
                TipoColision colisionRayo = objeto.DetectarColisionesEnfrente(direccionRayo);
                //... Si delante no hay ningún colisionador...
                if (colisionRayo == TipoColision.NINGUNO)
                {
                    //... Mientras el objeto no haya llegado a la siguiente casilla lo mueve
                    while (!objeto.MoverExternamente(siguienteCasilla, direccionRayo))
                    {
                        yield return null;
                    }
                }
                //... Si ha colisionado con un objeto normal, finalizamos el tramo
                else if (colisionRayo == TipoColision.ObjetoColision)
                {
                    finTramo = true;
                }
                //... Si ha colisionado con el personaje, ha llegado a su destino antes de tiempo
                else if (colisionRayo == TipoColision.Personaje)
                {
                    finTramo = true;
                    destinoAlcanzado = true;
                }

                yield return null;
            }
            finTramo = false;
            yield return null;
        }

        //Paramos la animación actual
        objeto.DetenerAnimacion();

        if (ejecutarAlFinalizar != null)
            ejecutarAlFinalizar();
    }

    private static Vector2 AjustarPosicionAlTilemap(Vector2 posicion)
    {
        float centroCasilla = Ajustes.Instancia.tamanioCasilla / 2;
        float restoX = posicion.x % Ajustes.Instancia.tamanioCasilla;
        float restoY = posicion.y % Ajustes.Instancia.tamanioCasilla;


        if (restoX != centroCasilla)
        {
            posicion.x = (posicion.x - restoX) - centroCasilla;
        }

        if (restoY != centroCasilla)
        {
            posicion.y = (posicion.y - restoY) - centroCasilla;
        }

        return posicion;

    }


    public static IEnumerator CurarEquipoPokemon()
    {
        ControladorDatos.Instancia.Datos.CentroPokemon();
        ControladorDatos.Instancia.ReproducirSonido(SonidoID.SonidoCurarPokemon);
        while (ControladorDatos.Instancia.audioSonidos.isPlaying)
            yield return null;
    }
    
    
}