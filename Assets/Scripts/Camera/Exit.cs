using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
	[HideInInspector]
	public Vector3 outPoint;

	public void GetOut() {
		PlayerManager.Instance.m_Agent.SetDestination(outPoint);
	}
}
