using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Transit : MonoBehaviour
{

	public Collider sas;
	public Transform destinationPoint;
	public CinemachineVirtualCamera localCam;

	private PlayerManager player;
	private CinemachineBrain cinemachineBrain;      // cinemachine
	private Stack<CinemachineVirtualCamera> vCams => CameraController.Instance.vCams;

	public void Start() {
		player = PlayerManager.Instance;
		cinemachineBrain = CameraController.Instance.gameObject.GetComponentInChildren<CinemachineBrain>();
		localCam?.VirtualCameraGameObject.SetActive(false);
		// désactiver toute représentation graphique
		// (les renderers sont des aides pour la mise en place)
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer r in renderers) {
			r.enabled = false;
		}
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponentInChildren<PlayerManager>()) {
			if (sas.bounds.Contains(other.transform.position)) {
				if (localCam) {														// s'il existe une caméra dédiée pour ce transit
					localCam.gameObject.SetActive(true);                            // activer la caméra locale
					vCams.Peek().gameObject.SetActive(false);                       // désactiver la caméra précédente
				}
				player.StartTransitTo(destinationPoint.position);
			} else {
				player.EndTransit();

			}
		}
	}
}
