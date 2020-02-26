﻿using UnityEngine;

public class Exit : MonoBehaviour
{
	public Vector3 outPoint { get; set; }
	public float defaultSpeed { get; set; } = 50f;

	bool? prevStatus;

	public void GetOut() {
		PlayerManager.Instance.m_Agent.SetDestination(outPoint);
		PlayerManager.Instance.m_Agent.speed = defaultSpeed;
		Hide();
	}

	public void SaveAndHide() {
		Save();
		Hide();
	}
	public void Save() {
		prevStatus = gameObject.activeInHierarchy;
	}

	public void Restore() {
		if (true == prevStatus) Show();
		if (false == prevStatus) Hide();
		prevStatus = null;
	}
	public void Hide() {
		gameObject.SetActive(false);
	}
	public void Show() {
		gameObject.SetActive(true);
	}

}
