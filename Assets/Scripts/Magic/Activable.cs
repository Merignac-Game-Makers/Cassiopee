using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

/// <summary>
/// Describes an InteractableObject that can be picked up and grants a specific item when interacted with.
///
/// It will also play a small animation (object going in an arc from spawn point to a random point around) when the
/// object is actually "spawned", and the object becomes interactable only when that animation is finished.
///
/// Finally it will notify the LootUI that a new loot is available in the world so the UI displays the name.
/// </summary>
public class Activable : InteractableObject
{

	public override bool IsInteractable() {
		return MagicManager.Instance.isOn && MagicManager.Instance.currentOrb == null;
	}

	Vector3 m_TargetPoint;

	public bool IsActive => m_IsActive;
	[HideInInspector]
	public bool m_IsActive;

	public Color activeColor = Color.magenta;
	public Color inactiveColor = Color.white;

	void Awake() {
		m_TargetPoint = transform.position;
		m_IsActive = false;
	}

	void Update() {
		Debug.DrawLine(m_TargetPoint, m_TargetPoint + new Vector3(0, 2, 0), Color.magenta);
	}

	public override void InteractWith(HighlightableObject target) {

	}

	//public void OnPointerDownDelegate(PointerEventData data) {
	//	Debug.Log("OnPointerDownDelegate called.");
	//}

	/// <summary>
	/// Ajouter/ retirer d'une constellation en cours :
	///		- ajouter => allumé
	///		- retirer => éteint
	/// </summary>
	public void Toggle() {
		m_IsActive = !m_IsActive;
		SetColor(IsActive ? activeColor : inactiveColor) ;
		MagicManager.Instance.AddOrRemove(this);
	}

	///// <summary>
	///// Forcer un statut
	///// </summary>
	///// <param name="on">allumé ou éteint</param>
	//public void Set(bool on) {
	//	if (on) {
	//		m_IsActive = true;                          // visuel
	//		Highlight(true);                            // 'sélectionné'
	//	} else {
	//		m_IsActive = false;                         // visuel
	//		Highlight(false);                           // 'désélectionné'
	//	}
	//}

	/// <summary>
	/// true  : allumer
	/// false : éteindre
	/// </summary>
	public override void Highlight(bool on) {
		base.Highlight(on);
		SetColor(IsActive ? activeColor : inactiveColor);
	}


}
