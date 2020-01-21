using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static InventorySystem;

/// <summary>
/// Handle all the UI code related to the inventory (drag'n'drop of object, using objects, equipping object etc.)
/// </summary>
public class InventoryUI : UIBase
{
	public class DragData
	{
		public ItemEntryUI DraggedEntry;
		public RectTransform OriginalParent;
	}

	public GameObject bookPanel;
	public GameObject content;

	float size;


	public ItemEntryUI itemEntryPrefab;
	public RectTransform slotPrefab;
	//public ItemTooltip Tooltip;
	//public EquipmentUI EquipementUI;

	public Canvas DragCanvas;

	public static InventoryUI Instance;

	// Raycast
	readonly RaycastHit[] m_RaycastHitCache = new RaycastHit[16];
	int m_TargetLayer;


	public DragData CurrentlyDragged { get; set; }
	public CanvasScaler DragCanvasScaler { get; private set; }

	public List<ItemEntryUI> itemEntries { get; private set; } = new List<ItemEntryUI>();
	public List<RectTransform> slots { get; private set; } = new List<RectTransform>();

	ItemEntryUI hoveredItem;
	HighlightableObject item;
	UIManager uiManager;


	public override void Init(UIManager uiManager) {
		Instance = this;
		this.uiManager = uiManager;

		gameObject.SetActive(true);

		CurrentlyDragged = null;

		DragCanvasScaler = DragCanvas.GetComponentInParent<CanvasScaler>();

		//for (int i = 0; i < itemEntries.Count; ++i) {
		//	itemEntries[i] = Instantiate(ItemEntryPrefab, ItemSlots[i]);
		//	itemEntries[i].gameObject.SetActive(false);
		//	itemEntries[i].Owner = this;
		//	itemEntries[i].InventoryEntry = i;
		//}

		m_TargetLayer = 1 << LayerMask.NameToLayer("Interactable");

		size = content.GetComponent<RectTransform>().rect.height;       // slot
		var rect = GetComponent<RectTransform>().rect;                  // carré
		rect.height = size;                                             //
	}

	void OnEnable() {
		hoveredItem = null;
		//Tooltip.gameObject.SetActive(false);
	}

	public ItemEntryUI AddItemEntry(int idx) {
		RectTransform slot = Instantiate(slotPrefab, content.transform);        // créer un nouvel emplacement
		slots.Add(slot);
		ItemEntryUI itemEntry = Instantiate(itemEntryPrefab, slot);             // créer une nouvelle entrée d'inventaire dans cet emplacement
		itemEntry.gameObject.SetActive(false);
		itemEntry.owner = this;
		itemEntry.inventoryEntry = itemEntries.Count;
		itemEntries.Add(itemEntry);
		return itemEntry;
	}

	public void RemoveItemEntry(int entryIndex) {
		for (int i = 0; i < slots.Count; i++) {
			ItemEntryUI itemEntry = slots[i].GetComponentInChildren<ItemEntryUI>();
			if (!itemEntry) {
				Destroy(slots[i].gameObject);
				slots.RemoveAt(i);
				itemEntries.RemoveAt(i);
				break;
			} else {
				if (itemEntry.inventoryEntry == entryIndex) {
					Destroy(slots[i].gameObject);
					slots.RemoveAt(i);
					itemEntries.RemoveAt(i);
					break;
				}
			}
		}
	}


	/// <summary>
	/// bascule d'affichage
	/// </summary>
	public override void Toggle() {
		panel.SetActive(!isOn);         // monter /cacher le panneau d'inventaire
		uiManager.ManageButtons();      // adapter l'affichage des autres boutons
		if (!isOn) {
			uiManager.questsUI.SetOff();
		}
	}

	/// <summary>
	/// Actualiser l'affichage de toutes les entrés d'iventaire
	/// </summary>
	/// <param name="item"></param>
	public void UpdateEntries(HighlightableObject item) {
		this.item = item;
		var entries = content.GetComponentsInChildren<ItemEntryUI>();
		for (int i = entries.Length - 1; i > 0; i--) {
			if (entries[i].count <= 0)
				Destroy(entries[i].gameObject);
		}

		for (int i = 0; i < itemEntries.Count; ++i) {
			itemEntries[i].UpdateEntry();
		}
	}

	/// <summary>
	/// utiliser un objet (ex: boire une potion...)
	/// (inutilisé pour l'instant)
	/// </summary>
	/// <param name="usedItem"></param>
	public void ObjectDoubleClicked(InventorySystem.InventoryEntry usedItem) {
		//if(m_Data.Inventory.UseItem(usedItem))
		//    SFXManager.PlaySound(SFXManager.Use.Sound2D, new SFXManager.PlayData() {Clip = usedItem.Item is EquipmentItem ? SFXManager.ItemEquippedSound : SFXManager.ItemUsedSound} );
		InventorySystem.Instance.UseItem(usedItem);
		ObjectHoverExited(hoveredItem);
		UpdateEntries(item);
	}


	public void ObjectHoveredEnter(ItemEntryUI hovered) {
		hoveredItem = hovered;

		//Tooltip.gameObject.SetActive(true);

		Item itemUsed = hoveredItem.inventoryEntry != -1 ? InventorySystem.Instance.Entries[hoveredItem.inventoryEntry].item : hoveredItem.equipmentItem;

		//Tooltip.Name.text = itemUsed.ItemName;
		//Tooltip.DescriptionText.text = itemUsed.GetDescription();
	}

	public void ObjectHoverExited(ItemEntryUI exited) {
		if (hoveredItem == exited) {
			hoveredItem = null;
			//Tooltip.gameObject.SetActive(false);
		}
	}

	public void HandledDroppedEntry(Vector3 position) {
		// check for drop on ItemSlots
		for (int i = 0; i < itemEntries.Count; ++i) {
			if (RectTransformUtility.RectangleContainsScreenPoint(slots[i], position)) {
				if (itemEntries[i] != CurrentlyDragged.DraggedEntry) {
					var prevItem = InventorySystem.Instance.Entries[CurrentlyDragged.DraggedEntry.inventoryEntry];
					InventorySystem.Instance.Entries[CurrentlyDragged.DraggedEntry.inventoryEntry] = InventorySystem.Instance.Entries[i];
					InventorySystem.Instance.Entries[i] = prevItem;

					CurrentlyDragged.DraggedEntry.UpdateEntry();
					itemEntries[i].UpdateEntry();
					return;
				}
			}
		}
		// check for drop on 3D target
		Ray screenRay = CameraController.Instance.GameplayCamera.ScreenPointToRay(Input.mousePosition);
		int count = Physics.SphereCastNonAlloc(screenRay, 1.0f, m_RaycastHitCache, 1000.0f, m_TargetLayer);
		if (count > 0) {
			foreach (RaycastHit rh in m_RaycastHitCache) {
				if (rh.collider != null) {
					Target data = rh.collider.GetComponentInParent<Target>();
					if (data != null && data.isFree) {
						//Debug.Log("Drop Item");
						DropItem(data, PlayerManager.Instance.m_InvItemDragging);
						break;
					}
				}
			}
		}
	}

	private void DropItem(Target target, InventoryUI.DragData dragData) {
		var EntryIndex = dragData.DraggedEntry.inventoryEntry;
		CreateWorldRepresentation(InventorySystem.Instance.Entries[EntryIndex].item, target);
		//target.DoQuests();
		InventorySystem.Instance.RemoveItem(EntryIndex);
	}

	void CreateWorldRepresentation(Item item, Target target) {
		var pos = target.gameObject.transform.position + Vector3.up * item.WorldObjectPrefab.gameObject.transform.localScale.y / 2;
		// if the item have a world object prefab set use that...
		if (item.WorldObjectPrefab != null) {
			//var obj = Instantiate(item.WorldObjectPrefab, pos, new Quaternion(), target.transform);
			//obj.transform.localScale = new Vector3(
			//	obj.transform.localScale.x / target.transform.localScale.x,
			//	obj.transform.localScale.y / target.transform.localScale.y,
			//	obj.transform.localScale.z / target.transform.localScale.z
			//	); 
			var obj = Instantiate(item.WorldObjectPrefab, pos, new Quaternion());
			obj.transform.parent = target.gameObject.transform;
			obj.layer = LayerMask.NameToLayer("Interactable");
		}
		//else {//...otherwise, we create a billboard using the item sprite
		//	GameObject billboard = new GameObject("ItemBillboard");
		//	billboard.transform.SetParent(transform, false);
		//	billboard.transform.localPosition = Vector3.up * 0.3f;
		//	billboard.layer = LayerMask.NameToLayer("Interactable");

		//	var renderer = billboard.AddComponent<SpriteRenderer>();
		//	renderer.sharedMaterial = ResourceManager.Instance.BillboardMaterial;
		//	renderer.sprite = item.ItemSprite;

		//	var rect = item.ItemSprite.rect;
		//	float maxSize = rect.width > rect.height ? rect.width : rect.height;
		//	float scale = item.ItemSprite.pixelsPerUnit / maxSize;

		//	billboard.transform.localScale = scale * Vector3.one * 0.5f;


		//	var bc = billboard.AddComponent<BoxCollider>();
		//	bc.size = new Vector3(0.5f, 0.5f, 0.5f) * (1.0f / scale);
		//}
	}

}
