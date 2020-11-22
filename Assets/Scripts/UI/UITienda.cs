using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITienda : MonoBehaviour {
    
    public RectTransform contenedorItemTienda;
    public Text dineroActual;
    public UITiendaVentanaConfirmacion ventanaConfirmacion;
    public List<UITiendaElementoComprar> itemTiendaLista;

    public void MostrarTienda(ItemID[] items)
    {
        ControladorDatos.Instancia.Datos.AniadirMonedas(5000);
        Personaje.UIAbierta = true;
        Personaje.PuedeMoverse = false;
        for (int i = 0; i < itemTiendaLista.Count; i++)
        {
            itemTiendaLista[i].gameObject.SetActive(false);
        }

        ventanaConfirmacion.gameObject.SetActive(false);
        dineroActual.text = ControladorDatos.Instancia.Datos.Monedas.ToString();
        bool elementoSinAsignar = false;
        foreach (ItemID item in items)
        {
            for (int i = 0; i < itemTiendaLista.Count; i++)
            {
                if (!itemTiendaLista[i].gameObject.activeSelf)
                {
                    itemTiendaLista[i].transform.SetParent(contenedorItemTienda.transform);
                    itemTiendaLista[i].AsignarValores(item);
                    elementoSinAsignar = true;
                    break;
                }
            }

            if (!elementoSinAsignar)
            {
                UITiendaElementoComprar go = Instantiate(itemTiendaLista[0].gameObject).GetComponent<UITiendaElementoComprar>();
                go.transform.SetParent(contenedorItemTienda.transform);
                go.AsignarValores(item);
                go.GetComponent<RectTransform>().localScale = Vector3.one;
                itemTiendaLista.Add(go);
            }            

            elementoSinAsignar = false;
        }

        //Calculamos la altura actual eliminando el elemento plantilla de la lista de pokémons
        float alturaPanel = itemTiendaLista.Count * itemTiendaLista[0].GetComponent<RectTransform>().rect.height;
        VerticalLayoutGroup vl = contenedorItemTienda.GetComponent<VerticalLayoutGroup>();
        //Calculamos el margin y el padding eliminando 2 elementos para el spacing (la plantilla y uno extra)
        alturaPanel += vl.padding.top + vl.padding.bottom + ((itemTiendaLista.Count - 2) * vl.spacing);
        contenedorItemTienda.sizeDelta = new Vector2(contenedorItemTienda.sizeDelta.x, alturaPanel);

        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        Personaje.UIAbierta = false;
        Personaje.PuedeMoverse = true;
    }

    public void ItemPulsado(Item item)
    {
        ventanaConfirmacion.MostrarVentanaConfirmacion(item);
    }
}
