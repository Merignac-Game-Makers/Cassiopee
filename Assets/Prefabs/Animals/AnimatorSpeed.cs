using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSpeed : MonoBehaviour
{
    [Range (.5f, 1.5f)]
    public float speed = 1f;
    void Start()
    {
        var anim = GetComponent<Animator>();
        if (anim) {
            anim.speed = speed;
        }
    }
}
