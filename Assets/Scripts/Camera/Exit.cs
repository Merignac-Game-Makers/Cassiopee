using UnityEngine;

public class Exit : MonoBehaviour
{
	[HideInInspector]
	public Vector3 outPoint;

	bool? prevStatus;

	public void GetOut() {
		PlayerManager.Instance.m_Agent.SetDestination(outPoint);
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
