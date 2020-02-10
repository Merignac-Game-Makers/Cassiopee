using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Masquer les aides visuelles des transits
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
	}
}
