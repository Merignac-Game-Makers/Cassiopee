using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class EnterHouse : MonoBehaviour
{
	public Collider sas;
	public Collider inside;
	public GameObject enterZone;
	public Transform lookAtAim;
	NavMeshAgent player;
	LookAtConstraint lookAtContraint;
	ConstraintSource constrainSource;
	// Start is called before the first frame update
	void Start() {
		player = PlayerManager.Instance.m_Agent;
		if (lookAtAim != null) {
			lookAtContraint = player.GetComponentInChildren<LookAtConstraint>();
			constrainSource = new ConstraintSource();
			constrainSource.sourceTransform = lookAtAim;
			constrainSource.weight = 1;
		}
	}


	public void Enter() {
		if (!isInside()) {
			player.SetDestination(enterZone.transform.position);
			if (lookAtAim != null) {
				StartCoroutine(ILookAt());
			}
		}
	}

	bool isInside() {
		return inside.bounds.Contains(player.transform.position);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject == player.gameObject)
			Enter();
	}

	IEnumerator ILookAt() {
		lookAtContraint.SetSource(0, constrainSource);
		lookAtContraint.constraintActive = true;
		while (player.hasPath)
			yield return new WaitForSeconds(1f);
		lookAtContraint.constraintActive = false;

	}
}
