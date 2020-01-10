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

	public override bool IsInteractable => UIManager.Instance.artifactButton.gameObject.activeInHierarchy;

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

	protected override void Start() {
		base.Start();
		//EventTrigger trigger = GetComponent<EventTrigger>();
		//EventTrigger.Entry entry = new EventTrigger.Entry();
		//entry.eventID = EventTriggerType.PointerClick;
		//entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
		//trigger.triggers.Add(entry);
	}

	void Update() {
		Debug.DrawLine(m_TargetPoint, m_TargetPoint + new Vector3(0, 2, 0), Color.magenta);
	}

	public override void InteractWith(HighlightableObject target) {

	}

	public void OnPointerDownDelegate(PointerEventData data) {
		Debug.Log("OnPointerDownDelegate called.");
	}

	public void Toggle() {
		m_IsActive = !m_IsActive;
		SetColor(IsActive ? activeColor : inactiveColor) ;
		MagicManager.Instance.AddOrRemove(this);
	}

	/// <summary>
	/// true  : allumer le projecteur
	/// false : éteindre le projecteur
	/// </summary>
	public override void Highlight(bool on) {
		base.Highlight(on);
		SetColor(IsActive ? activeColor : inactiveColor);
	}


}
