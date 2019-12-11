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
public class Target : InteractableObject
{
	public override bool IsInteractable => true;

	public bool isFree => IsFree();

	Vector3 m_TargetPoint;

	void Awake() {
		m_TargetPoint = transform.position;
	}

	protected override void Start() {
		base.Start();
	}


	void Update() {
		Debug.DrawLine(m_TargetPoint, m_TargetPoint + new Vector3(0, 2, 0), Color.magenta);
	}

	public override void InteractWith(HighlightableObject target) {

	}

	private bool IsFree() {
		return !GetComponentInChildren<Loot>();
	}

}
