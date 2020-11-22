using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMochilaSeccionPanel : MonoBehaviour {

    public TipoDeItem listarTipoDeItem;
    public UIMochilaItem itemPlantilla;

    private Dictionary<ItemID, UIMochilaItem> elementosListados;

    private void OnEnable()
    {
        if (elementosListados == null)
            elementosListados = new Dictionary<ItemID, UIMochilaItem>();

        Dictionary<ItemID, int> inventario = ControladorDatos.Instancia.Datos.ObtenerInventario();

        foreach (KeyValuePair<ItemID, int> itemActual in inventario)
        {
            Item item = ControladorDatos.Instancia.ObtenerItem(itemActual.Key);
            if(item.tipoDeItem == listarTipoDeItem)
            {
                if (itemActual.Value > 0)
                {
                    if (!elementosListados.ContainsKey(item.ID))
                    {
                        UIMochilaItem nuevoItem = Instantiate(itemPlantilla).GetComponent<UIMochilaItem>();
                        nuevoItem.transform.SetParent(gameObject.transform);
                        nuevoItem.GetComponent<RectTransform>().localScale = Vector3.one;
                        elementosListados.Add(item.ID, nuevoItem);
                    }
                    elementosListados[item.ID].MostrarItem(item, itemActual.Value);
                }
                else
                {
                    if (elementosListados.ContainsKey(item.ID))
                    {
                        elementosListados[item.ID].gameObject.SetActive(false);
                    }
                }                    
            }
        }
    }
}
