﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;
using static InventoryManager;
using static InteractableObject.Action;

/// <summary>
/// Describes an InteractableObject that can be picked up and grants a specific item when interacted with.
///
/// It will also play a small animation (object going in an arc from spawn point to a random point around) when the
/// object is actually "spawned", and the object becomes interactable only when that animation is finished.
///
/// Finally it will notify the LootUI that a new loot is available in the world so the UI displays the name.
/// </summary>
public class Loot : InteractableObject
{	

	static float AnimationTime = 0.1f;

	public Item item;
	public static InventoryUI inventoryUI;

	public override bool IsInteractable() {
		return !animate || m_AnimationTimer >= AnimationTime;
	}

	public bool animate => item.animate;

	Vector3 m_OriginalPosition;
	Vector3 m_TargetPoint;
	float m_AnimationTimer = 0.0f;


	void Awake() {
		m_OriginalPosition = transform.position;
		m_TargetPoint = transform.position;
		m_AnimationTimer = AnimationTime - 0.1f;
	}

	protected override void Start() {
		base.Start();
		inventoryUI = InventoryUI.Instance;
		//animate = item.animate;
	}


	void Update() {
		// animation de mise en place
		if (animate && m_AnimationTimer < AnimationTime) {
			m_AnimationTimer += Time.deltaTime;

			float ratio = Mathf.Clamp01(m_AnimationTimer / AnimationTime);

			Vector3 currentPos = Vector3.Lerp(m_OriginalPosition, m_TargetPoint, ratio);
			currentPos.y = currentPos.y + Mathf.Sin(ratio * Mathf.PI) * 2.0f;

			transform.position = currentPos;
		}
	}

	/// <summary>
	/// Ramasser / déposer un objet
	/// </summary>
	/// <param name="target"></param>
	public override void InteractWith(CharacterData character, HighlightableObject target = null, Action action = take) {
		base.InteractWith(character , target, action);
		// si c'est une intéraction avec le joueur : ramasser l'objet
		if (action == take) {
			SFXManager.PlaySound(SFXManager.Use.Sound2D, new SFXManager.PlayData() { Clip = SFXManager.PickupSound });

			InventoryManager.Instance.AddItem(item);

			inventoryUI.UpdateEntries(target);
			Destroy(gameObject);

		} else
		// si c'est un dépôt sur une cible => drop
		if (action == drop && target is Target) {
			if ((target as Target).isFree && !item.combinable) {                        // et que cet emplacement est libre et que l'objet n'est pas combinable
				inventoryUI.DropItem(target as Target, item.entry);                     // déposer l'objet d'inventaire
			}
		}
	}

	/// <summary>
	/// This is called when the loot become available. It will setup to play the small spawn animation.
	/// This is rarely called manually, and mostly called by the LootSpawner class.
	/// </summary>
	/// <param name="position"></param>
	//public void Spawn(Vector3 position) {
	//	m_OriginalPosition = position;
	//	transform.position = position;

	//	Vector3 targetPos;
	//	if (!RandomPoint(transform.position, 2.0f, out targetPos))
	//		targetPos = transform.position;

	//	m_TargetPoint = targetPos;
	//	m_AnimationTimer = 0.0f;

	//	gameObject.layer = LayerMask.NameToLayer("Interactable");
	//}

	//bool RandomPoint(Vector3 center, float range, out Vector3 result) {
	//	for (int i = 0; i < 30; i++) {
	//		Vector3 randomPoint = center + Random.insideUnitSphere * range;
	//		NavMeshHit hit;
	//		if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
	//			result = hit.position;
	//			return true;
	//		}
	//	}
	//	result = Vector3.zero;
	//	return false;
	//}

	//void CreateWorldRepresentation() {
	//	//if the item have a world object prefab set use that...
	//	if (Item.WorldObjectPrefab != null) {
	//		var obj = Instantiate(Item.WorldObjectPrefab, transform, false);
	//		obj.transform.localPosition = Vector3.zero;
	//		obj.layer = LayerMask.NameToLayer("Interactable");
	//	} else {//...otherwise, we create a billboard using the item sprite
	//		GameObject billboard = new GameObject("ItemBillboard");
	//		billboard.transform.SetParent(transform, false);
	//		billboard.transform.localPosition = Vector3.up * 0.3f;
	//		billboard.layer = LayerMask.NameToLayer("Interactable");

	//		var renderer = billboard.AddComponent<SpriteRenderer>();
	//		renderer.sharedMaterial = ResourceManager.Instance.BillboardMaterial;
	//		renderer.sprite = Item.ItemSprite;

	//		var rect = Item.ItemSprite.rect;
	//		float maxSize = rect.width > rect.height ? rect.width : rect.height;
	//		float scale = Item.ItemSprite.pixelsPerUnit / maxSize;

	//		billboard.transform.localScale = scale * Vector3.one * 0.5f;


	//		var bc = billboard.AddComponent<BoxCollider>();
	//		bc.size = new Vector3(0.5f, 0.5f, 0.5f) * (1.0f / scale);
	//	}
	//}
}
