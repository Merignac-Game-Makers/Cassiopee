using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MotionMode { idle, walk, run, jump }

[CreateAssetMenu(fileName = "MotionParams", menuName = "Custom/Motion Parameters", order = -999)]

public class MotionParams : ScriptableObject
{
    public MotionMode mode;
    public float speed;
    public float angularSpeed;
    public float acceleration;

    public MotionParams(MotionMode mode, float speed, float acceleration, float angularSpeed) {
        this.mode = mode;
        this.speed = speed;
        this.acceleration = acceleration;
        this.angularSpeed = angularSpeed;
    }
}
