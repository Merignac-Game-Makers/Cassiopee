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
	Collider player;

	public GameObject outPoint;				// point de sortie si on est en intérieur et qu'on appuie sur le bouton Exit
	ZoomBase zoom;							// script Zoom si ce volume est un zoom (il doit être porté par un parent)
	ZoomUI zoomUI;							// fenêtre Zoom pour affichage

	GameObject currentCam => cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject;

	Stack<CinemachineVirtualCamera> vCams => CameraController.Instance.vCams;
	CameraController camController;

	// initialisation des variables
	void Start() {
		camController = CameraController.Instance;
		cinemachineBrain = CameraController.Instance.gameObject.GetComponentInChildren<CinemachineBrain>();
		localCam = gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
		localCam.gameObject.SetActive(false);
		player = PlayerManager.Instance.gameObject.GetComponentInChildren<Collider>();
		zoom = GetComponentInParent<ZoomBase>();
		if (zoom)
			zoomUI = zoom.zoomUI;
	}

	private void OnTriggerEnter(Collider other) {
		// si on est à l'intérieur du volume surveillé
		if (other == player && !vCams.Contains(localCam)) {                 // si la caméra localde n'est pas dans la pile des caméras
			localCam.gameObject.SetActive(true);                            // activer la caméra locale
			currentCam.SetActive(false);                                    // désactiver la caméra précédente
			vCams.Push(localCam);                                           // ajouter la caméra locale à la pile
			CameraController.Instance.m_CurrentDistance = Dist(vCams.Peek().m_Lens.FieldOfView); // restaurer facteur de zoom
			if (outPoint != null) {
				UIManager.Instance.exitButton.GetComponent<Exit>().outPoint = outPoint.transform.position;
				UIManager.Instance.exitButton.gameObject.SetActive(true);   // si besoin, afficher le bouton 'exit'
			}

			if (zoomUI)
				zoomUI.Enter(this);
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other == player && vCams.Contains(localCam)) {                      // si la caméra locale est dans la pile
			Exit();
		}
	}

	float Dist(float fov) {
		return ((fov - camController.MinAngle) / (camController.MaxAngle - camController.MinAngle));
	}

	public void Exit() {
		vCams.Pop();														// retirer la caméra locale de la pile
		if (currentCam == localCam.gameObject) {                            // si la caméra locale est active
			localCam.gameObject.SetActive(false);                           // désactiver la caméra locale
			vCams.Peek().gameObject.SetActive(true);                        // réactiver la caméra précédente
			CameraController.Instance.m_CurrentDistance = Dist(vCams.Peek().m_Lens.FieldOfView); // restaurer facteur de zoom
			UIManager.Instance.exitButton.gameObject.SetActive(false);      // masquer le bouton 'exit'
		}
	}
}
