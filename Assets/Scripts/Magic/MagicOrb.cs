﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicOrb : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public enum OrbType { Moon, Sun}

	public OrbType orbType;

	private MagicTarget mTarget;
	private MagicController magicController;
	private int m_Layer;
	private RaycastHit[] m_RaycastHitCache = new RaycastHit[4];

	// Start is called before the first frame update
	void Start() {
		magicController = MagicController.Instance;
		m_Layer = ~(1 << LayerMask.NameToLayer("Magic"));
	}


	public void OnBeginDrag(PointerEventData eventData) {
		magicController.dragging = this;
		transform.SetParent(null, true);
	}

	public void OnDrag(PointerEventData eventData) {
		Ray screenRay = CameraController.Instance.GameplayCamera.ScreenPointToRay(Input.mousePosition);
		int count = Physics.SphereCastNonAlloc(screenRay, .2f, m_RaycastHitCache, 1000.0f);
		if (count > 0) {
			GameObject obj = m_RaycastHitCache[0].collider.gameObject;
			var rch = RayCast();
			if (rch.collider != null) {
				transform.localPosition = rch.point + Vector3.up;
				Debug.Log(rch.collider?.gameObject);
				Debug.Log(transform.localPosition);
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData) {
		magicController.dragging = null;
		if (mTarget!=null && mTarget.IsFree()) {
			mTarget.MakeMagicalStuff(this);
		}
	}

	RaycastHit RayCast() {
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		int count = Physics.SphereCastNonAlloc(ray, .2f, m_RaycastHitCache, 1000.0f, m_Layer);
		if (count > 0) {
			foreach(RaycastHit r in m_RaycastHitCache) {
				if (r.collider!=null && r.collider.gameObject.GetComponentInChildren<MagicTarget>() != null) {
					mTarget = r.collider.gameObject.GetComponentInChildren<MagicTarget>();
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
