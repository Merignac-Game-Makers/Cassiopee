using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using static CameraController;

/// <summary>
/// Changer de caméra lorsque le joueur change de zone
/// </summary>
/// 
public class SwapCamera : MonoBehaviour
{

	public CinemachineBrain cinemachineBrain { get; set; }      // cinemachine
	public LocalCam localCam { get; set; }						// la caméra locale à activer pour la zone
	public PlayerManager player { get; set; }                   // le gestionnaire du joueur
	public Collider playerCollider { get; set; }                // le collider du joueur

	ZoomBase zoom;                          // script Zoom si ce volume est un zoom (il doit être porté par un parent)

	public GameObject currentCam => cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject;

	public Stack<LocalCam> vCams => CameraController.Instance.vCams;
	public CameraController camController { get; set; }

	public float maxAngle = 45;
	public MotionMode motionMode = MotionMode.run;

	// initialisation des variables
	public virtual void Start() {
		camController = CameraController.Instance;
		cinemachineBrain = camController.gameObject.GetComponentInChildren<CinemachineBrain>(true);
		localCam = new LocalCam(gameObject.GetComponentInChildren<CinemachineVirtualCamera>(true), maxAngle, motionMode);
		localCam.cam.gameObject.SetActive(false);
		player = PlayerManager.Instance;
		playerCollider = player.gameObject.GetComponentInChildren<Collider>();
		zoom = GetComponentInParent<ZoomBase>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other == playerCollider) {
			if (this is PlaceCamera) {
				PlacesUI.Instance.SetCurrentPlace(this as PlaceCamera);
			}
			Enter();
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other == playerCollider) {
			if (this is PlaceCamera) {
				PlacesUI.Instance.SetCurrentPlace(null);
			}
			Exit();
		}
	}

	private void ActivateUpperCamera() {
		foreach (LocalCam lc in vCams) {								// dasactiver
			lc.SetActive(false);										// toutes les caméras
		}
		vCams.Peek().SetActive(true);                                   // activer la caméra du sommet de la pile
		camController.m_CurrentDistance = Dist(vCams.Peek().cam.m_Lens.FieldOfView); // restaurer facteur de zoom
		camController.MaxAngle = vCams.Peek().maxAngle;                 // régler le zoom max
		player.SetMotionMode(vCams.Peek().motionMode);  // régler le mode de déplacement
	}

	float Dist(float fov) {
		return ((fov - camController.MinAngle) / (camController.MaxAngle - camController.MinAngle));
	}

	public void Enter() {
		if (vCams.Contains(localCam)) {                                 // si la caméra locale est déjà dans la pile des caméras
			for (int i = vCams.Count - 1; i > 0; i--) {                     // retirer de la pile
				if (vCams.Peek() != localCam)                               // toutes les caméras
					vCams.Pop();                                            // au dessus
				else                                                        // de la 
					break;                                                  // caméra locale
			}
		} else {                                                        // si la caméra locale n'est pas dans la pile
			vCams.Push(localCam);                                           // ajouter la caméra locale à la pile
		}
		ActivateUpperCamera();                                          // activer la caméra au sommet de la pile
	}

	public void Exit() {
		if (vCams.Peek() == localCam && vCams.Count > 1) {              // si la caméra locale est sur la pile et n'est pas seule
			vCams.Pop();                                                    // retirer la caméra locale du sommet de la pile
			localCam.SetActive(false);										// désactiver la caméra locale
		} else {                                                        // sinon
			var array = new List<LocalCam>(new Stack<LocalCam>(vCams.ToArray()).ToArray());  // transformer la pile en liste (2 fois pour éviter le retournement)
			if (array[0] != localCam) {                                     // si la caméra locale n'est pas tout en bas de la pile
				array.Remove(localCam);                                         // retirer la caméra locale de la liste
				camController.vCams = new Stack<LocalCam>(array);               // re convertir la liste en pile		
			}
		}
		ActivateUpperCamera();                                          // activer la caméra au sommet de la pile
	}
}
