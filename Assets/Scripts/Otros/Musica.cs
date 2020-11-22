using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Musica : ScriptableObject {

    public MusicaID ID;
    [Range(0.05f, 1f)]
    public float volumen = 1f;
    public AudioClip audio;

}
