using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Masquer les aides visuelles des transits
/// 
/// Les actions en début de transit sont portées par la classe <see cref="TransitTrigger"/>
/// Les actions en fin de transit sont portées par la classe <see cref="TransitEnd"/>
/// </summary>
public class Transit : MonoBehaviour
{
	public CinemachineVirtualCamera localCam;

	public void Start() {
		// désactiver la caméra locale
		localCam.gameObject.SetActive(false);
		// désactiver toute représentation graphique
		// (les renderers sont des aides pour la mise en place)
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer r in renderers) {
			r.enabled = false;
		}
		// passer les variables utiles aux triggers et au point de destination
		var destionationPoint = GetComponentInChildren<TransitEnd>();
		foreach (TransitTrigger tt in GetComponentsInChildren<TransitTrigger>()) {
			tt.localCam = localCam;
			tt.destinationPoint = destionationPoint.transform;
			tt.transitEnd = destionationPoint;
		}
		destionationPoint.localCam = localCam;
	}
}
