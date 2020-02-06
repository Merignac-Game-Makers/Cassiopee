// LocomotionSimpleAgent.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static LocomotionSimpleAgent;
using static MotionMode;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class LocomotionSimpleAgent : MonoBehaviour
{
    public Dictionary<MotionMode, MotionParams> motionModes;
    [HideInInspector]
    public MotionMode motion = idle;

    public float acceleration = 15f;
    public float deceleration = 150f;
    public float closeEnoughMeters = 4f;

    Animator anim;
    NavMeshAgent agent;

    Vector2 deltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    Vector2 smoothDeltaPosition = Vector2.zero;

    Vector3 worldDeltaPosition = Vector3.zero;

    void Start() {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        // Don’t update position automatically
        agent.updatePosition = false;

        motionModes = new Dictionary<MotionMode, MotionParams>();
        motionModes[walk] = (MotionParams)Resources.Load("Navigation/MotionParams/Walk");//new MotionParams(walk, 1, 3600, 1);
        motionModes[run] = (MotionParams)Resources.Load("Navigation/MotionParams/Run");//new MotionParams(run, 10, 3600, 15);
    }

    /// <summary>
    /// Relation entre la position du NavMeshAgent et la position du personnage 
    /// </summary>
    void OnAnimatorMove() {

        worldDeltaPosition = agent.nextPosition - transform.position;                           // vecteur jusqu'au point de passage suivant

        // Transformer 'worldDeltaPosition' en coordonnées locales du personnage
        deltaPosition.x = Vector3.Dot(transform.right, worldDeltaPosition);
        deltaPosition.y = Vector3.Dot(transform.forward, worldDeltaPosition);

        // lisser les variations
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.95f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // ### Lien strict => position perso = position agent
        // => risque de  glissement de pied si le 'BlendTree' et la vitesse de l'agent ne sont pas parfaitement réglés
        // if (Time.deltaTime > .001f)
        //    velocity = smoothDeltaPosition / Time.deltaTime;
        // transform.position = agent.nextPosition;


        // ### Lien plus élastique => le personnage suit l'agent lorsqu'il s'éloigne trop
        // => démarrage et arrêt plus souples
        // => laisse le temps de la rotation sur place avant de démarrer
        // Rem : ajuster la transition de démarrage pour éviter le 'pédalage sur place'
        // Rem : ajuster la transition d'arrêt pour éviter le glissement à l'arrivée        
        if (Time.deltaTime > .001f && worldDeltaPosition.magnitude > agent.radius * .50f)   // Rem : le multiplicateur définit la réactivité au démarrage
            velocity = smoothDeltaPosition / Time.deltaTime *.5f;                           // Rem : le multiplicateur définit la progressivité dans le BlendTree
        else
            velocity = Vector3.zero;
        if (worldDeltaPosition.magnitude > agent.radius * .25f) {                           // Rem : le multiplicateur définit la distance d'arrêt
            transform.position = agent.nextPosition - worldDeltaPosition * 0.9f;            // Rem : le multiplicateur définit l'élasticité
        } else {
            smoothDeltaPosition = Vector3.zero;
        }

        // ### Déplacement basé sur l'animation plutôt que sur l'agent 
        // => aucun glissement de pied mais course en zig-zag (?)
        // (en utilisant l'altitude de l'agent)
        //Vector3 position = anim.rootPosition;
        //position.y = agent.nextPosition.y;
        //transform.position = position;

        // déclencher le mouvement si la vitesse est supérieure au seuil de déclenchement
        // trigger => on bouge / ou pas
        // les paramètres de vitesse sont transmis au 'BlendTree' pour choisir l'animation de déplacement
        // (marche / jogging / course...)
        bool shouldMove = velocity.magnitude > .1f && agent.remainingDistance > agent.radius;
        anim.SetBool("move", shouldMove);       // trigger => déplacement
        anim.SetFloat("velx", velocity.x);      // vitesse X
        anim.SetFloat("vely", velocity.y);      // vitesse Y

        // ##################################### NE FONCTIONNE PAS ################################
        // Anomalie Unity : accélère progressivement, freine brutalement
        //if (agent.hasPath)
        //    agent.acceleration = (agent.remainingDistance < closeEnoughMeters) ? deceleration : acceleration;
        //Debug.Log("remains : " + agent.remainingDistance + " - acc : " + agent.acceleration + " - spd : " + agent.velocity.magnitude);
        // ##################################### NE FONCTIONNE PAS ################################

        // ##################################### NE FONCTIONNE PAS ################################
        // ### parce que l'animation pilote l'orientation de la tête ???
        // Tourner la tête vers le point de destination suivant
        LookAt lookAt = GetComponent<LookAt>();
        if (lookAt)
            lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;
        // ##################################### NE FONCTIONNE PAS ################################



    }

}

