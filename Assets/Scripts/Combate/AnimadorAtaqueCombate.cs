using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimadorAtaqueCombate : MonoBehaviour {

    [SerializeField]
    private AnimatorOverrideController animadorSobreescrito;
    public SpriteRenderer spriteRenderer;

    private Animator animaciones;
    private bool haIniciado;

    public bool ReproducirClipAnimacionAtaque(AnimationClip clip)
    {
        if(animaciones == null)
        {
            animaciones = GetComponent<Animator>();
            animaciones.runtimeAnimatorController = animadorSobreescrito;
        }

        if (!haIniciado)
        {
            haIniciado = true;
            animadorSobreescrito["AtacarBase"] = clip;

            animaciones.Play("AtacarBase");
        }
        else if (animaciones.GetCurrentAnimatorStateInfo(0).IsName("AtacarBase") && animaciones.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            haIniciado = false;
            animadorSobreescrito["AtacarBase"] = null;
            animaciones.Play("New State");
            return false;
        }
        return true;
    }

    public void ReproducirSonido(SonidoID sonido)
    {
        ControladorDatos.Instancia.ReproducirSonido(sonido);
    }
}
