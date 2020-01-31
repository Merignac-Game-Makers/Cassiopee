using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerMoves : PnjMoves
{
	public Vector3 move;

	public override void MoveToPos(Vector3 position) {
		agent.destination = position;
	}

	public void GoBack() {
		var pos = agent.transform.position + move;
		MoveToPos(pos);
	}
}
