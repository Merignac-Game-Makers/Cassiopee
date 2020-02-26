using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

public class EnterHouse : MonoBehaviour
{
	public Collider inside;
	public Transform inPoint;
	public Transform outPoint;
	public Transform lookAtAim;
	PlayerManager player;
	LookAtConstraint lookAtContraint;
	ConstraintSource constrainSource;

	float maxSpeed;

	private void OnMouseDown() {
		player = PlayerManager.Instance;
		maxSpeed = player.m_Agent.speed;
		Enter();
	}

	public void Enter() {
		StartCoroutine(IEnter());
		
	}
	public void Exit() {
		player = PlayerManager.Instance;
		player.m_Agent.SetDestination(outPoint.position);
	}

	bool isInside() {
		return inside.bounds.Contains(player.transform.position);
	}

	IEnumerator IEnter() {
		// définir la destination
		player.m_Agent.SetDestination(inPoint.position);
		player.inTransit = true;							// ce déplacement est assimilable à un transit
		// approcher de l'entrée
		while (inside.bounds.SqrDistance(player.transform.position) > player.m_Agent.radius * player.m_Agent.radius)
			yield return new WaitForSeconds(.1f);
		// à l'entrée : réduire la vitesse
		player.m_Agent.speed = 5;
		while (player.m_Agent.hasPath)
			yield return new WaitForSeconds(.25f);
		// quand on est arrivé
		UIManager.Instance.exitButton.outPoint = outPoint.position;
		UIManager.Instance.exitButton.defaultSpeed = maxSpeed;
		UIManager.Instance.exitButton.Show();                                       // afficher le bouton Exit
		player.inTransit = false;													// on n'est plus en transit
		if (lookAtAim != null) {													// si le personnage doit regarder dans une direction précise
			lookAtContraint = player.GetComponentInChildren<LookAtConstraint>();	// définir la direction
			constrainSource = new ConstraintSource() { sourceTransform = lookAtAim, weight = 1};
			lookAtContraint.SetSource(0, constrainSource);
			lookAtContraint.constraintActive = true;
			yield return new WaitForSeconds(.2f);
			lookAtContraint.constraintActive = false;
		}
	}
}
