using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CameraController;

public class PlaceCamera : SwapCamera
{
    
    // orientation
    public float duration = 1f;
    private int orientation, current;
    private float init;
    private PlacesUI swipe;

    public override void Start()
    {
        base.Start();
        swipe = PlacesUI.Instance;
        orientation = current = 0;
        init = transform.rotation.eulerAngles.y;
    }


    public void TurnLeft() {
           swipe.Reset();
            orientation--;
            StartCoroutine(Iorient(orientation));
    }

    public void TurnRight() {
           swipe.Reset();
            orientation++;
            StartCoroutine(Iorient(orientation));
    }

    private IEnumerator Iorient(int orientation) {
        Quaternion q = Quaternion.Euler(0, 90*orientation, 0);
        float t = 0;
        float tStep = 1 / duration;                                                                 // incrément
        while (t < 1) {                                                                             // tant qu'on est pas arrivé
            transform.rotation = Quaternion.Euler(0, init + 90 * Mathf.Lerp(current, orientation, t), 0);  // modifier l'orientation
            t += tStep * Time.deltaTime;                                                            // incrémenter le timer
            yield return null;
        }
        // en fin de rotation
        transform.rotation = Quaternion.Euler(0, init + 90 * orientation, 0);  // éliminer les éventuels arrondis
        current = orientation;
    }
}
