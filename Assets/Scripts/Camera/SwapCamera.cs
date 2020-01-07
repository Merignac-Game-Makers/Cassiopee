﻿using System.Collections;
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
	Collider[] colliders;                   // la zone à surveiller (éventuellement plusieurs colliders)
	GameObject player;                      // l'objet à surveiller

	public GameObject outPoint;

	CinemachineVirtualCamera cam;
	bool inside;

	Stack<CinemachineVirtualCamera> vCams => CameraController.Instance.vCams;
	CameraController camController;

	// initialisation des variables
	void Start() {
		camController = CameraController.Instance;
		cinemachineBrain = CameraController.Instance.gameObject.GetComponentInChildren<CinemachineBrain>();
		localCam = gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
		colliders = gameObject.GetComponentsInChildren<Collider>();
		localCam.gameObject.SetActive(false);
		player = PlayerManager.Instance.gameObject;
	}

	void Update() {

		// le personnage est-il à l'intérieur du volume surveillé ?
		inside = false;
		foreach (Collider collider in colliders) {							// pour chaque collider
			collider.enabled = true;										//	activer le collider
			inside = collider.bounds.Contains(player.transform.position);	//	tester
			collider.enabled = false;										//	désactier le collider
			if (inside) break;												// si on est à l'intérieur => interrompre la boucle
		}
		// si on est à l'intérieur du volume surveillé
		if (inside && !vCams.Contains(localCam)) {							// si la caméra localde n'est pas dans la pile des caméras
			localCam.gameObject.SetActive(true);							// activer la caméra locale
			vCams.Peek().gameObject.SetActive(false);						// désactiver la caméra précédente
			vCams.Push(localCam);                                           // ajouter la caméra locale à la pile
			CameraController.Instance.m_CurrentDistance = Dist(vCams.Peek().m_Lens.FieldOfView); // restaurer facteur de zoom
			if (outPoint != null) {
				UIManager.Instance.exitButton.GetComponent<Exit>().outPoint = outPoint.transform.position;
				UIManager.Instance.exitButton.gameObject.SetActive(true);	// si besoin, afficher le bouton 'exit'
			}
			// si on est à l'extérieur et que la caméra locale est active => restaurer la caméra précédente
		}
		if (!inside && vCams.Contains(localCam)) {                          // si la caméra locale est dans la pile
			vCams.Pop();                                                    // retirer la caméra locale de la pile
			localCam.gameObject.SetActive(false);							// désactiver la caméra locale
			vCams.Peek().gameObject.SetActive(true);						// réactiver la caméra précédente
			CameraController.Instance.m_CurrentDistance = Dist(vCams.Peek().m_Lens.FieldOfView); // restaurer facteur de zoom
			UIManager.Instance.exitButton.gameObject.SetActive(false);		// masquer le bouton 'exit'
		}
	}
	float Dist(float fov) {
		return ((fov - camController.MinAngle) / (camController.MaxAngle - camController.MinAngle));
	}
}
