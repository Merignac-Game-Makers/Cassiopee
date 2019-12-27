using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class SwapCamera : MonoBehaviour
{
	/// <summary>
	/// Changer de caméra lorsque 1 objet (player) entre dans une zone à surveiller
	/// </summary>
	/// 
	CinemachineBrain cinemachineBrain;      // cinemachine
	CinemachineVirtualCamera localCam;      // la caméra locale à activer pour la zone
	Collider m_Collider;                    // la zone à surveiller
	GameObject player;                      // l'objet à surveiller


	CinemachineVirtualCamera cam;
	bool inside;

	List<CinemachineVirtualCamera> vCams => CameraController.Instance.vCams;
	

	// initialisation des variables
	void Start() {
		cinemachineBrain = CameraController.Instance.gameObject.GetComponentInChildren<CinemachineBrain>();
		localCam = gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
		m_Collider = gameObject.GetComponentInChildren<Collider>();
		localCam.gameObject.SetActive(false);
		player = PlayerManager.Instance.gameObject;
	}

	void Update() {
		m_Collider.enabled = true;
		inside = m_Collider.bounds.Contains(player.transform.position);
		m_Collider.enabled = false;
		// si on est à l'intérieur du volume surveillé
		if (inside && localCam != vCams.Contains(localCam)) {
			localCam.gameObject.SetActive(true);
			vCams.Last().gameObject.SetActive(false);
			vCams.Add(localCam);
		// si on est à l'extérieur et que la caméra locale est active => restaurer la caméra active avant d'entrer
		}  
		if (!inside && localCam == vCams.Contains(localCam)) {
			int idx = vCams.IndexOf(localCam);
			vCams.RemoveRange(idx, vCams.Count - idx);
			localCam.gameObject.SetActive(false);
			vCams.Last().gameObject.SetActive(true);
		}
	}
}
