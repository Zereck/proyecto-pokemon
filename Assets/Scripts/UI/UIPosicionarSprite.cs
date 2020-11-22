using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPosicionarSprite : MonoBehaviour {
        
    private void Start()
    {
        transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
    }
}
