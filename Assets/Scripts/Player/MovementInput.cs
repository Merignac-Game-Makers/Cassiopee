//Copyright Filmstorm (C) 2018 - Movement Controller for Root Motion and built in IK solver
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//This script requires you to have setup your animator with 3 parameters, "InputMagnitude", "InputX", "InputZ"
//With a blend tree to control the inputmagnitude and allow blending between animations.
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class MovementInput : MonoBehaviour
{
	#region Variables
	private float inputX;		//Left and Right Inputs
	private float inputZ;		//Forward and Back Inputs

	private Animator anim;										//Animator
	private NavMeshAgent agent;									//Navigation agent

	private Vector3 rightFootPosition, leftFootPosition, leftFootIkPosition, rightFootIkPosition;
	private Quaternion leftFootIkRotation, rightFootIkRotation;
	private float lastPelvisPositionY, lastRightFootPositionY, lastLeftFootPositionY;

	//[Range(0.01f, 1f)] public float timeScale = 1;

	[Header("Feet Grounder")]
	public bool enableFeetIk = true;
	[Range(0, 9)] [SerializeField] private float heightFromGroundRaycast = 1.14f;
	[Range(0, 10)] [SerializeField] private float raycastDownDistance = 1.5f;
	//[SerializeField] private LayerMask environmentLayer;
	public LayerMask environmentLayer;
	[SerializeField] private float pelvisOffset = 0f;
	[Range(0, 1)] [SerializeField] private float pelvisUpAndDownSpeed = 0.28f;
	[Range(0, 1)] [SerializeField] private float feetToIkPositionSpeed = 0.5f;

	//public string leftFootAnimVariableName = "LeftFootCurve";
	//public string rightFootAnimVariableName = "RightFootCurve";

	//public bool useProIkFeature = false;
	public bool showSolverDebug = true;


	[Header("Animation Smoothing")]
	[Range(0, 1f)]
	public float animSmoothTime = 0.2f; //velocity dampening

	public LayerMask EnvironmentLayer { get => EnvironmentLayer2; set => EnvironmentLayer2 = value; }
	public LayerMask EnvironmentLayer1 { get => EnvironmentLayer2; set => EnvironmentLayer2 = value; }
	public LayerMask EnvironmentLayer2 { get => environmentLayer; set => environmentLayer = value; }

	#endregion

	#region Initialization
	// Initialization of variables
	void Start() {
		//timeScale = 1f;

		anim = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();

		if (anim == null)
			Debug.LogError("We require " + transform.name + " game object to have an animator. This will allow for Foot IK to function");
		if (agent == null)
			Debug.LogError("We require " + transform.name + " game object to have a Nav Mesh Agent. This will allow for Foot IK to function");
	}
	#endregion


	#region PlayerMovement
	void InputMagnitude() {
		//Calculate Input Vectors
		inputX = agent.velocity.x;
		inputZ = agent.velocity.z;

		anim.SetFloat("InputX", inputX, animSmoothTime, Time.deltaTime * 2f);
		anim.SetFloat("InputZ", inputZ, animSmoothTime, Time.deltaTime * 2f);

		//Physically move player
		anim.SetFloat("InputMagnitude", agent.velocity.sqrMagnitude, animSmoothTime, Time.deltaTime);
		bool shouldMove = agent.velocity.magnitude > .1f && agent.remainingDistance > agent.radius;
		anim.SetBool("move", shouldMove);       // trigger => déplacement

	}

	#endregion


	#region FeetGrounding

	// Update is called once per frame
	void Update() {
		InputMagnitude();
		//Time.timeScale = timeScale;


		if (enableFeetIk == false) { return; }
		if (anim == null) { return; }

		AdjustFeetTarget(ref rightFootPosition, HumanBodyBones.RightFoot);
		AdjustFeetTarget(ref leftFootPosition, HumanBodyBones.LeftFoot);

		//find and raycast to the ground to find positions
		FeetPositionSolver(rightFootPosition, ref rightFootIkPosition, ref rightFootIkRotation); // handle the solver for right foot
		FeetPositionSolver(leftFootPosition, ref leftFootIkPosition, ref leftFootIkRotation); //handle the solver for the left foot
	}

	private void OnAnimatorIK(int layerIndex) {
		if (enableFeetIk == false) { return; }
		if (anim == null) { return; }

		MovePelvisHeight();

		//right foot ik position and rotation -- utilise the pro features in here
		anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

		//if (useProIkFeature) {
		//	anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat(rightFootAnimVariableName));
		//}

		MoveFeetToIkPoint(AvatarIKGoal.RightFoot, rightFootIkPosition, rightFootIkRotation, ref lastRightFootPositionY);

		//left foot ik position and rotation -- utilise the pro features in here
		anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

		//if (useProIkFeature) {
		//	anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat(leftFootAnimVariableName));
		//}

		MoveFeetToIkPoint(AvatarIKGoal.LeftFoot, leftFootIkPosition, leftFootIkRotation, ref lastLeftFootPositionY);
	}

	#endregion



	#region FeetGroundingMethods

	/// <summary>
	/// Moves the feet to ik point.
	/// </summary>
	/// <param name="foot">Foot.</param>
	/// <param name="positionIkHolder">Position ik holder.</param>
	/// <param name="rotationIkHolder">Rotation ik holder.</param>
	/// <param name="lastFootPositionY">Last foot position y.</param>
	void MoveFeetToIkPoint(AvatarIKGoal foot, Vector3 positionIkHolder, Quaternion rotationIkHolder, ref float lastFootPositionY) {
		Vector3 targetIkPosition = anim.GetIKPosition(foot);

		if (positionIkHolder != Vector3.zero) {
			targetIkPosition = transform.InverseTransformPoint(targetIkPosition);
			positionIkHolder = transform.InverseTransformPoint(positionIkHolder);

			float yVariable = Mathf.Lerp(lastFootPositionY, positionIkHolder.y, feetToIkPositionSpeed);
			targetIkPosition.y += yVariable;

			lastFootPositionY = yVariable;

			targetIkPosition = transform.TransformPoint(targetIkPosition);

			anim.SetIKRotation(foot, rotationIkHolder);
		}

		anim.SetIKPosition(foot, targetIkPosition);
	}
	/// <summary>
	/// Moves the height of the pelvis.
	/// </summary>
	private void MovePelvisHeight() {

		if (rightFootIkPosition == Vector3.zero || leftFootIkPosition == Vector3.zero || lastPelvisPositionY == 0) {
			lastPelvisPositionY = anim.bodyPosition.y;
			return;
		}

		float lOffsetPosition = leftFootIkPosition.y - transform.position.y;
		float rOffsetPosition = rightFootIkPosition.y - transform.position.y;

		float totalOffset = (lOffsetPosition < rOffsetPosition) ? lOffsetPosition : rOffsetPosition;

		Vector3 newPelvisPosition = anim.bodyPosition + Vector3.up * totalOffset;

		newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);

		anim.bodyPosition = newPelvisPosition;

		lastPelvisPositionY = anim.bodyPosition.y;

	}

	/// <summary>
	/// We are locating the Feet position via a Raycast and then Solving
	/// </summary>
	/// <param name="fromSkyPosition">From sky position.</param>
	/// <param name="feetIkPositions">Feet ik positions.</param>
	/// <param name="feetIkRotations">Feet ik rotations.</param>
	private void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIkPositions, ref Quaternion feetIkRotations) {
		//raycast handling section 
		RaycastHit feetOutHit;

		if (showSolverDebug)
			Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * raycastDownDistance, Color.yellow);

		if (Physics.Raycast(fromSkyPosition, Vector3.down, out feetOutHit, raycastDownDistance + heightFromGroundRaycast, EnvironmentLayer2)) {
			//finding our feet ik positions from the sky position
			feetIkPositions = fromSkyPosition;
			feetIkPositions.y = feetOutHit.point.y + pelvisOffset;
			feetIkRotations = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;

			return;
		}

		feetIkPositions = Vector3.zero; //it didn't work :(

	}
	/// <summary>
	/// Adjusts the feet target.
	/// </summary>
	/// <param name="feetPositions">Feet positions.</param>
	/// <param name="foot">Foot.</param>
	private void AdjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot) {
		feetPositions = anim.GetBoneTransform(foot).position;				// récupérer la postion du pied
		feetPositions.y = transform.position.y + heightFromGroundRaycast;	// repmonter la position pour la source du raycast

	}

	#endregion


}





