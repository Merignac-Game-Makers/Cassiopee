// LookAt.cs
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class LookAt : MonoBehaviour
{
	public Transform head = null;
	public Vector3 lookAtTargetPosition;
	public float lookAtCoolTime = 0.2f;
	public float lookAtHeatTime = 0.2f;
	public bool looking = true;

	private Vector3 lookAtPosition;
	private Animator animator;
	private NavMeshAgent agent;
	
	public float lookAtWeight = 1.0f;

	void Start() {
		if (!head) {
			Debug.LogError("No head transform - LookAt disabled");
			enabled = false;
			return;
		}
		animator = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		lookAtTargetPosition = head.position + transform.forward;
		lookAtPosition = lookAtTargetPosition;

		if (!animator || !agent) {		// si l'agent ou l'animateur ne sont pas définis
			looking = false;			// on désactive le script
		}
	}

	private void OnAnimatorIK(int layerIndex) {
		lookAtTargetPosition = agent.nextPosition;
		lookAtTargetPosition.y = head.position.y;
		float lookAtTargetWeight = looking ? 1f : 0.0f;

		Vector3 curDir = lookAtPosition - head.position;
		Vector3 futDir = lookAtTargetPosition - head.position;

		curDir = Vector3.RotateTowards(curDir, futDir, 6.28f * Time.deltaTime, float.PositiveInfinity);
		lookAtPosition = head.position + curDir;
		lookAtPosition.y = head.position.y;

		float blendTime = lookAtTargetWeight > lookAtWeight ? lookAtHeatTime : lookAtCoolTime;
		lookAtWeight = Mathf.MoveTowards(lookAtWeight, lookAtTargetWeight, Time.deltaTime / blendTime);
		animator.SetLookAtWeight(lookAtWeight, 0.2f, 0.5f, 0.7f, 0.5f);
		animator.SetLookAtPosition(lookAtPosition);

	}
}
