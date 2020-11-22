using System;
using UnityEngine;

public static class Herramientas {
        
    public static Vector2 ObtenerDireccion(Direccion direccion)
    {
        Vector2 nuevaDireccion = Vector2.zero;

        switch (direccion)
        {
            case Direccion.Arriba:
                nuevaDireccion = Vector2.up;
                break;
            case Direccion.Abajo:
                nuevaDireccion = Vector2.down;
                break;
            case Direccion.Izquierda:
                nuevaDireccion = Vector2.left;
                break;
            case Direccion.Derecha:
                nuevaDireccion = Vector2.right;
                break;
        }

        return nuevaDireccion;

    }

    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, ignoreCase: true);
    }
    
    public static string TextoAtaqueElemento(Elemento elemento)
    {
        switch (elemento)
        {
            case Elemento.Psiquico:
                return "Psíquico";
            case Elemento.Electrico:
                return "Eléctrico";
            case Elemento.Dragon:
                return "Dragón";
        }
        return elemento.ToString();
    }

    public static Vector2 ObtenerDireccion(Vector2 origen, Vector2 destino)
    {
        Vector2 direccion = destino - origen;
        if (direccion.x < -Ajustes.Instancia.QuintaParteTamanioCasilla)
            direccion.x = -1;
        else if (direccion.x > Ajustes.Instancia.QuintaParteTamanioCasilla)
            direccion.x = 1;
        else
            direccion.x = 0;

        if (direccion.y < -Ajustes.Instancia.QuintaParteTamanioCasilla)
            direccion.y = -1;
        else if (direccion.y > Ajustes.Instancia.QuintaParteTamanioCasilla)
            direccion.y = 1;
        else
            direccion.y = 0;
        return direccion;
    }

    public static Vector2 ObtenerDireccionInversa(Vector2 origen, Vector2 destino)
    {
        return ObtenerDireccion(destino, origen);
    }

    public static bool LayerSonIguales(int layerObjecto, LayerMask layerCampo)
    {
        if (((1 << layerObjecto) & layerCampo) != 0)
            return true;
        return false;
    }


    public static void ResizeSpriteToScreen(GameObject theSprite, Camera theCamera, int fitToScreenWidth, int fitToScreenHeight)
    {
        SpriteRenderer sr = theSprite.GetComponent<SpriteRenderer>();

        theSprite.transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = (float)(theCamera.orthographicSize * 2.0);
        float worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width);

        if (fitToScreenWidth != 0)
        {
            Vector2 sizeX = new Vector2(worldScreenWidth / width / fitToScreenWidth, theSprite.transform.localScale.y);
            theSprite.transform.localScale = sizeX;
        }

        if (fitToScreenHeight != 0)
        {
            Vector2 sizeY = new Vector2(theSprite.transform.localScale.x, worldScreenHeight / height / fitToScreenHeight);
            theSprite.transform.localScale = sizeY;
        }
    }
}