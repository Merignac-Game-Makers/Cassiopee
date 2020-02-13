using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galina : MonoBehaviour
{
    [Range (.5f, 1.5f)]
    public float speed = 1;
    void Start()
    {
        var anim = GetComponent<Animator>();
        if (anim) {
            anim.speed = speed;
        }
    }
}
