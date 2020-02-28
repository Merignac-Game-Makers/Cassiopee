using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using static CameraController;

public class SwapCamera : MonoBehaviour
{
	/// <summary>
	/// Changer de caméra lorsque 1 objet (player) entre dans une zone à surveiller
	/// </summary>
	/// 
	CinemachineBrain cinemachineBrain;      // cinemachine
	LocalCam localCam;      // la caméra locale à activer pour la zone
	Collider player;

	ZoomBase zoom;							// script Zoom si ce volume est un zoom (il doit être porté par un parent)
	ZoomUI zoomUI;							// fenêtre Zoom pour affichage

	GameObject currentCam => cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject;

	Stack<LocalCam> vCams => CameraController.Instance.vCams;
	CameraController camController;

	public float maxAngle = 45;
	public MotionMode motionMode = MotionMode.run;

	// initialisation des variables
	void Start() {
		camController = CameraController.Instance;
		cinemachineBrain = camController.gameObject.GetComponentInChildren<CinemachineBrain>();
		localCam = new LocalCam(gameObject.GetComponentInChildren<CinemachineVirtualCamera>(), maxAngle, motionMode);
		localCam.cam.gameObject.SetActive(false);
		player = PlayerManager.Instance.gameObject.GetComponentInChildren<Collider>();
		zoom = GetComponentInParent<ZoomBase>();
		if (zoom)
			zoomUI = zoom.zoomUI;
	}

	private void OnTriggerEnter(Collider other) {
		// si on est à l'intérieur du volume surveillé
		if (other == player && !vCams.Contains(localCam)) {                 // si la caméra locale n'est pas dans la pile des caméras
			//if (!PlayerManager.Instance.inTransit) {
				localCam.cam.gameObject.SetActive(true);                        // activer la caméra locale
				currentCam.SetActive(false);                                // désactiver la caméra précédente
			//}
			vCams.Push(localCam);                                           // ajouter la caméra locale à la pile
			camController.m_CurrentDistance = Dist(vCams.Peek().cam.m_Lens.FieldOfView); // restaurer facteur de zoom
			camController.MaxAngle = vCams.Peek().maxAngle;
			PlayerManager.Instance.SetMotionMode(vCams.Peek().motionMode);

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
		vCams.Pop();															// retirer la caméra locale de la pile
		if (currentCam == localCam.cam.gameObject) {							// si la caméra locale est active
			localCam.cam.gameObject.SetActive(false);                           // désactiver la caméra locale
			vCams.Peek().cam.gameObject.SetActive(true);                        // réactiver la caméra précédente
			camController.MaxAngle = vCams.Peek().maxAngle;
			PlayerManager.Instance.SetMotionMode(vCams.Peek().motionMode);
			camController.m_CurrentDistance = Dist(vCams.Peek().cam.m_Lens.FieldOfView); // restaurer facteur de zoom
			UIManager.Instance.exitButton.gameObject.SetActive(false);			// masquer le bouton 'exit'
		}
	}
}
