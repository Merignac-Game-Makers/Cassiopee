using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
	[HideInInspector]
	public Vector3 outPoint;

	public void GetOut() {
		PlayerManager.Instance.m_Agent.SetDestination(outPoint);
	}
}
