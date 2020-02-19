using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Point d'arrivée de transit
/// </summary>
public class TransitEnd : MonoBehaviour
{
	public CinemachineVirtualCamera localCam { get; set; }
	private PlayerManager player;
	private Stack<CinemachineVirtualCamera> vCams => CameraController.Instance.vCams;

	public void Start() {
		player = PlayerManager.Instance;
	}

	public void OnTriggerEnter(Collider other) {
		if (player.inTransit && other.gameObject.GetComponentInChildren<PlayerManager>()) {
			player.StopAgent();                                                 // arrêter le déplacement du joueur
		}
	}

	public void End() {
		if (localCam) {                                                     //	  s'il existe une caméra dédiée pour ce transit
			vCams.Peek().gameObject.SetActive(true);                        //		réactiver la caméra précédente
			localCam.gameObject.SetActive(false);                           //		désactiver la caméra locale
		}
	}
}
