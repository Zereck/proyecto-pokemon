using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITeletransportable {

    void CambiarPosicion(Vector2 nuevaPosicion, Vector2 direccionMirar);
}
