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
	CinemachineVirtualCamera mainCam;       // la caméra active AVANT d'entrer dans la zone
	CinemachineVirtualCamera localCam;      // la caméra locale à activer pour la zone
	Collider m_Collider;                    // la zone à surveiller
	GameObject player;                      // l'objet à surveiller


	CinemachineVirtualCamera cam;

	// initialisation des variables
	void Start() {
		cinemachineBrain = CameraController.Instance.gameObject.GetComponentInChildren<CinemachineBrain>();
		localCam = gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
		m_Collider = gameObject.GetComponentInChildren<Collider>();
		player = PlayerManager.Instance.gameObject;
	}

	void Update() {
		// si on est à l'intérieur du volume surveillé
		if (m_Collider.bounds.Contains(player.transform.position)) {		
			// récupérer la caméra active
			cam = cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
			// si ce n'est pas la caméra locale => basculer sur la caméra locale
			if (cam != localCam) mainCam = cam;
			localCam.gameObject.SetActive(true);
			mainCam?.gameObject.SetActive(false);
		// si on est à l'extérieur et que la caméra locale est active => restaurer la caméra active avant d'entrer
		} else if (cam == localCam) {
			localCam.gameObject.SetActive(false);
			mainCam?.gameObject.SetActive(true);
		}
	}
}
