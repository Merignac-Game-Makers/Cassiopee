using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;
using static CameraController;

/// <summary>
/// Masquer les aides visuelles des transits
/// 
/// Les actions en début de transit sont portées par la classe <see cref="TransitTrigger"/>
/// Les actions en fin de transit sont portées par la classe <see cref="TransitEnd"/>
/// </summary>
public class Transit : SwapCamera
{
	// points : start, end, wayPoints
	public Transform startPoint;
	public Transform endPoint;
	public Transform[] path;

	private int idx = 0;
	private Vector3 targetPoint;

	public bool reverse { get; set; } = false;

	public override void Start() {
		base.Start();
		// désactiver toute représentation graphique
		// (les renderers sont des aides pour la mise en place)
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer r in renderers) {
			r.enabled = false;
		}

		// passer les variables utiles aux triggers
		foreach (TransitTrigger tt in GetComponentsInChildren<TransitTrigger>()) {
			tt.localCam = localCam.cam;
			tt.transit = this;
		}
	}

	/// <summary>
	/// Commencer un transit
	/// si reverse = true : c'est un trajet retour
	/// on parcours alors les points en ordre inverse
	/// </summary>
	/// <param name="reverse">si reverse = true : c'est un trajet retour</param>
	public void StartPath(bool reverse) {
		Enter();
		this.reverse = reverse;
		player.m_Agent.autoBraking = false;		// désactiver le freinage à l'arrivée pour éviter les à-coups au passage des checkPoints
		idx = reverse? path.Length: -1;			// on commence au début ... ou à la fin si c'est un retour
		player.inTransit = true;				// on est en transit
		player.currentTransit = this;
		GotoNextPoint();
	}

	/// <summary>
	/// Fin de parcours
	/// </summary>
	private void EndPath() {
		player.StopAgent();                                 // arrêter le déplacement du joueur
		player.currentTransit = null;                       // on n'est plus en transit

		Exit();
	}

	/// <summary>
	/// Aller au point suivant
	/// </summary>
	void GotoNextPoint() {
		// si on a atteint le dernier point... (ou le 1er si on est sur un retour)
		if ((reverse && targetPoint == startPoint.position) || (!reverse && targetPoint == endPoint.position)) {
			EndPath();                                      //   => fin de parcours
			return;
		}
		
		idx = idx + (reverse ? -1 : 1);         // point suivant
		if (!reverse) {
			targetPoint = (idx == path.Length ? endPoint.position : path[idx].position);
		} else {
			targetPoint = (idx ==-1 ? startPoint.position : path[idx].position);
		}

		player.m_Agent.destination = targetPoint;                    // donner la destination.

		if ((reverse && targetPoint == startPoint.position) || (!reverse && targetPoint == endPoint.position)) {
			player.m_Agent.autoBraking = true;						// rétablir le freinage
		}
	}

	void Update() {
		// Choose the next destination point when the agent gets close to the current one.
		if (player.currentTransit == this && !player.m_Agent.pathPending && player.m_Agent.remainingDistance < player.m_Agent.radius * 5)
			GotoNextPoint();
	}

}
