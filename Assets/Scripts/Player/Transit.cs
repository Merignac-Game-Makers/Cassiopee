using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Transit : MonoBehaviour
{
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
			if (IsFacingForward(other)) {											// si on rentre dans le sens du transit
				if (localCam) {														//	  s'il existe une caméra dédiée pour ce transit
					localCam.gameObject.SetActive(true);                            //		activer la caméra locale
					vCams.Peek().gameObject.SetActive(false);                       //		désactiver la caméra précédente
				}
				player.StartTransitTo(destinationPoint.position);					// diriger le joueur vers le point de destination du transit
			} else {
				// TODO: revoir la fin de transit
				// (ce n'est pas une entrée "dans le mauvais sens" qui doit déclarer un transit terminé)
				player.EndTransit();

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
