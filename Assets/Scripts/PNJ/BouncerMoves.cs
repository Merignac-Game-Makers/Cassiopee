using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerMoves : PnjMoves
{
	public Transform restPosition;
	public Animator house;
	Vector3 pos;

	protected override void Start() {
		base.Start();
		pos = restPosition.position;
	}

	public override void MoveToPos(Vector3 position) {
		agent.destination = position;
	}

	public void GoBack() {
		MoveToPos(pos);
		var anim = house.GetComponentInChildren<Animator>();
		if (anim) {
			anim.SetTrigger("OpenDoors");
		}
	}
}
