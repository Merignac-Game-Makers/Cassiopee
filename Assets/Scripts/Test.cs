using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
	public Transform inPoint;
	PlayerManager player;

	private void OnMouseDown() {
		Debug.Log("test");
		player = PlayerManager.Instance;
		player.m_Agent.SetDestination(inPoint.position);
		player.inTransit = true;
	}
	//public void OnMouseDown() {
	//	Debug.Log("test");
	//}
}
