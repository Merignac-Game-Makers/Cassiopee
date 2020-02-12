using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Déclencheurs de transit
/// </summary>
public class TransitTrigger : MonoBehaviour
{
	public Transform destinationPoint { get; set; }
	public TransitEnd transitEnd { get; set; }
	public CinemachineVirtualCamera localCam { get; set; }
	private PlayerManager player;
	private Stack<CinemachineVirtualCamera> vCams => CameraController.Instance.vCams;

	public void Start() {
		player = PlayerManager.Instance;
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponentInChildren<PlayerManager>()) {
			if (IsFacingForward(other)) {											// si on rentre dans le sens du transit
				if (localCam) {														//	  s'il existe une caméra dédiée pour ce transit
					localCam.gameObject.SetActive(true);                            //		activer la caméra locale
					vCams.Peek().gameObject.SetActive(false);                       //		désactiver la caméra précédente
				}
				player.StartTransitTo(destinationPoint.position);					// diriger le joueur vers le point de destination du transit
			} else {
				transitEnd.End();
			}
		}
	}

	/// <summary>
	/// Le joueur entre-t-il dnas le sens du transit ?
	/// </summary>
	/// <param name="other">le joueur</param>
	/// <returns>positif si le joueur entre dans le sens du transit</returns>
	public bool IsFacingForward(Collider other){
		var collisionDirection = transform.position - other.transform.position;
		return Vector3.Dot(collisionDirection, transform.forward) > 0;
	}

}
