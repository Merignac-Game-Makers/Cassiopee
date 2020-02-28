using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static CameraController;

/// <summary>
/// Déclencheurs de transit
/// </summary>
public class TransitTrigger : MonoBehaviour
{
	public Transform destinationPoint { get; set; }
	public TransitEnd transitEnd { get; set; }
	public CinemachineVirtualCamera localCam { get; set; }
	private PlayerManager player;
	private Collider playerCollider;
	private Stack<LocalCam> vCams => CameraController.Instance.vCams;

	public void Start() {
		player = PlayerManager.Instance;
		playerCollider = player.gameObject.GetComponentInChildren<Collider>();
	}

	public void OnTriggerEnter(Collider other) {
		if (other == playerCollider) {
		if (IsFacingForward(other)) {                                           // si on rentre dans le sens du transit
				if (localCam) {                                                     //	  s'il existe une caméra dédiée pour ce transit
					localCam.gameObject.SetActive(true);                            //		activer la caméra locale
					vCams.Peek().cam.gameObject.SetActive(false);                   //		désactiver la caméra précédente
				}
				player.m_Agent.SetDestination(destinationPoint.position);           // diriger le joueur vers la destination
				player.inTransit = true;											// on est en transit
				MagicManager.Instance.ResetConstellation();							// désactiver les objets magiques en quittant le lieu actuel

			} else {															// si on entre dans l'autre sens
				transitEnd.End();													//		transition de caméra de fin de transit
			}
		}
	}

	/// <summary>
	/// Le joueur entre-t-il dans le sens du transit ?
	/// </summary>
	/// <param name="other">le joueur</param>
	/// <returns>positif si le joueur entre dans le sens du transit</returns>
	public bool IsFacingForward(Collider other){
		var collisionDirection = transform.position - other.transform.position;
		return Vector3.Dot(collisionDirection, transform.forward) > 0;
	}

}
