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
	public enum OrbType { Moon, Sun}

	public OrbType orbType;                     // type d'orbe
	[HideInInspector]
	public string constellation;				// nom de la constellation grâce à laquelle a été obtenu cet orbe

	private MagicEffectBase mTarget;				// cible sur laquelle est déposé l'orbe
	private MagicManager magicController;	// le gestionnaire de magie
	private int m_Layer;						// layer contenant les orbes (pour sélection et drag & drop)
	private RaycastHit[] m_RaycastHitCache = new RaycastHit[4]; // pour sélection des orbes

	void Start() {
		magicController = MagicManager.Instance;			// créer l'instance statique
		m_Layer = ~(1 << LayerMask.NameToLayer("Magic"));	// créer le masque de layer
	}

	/// <summary>
	/// début de drag & drop
	/// </summary>
	/// <param name="eventData"></param>
	public void OnBeginDrag(PointerEventData eventData) {
		magicController.dragging = this;	// liaison avec le contrôleur de magie
		transform.SetParent(null, true);	// détacher du player
	}

	/// <summary>
	/// détecter un glissement de pointeur sur l'orbe
	/// </summary>
	/// <param name="eventData"></param>
	public void OnDrag(PointerEventData eventData) {
		Ray screenRay = CameraController.Instance.GameplayCamera.ScreenPointToRay(Input.mousePosition);		// lancer de rayon
		int count = Physics.SphereCastNonAlloc(screenRay, .2f, m_RaycastHitCache, 1000.0f);					// combien d'objets dans le layer 'Magic' ?
		if (count > 0) {
			var rch = RayCast();										// trouver l'objet sous le pointeur de souris
			if (rch.collider != null) {									// s'il a un collider
				transform.localPosition = rch.point + Vector3.up;		// déplacer l'orbe au dessus de ce point
			}
		}
	}

	/// <summary>
	/// fin du Drag & Drop
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData) {
		magicController.dragging = null;			// informer le contrôleur de magie
		if (mTarget!=null && mTarget.isFree) {		// si on lâche l'orbe sur une cible de magie
			mTarget.MakeMagicalStuff(this);			// déclencher la magie
		}
	}

	RaycastHit RayCast() {
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		int count = Physics.SphereCastNonAlloc(ray, .2f, m_RaycastHitCache, 1000.0f, m_Layer);
		if (count > 0) {
			foreach(RaycastHit r in m_RaycastHitCache) {
				if (r.collider!=null && r.collider.gameObject.GetComponentInChildren<MagicEffectBase>() != null) {
					mTarget = r.collider.gameObject.GetComponentInChildren<MagicEffectBase>();
					break;
				} else {
					mTarget = null;
				}
			}
			return m_RaycastHitCache[0];
		}
		return new RaycastHit();
	}
}
