using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMovible {

    Transform Transform();
    bool MoverExternamente(Vector2 nuevaCasilla, Vector2 nuevaDireccion, bool estaSiguiendoAOtro = false);
    TipoColision DetectarColisionesEnfrente(Vector2 direccion);
    void DetenerAnimacion();
    Vector2 SiguienteDireccion();
    Vector2 SiguientePosicion();
    bool EstaCercaDeLaSiguienteCasilla();

}
