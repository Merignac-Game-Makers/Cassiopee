// LocomotionSimpleAgent.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static LocomotionSimpleAgent.MotionMode;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class LocomotionSimpleAgent : MonoBehaviour
{
    public enum MotionMode { idle, walk, run, jump }
    public Dictionary<MotionMode, MotionParams> motionModes;
    [HideInInspector]
    public MotionMode motion = idle;

    public float acceleration = 15f;
    public float deceleration = 150f;
    public float closeEnoughMeters = 4f;

    Animator anim;
    NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    Vector3 prevSteeringTarget = Vector3.zero;

    void Start() {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        // Don’t update position automatically
        agent.updatePosition = false;
        motionModes = new Dictionary<MotionMode, MotionParams>();
        motionModes[walk] = new MotionParams(1, 3600, 1);
        motionModes[run] = new MotionParams(10, 3600, 15);
    }

    void Update() {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.25f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

        // Update animation parameters
        anim.SetBool("move", shouldMove);
        anim.SetFloat("velx", velocity.x);
        anim.SetFloat("vely", velocity.y);


        // ##################################### NE FONCTIONNE PAS ################################
        // speed up slowly, but stop quickly
        //if (agent.hasPath)
        //    agent.acceleration = (agent.remainingDistance < closeEnoughMeters) ? deceleration : acceleration;
        //Debug.Log("remains : " + agent.remainingDistance + " - acc : " + agent.acceleration + " - spd : " + agent.velocity.magnitude);
        // ##################################### NE FONCTIONNE PAS ################################

        transform.position = agent.nextPosition;
        //// Pull character towards agent
        //if (worldDeltaPosition.magnitude > agent.radius)
        //    transform.position = agent.nextPosition - 0.9f * worldDeltaPosition;

        LookAt lookAt = GetComponent<LookAt>();
        if (lookAt)
            lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;

        if( prevSteeringTarget != agent.steeringTarget) {
            Debug.Log("lookAtTargetPosition : " + lookAt.lookAtTargetPosition);
        }
        prevSteeringTarget = agent.steeringTarget;
    }

    void OnAnimatorMove() {
        // Update position to agent position
        // transform.position = agent.nextPosition;
        // Update position based on animation movement using navigation surface height
        Vector3 position = anim.rootPosition;
        position.y = agent.nextPosition.y;
        transform.position = position;

    }

}

    public class MotionParams : ScriptableObject
{
    public float speed;
    public float acceleration;
    public float angularSpeed;

    public MotionParams(float speed, float acceleration, float angularSpeed) {
        this.speed = speed;
        this.acceleration = acceleration;
        this.angularSpeed = angularSpeed;
    }
}