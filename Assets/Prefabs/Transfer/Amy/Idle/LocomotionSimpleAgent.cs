// LocomotionSimpleAgent.cs
using UnityEngine;
using UnityEngine.AI;
using static LocomotionSimpleAgent.Motion;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class LocomotionSimpleAgent : MonoBehaviour
{
    public enum Motion { idle, walk, run, jump }
    public Motion motion = idle;

    public float acceleration = 15f;
    public float deceleration = 150f;
    public float closeEnoughMeters = 4f;

    Animator anim;
    NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    void Start() {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        // Don’t update position automatically
        agent.updatePosition = false;
    }

    void Update() {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.5f);
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
        if (agent.hasPath)
            agent.acceleration = (agent.remainingDistance < closeEnoughMeters) ? deceleration : acceleration;
        Debug.Log("remains : " + agent.remainingDistance + " - acc : " + agent.acceleration);
        // ##################################### NE FONCTIONNE PAS ################################

        transform.position = agent.nextPosition;
        //// Pull character towards agent
        //if (worldDeltaPosition.magnitude > agent.radius)
        //    transform.position = agent.nextPosition - 0.9f * worldDeltaPosition;

        LookAt lookAt = GetComponent<LookAt>();
        if (lookAt)
            lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;
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
