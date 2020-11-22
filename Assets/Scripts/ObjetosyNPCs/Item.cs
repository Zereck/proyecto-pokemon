using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public ItemID ID = ItemID.NINGUNO;
    public string nombre;
    public string descripcion;
    [Range(0, 9999)]
    public int precioEnTienda;
    public TipoDeItem tipoDeItem = TipoDeItem.Curacion;
    [HideInInspector]
    public AtaqueID enseñaAtaque = AtaqueID.NINGUNO;
    [HideInInspector]
    public int cantidadSanacion = 0;
    [HideInInspector]
    public EstadoAlterado restaurarEstadoAlterado;
    [HideInInspector]
    public int posibilidadCaptura = 50;
    
}