using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Point d'arrivée de transit
/// </summary>
public class TransitEnd : MonoBehaviour
{
	public CinemachineVirtualCamera localCam;
	private PlayerManager player;
	private Stack<CinemachineVirtualCamera> vCams => CameraController.Instance.vCams;

	public void Start() {
		player = PlayerManager.Instance;
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponentInChildren<PlayerManager>()) {
			if (localCam) {                                                     //	  s'il existe une caméra dédiée pour ce transit
				localCam.gameObject.SetActive(false);                           //		désactiver la caméra locale
				vCams.Peek().gameObject.SetActive(true);						//		réactiver la caméra précédente
			}
			player.StopAgent();													// arrêter le déplacement du joueur

		}
	}
}
