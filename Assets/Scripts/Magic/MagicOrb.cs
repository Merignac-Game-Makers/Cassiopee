using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicOrb : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	/// <summary>
	/// Types d'orbe (LUNE / SOLEIL)
	/// </summary>
	public enum OrbType { Moon, Sun }

	public OrbType orbType;                     // type d'orbe
	[HideInInspector]
	public string constellation;                // nom de la constellation grâce à laquelle a été obtenu cet orbe

	private MagicEffectBase mTarget;            // cible sur laquelle est déposé l'orbe
	private MagicManager magicManager;          // le gestionnaire de magie
	//private int m_Layer;                        // layer contenant les orbes (pour sélection et drag & drop)
	private RaycastHit[] m_RaycastHitCache = new RaycastHit[4]; // pour sélection des orbes

	private PlayerManager player;               // le joueur pour pouvoir reprendre l'orbe

	int raycastableLayers;                      // tous les layers à tester pour le Raycast



	void Start() {
		magicManager = MagicManager.Instance;               // créer l'instance statique
		//m_Layer = ~(1 << LayerMask.NameToLayer("Magic"));   // créer le masque de layer (tout sauf 'Magic')

		var postProcessingMask = 1 << LayerMask.NameToLayer("PostProcess");
		var ignoreRaycastMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
		var magicLayerMask = 1 << LayerMask.NameToLayer("Magic");
		raycastableLayers = ~(postProcessingMask | ignoreRaycastMask | magicLayerMask);
	}

	/// <summary>
	/// début de drag & drop
	/// </summary>
	/// <param name="eventData"></param>
	public void OnBeginDrag(PointerEventData eventData) {
		magicManager.dragging = this;       // liaison avec le contrôleur de magie
		transform.SetParent(null, true);    // détacher du player
	}

	/// <summary>
	/// détecter un glissement de pointeur sur l'orbe
	/// </summary>
	/// <param name="eventData"></param>
	public void OnDrag(PointerEventData eventData) {
		Ray screenRay = CameraController.Instance.GameplayCamera.ScreenPointToRay(Input.mousePosition);			// lancer de rayon
		int count = Physics.SphereCastNonAlloc(screenRay, .2f, m_RaycastHitCache, 1000.0f, raycastableLayers);	// combien d'objets dans le layer 'Magic' ?
		if (count > 0) {
			var rch = RayCast();                                        // trouver l'objet sous le pointeur de souris
			if (rch.collider != null) {                                 // s'il a un collider
				transform.localPosition = rch.point + Vector3.up;       // déplacer l'orbe au dessus de ce point
			}
		}
	}

	/// <summary>
	/// fin du Drag & Drop
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData) {
		magicManager.dragging = null;               // informer le contrôleur de magie
		if (mTarget != null && mTarget.isFree) {        // si on lâche l'orbe sur une cible de magie
			mTarget.MakeMagicalStuff(this);         // déclencher la magie
		}
		if (player != null) {                       // si on reprend l'orbe
			transform.SetParent(player.transform, true);
			transform.localPosition = new Vector3(0, 2, 0);
		}
	}

	RaycastHit RayCast() {
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		int count = Physics.SphereCastNonAlloc(ray, .2f, m_RaycastHitCache, 1000.0f, raycastableLayers);
		mTarget = null;
		player = null;
		if (count > 0) {
			for (int i = 0; i < count; i++) {
				var r = m_RaycastHitCache[i];
				// recherche d'une cible de magie (pour appliquer la magie)
				if (r.collider != null && r.collider.gameObject.GetComponentInChildren<MagicEffectBase>() != null) {
					mTarget = r.collider.gameObject.GetComponentInChildren<MagicEffectBase>();
					break;
				}
				// recherche du joueur (pour reprendre l'orbe)
				if (r.collider != null && r.collider.gameObject.GetComponentInChildren<PlayerManager>() != null) {
					player = r.collider.gameObject.GetComponentInChildren<PlayerManager>();
					break;
				}

			}
			return m_RaycastHitCache[0];
		}
		return new RaycastHit();
	}
}
