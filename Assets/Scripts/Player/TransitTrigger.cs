using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static CameraController;
using UnityEngine.AI;

/// <summary>
/// Déclencheurs de transit
/// </summary>
public class TransitTrigger : MonoBehaviour
{
	public bool reverse = false;
	public Transit transit { get; set; }
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
			if (IsFacingForward(player.m_Agent)) {                                  // si on rentre dans le sens du transit
				//if (localCam) {                                                     //	  s'il existe une caméra dédiée pour ce transit
				//	localCam.gameObject.SetActive(true);                            //		activer la caméra locale
				//	vCams.Peek().cam.gameObject.SetActive(false);                   //		désactiver la caméra précédente
				//}
				transit.StartPath(reverse);
				MagicManager.Instance.ResetConstellation();								// désactiver les objets magiques en quittant le lieu actuel
			} else {																// si on entre dans l'autre sens
				//if (localCam) {                                                         //	  s'il existe une caméra dédiée pour ce transit
				//	vCams.Peek().cam.gameObject.SetActive(true);                        //		réactiver la caméra précédente
				//	localCam.gameObject.SetActive(false);                               //		désactiver la caméra locale
				//}
				transit.Exit();
			}
		}
	}

	/// <summary>
	/// Le joueur entre-t-il dans le sens du transit ?
	/// </summary>
	/// <param name="agent">le joueur</param>
	/// <returns>positif si le joueur entre dans le sens du transit</returns>
	public bool IsFacingForward(NavMeshAgent agent) {
		return Vector3.Dot(agent.velocity, transform.forward) > 0;
	}

}
