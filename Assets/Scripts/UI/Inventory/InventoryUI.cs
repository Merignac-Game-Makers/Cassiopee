using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;


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

	public UIButton bookButton;
	public GameObject InvPanel;

	public RectTransform[] ItemSlots;

	public ItemEntryUI ItemEntryPrefab;
	public ItemTooltip Tooltip;

	public EquipmentUI EquipementUI;

	public Canvas DragCanvas;

	public static InventoryUI Instance;

	// Raycast
	readonly RaycastHit[] m_RaycastHitCache = new RaycastHit[16];
	int m_TargetLayer;


	public DragData CurrentlyDragged { get; set; }
	public CanvasScaler DragCanvasScaler { get; private set; }

	[HideInInspector]
	public ItemEntryUI[] m_ItemEntries;
	ItemEntryUI m_HoveredItem;
	HighlightableObject m_Item;

	//private void Awake() {
	//	Instance = this;
	//}

	public override void Init() {
		Instance = this;
		gameObject.SetActive(true);
		InvPanel.SetActive(false);

		CurrentlyDragged = null;

		DragCanvasScaler = DragCanvas.GetComponentInParent<CanvasScaler>();

		m_ItemEntries = new ItemEntryUI[ItemSlots.Length];

		for (int i = 0; i < m_ItemEntries.Length; ++i) {
			m_ItemEntries[i] = Instantiate(ItemEntryPrefab, ItemSlots[i]);
			m_ItemEntries[i].gameObject.SetActive(false);
			m_ItemEntries[i].Owner = this;
			m_ItemEntries[i].InventoryEntry = i;
		}

		m_TargetLayer = 1 << LayerMask.NameToLayer("Interactable");


	}

	void OnEnable() {
		m_HoveredItem = null;
		Tooltip.gameObject.SetActive(false);
	}

	private void Update() {
		//Keyboard shortcut
		if (Input.GetKeyUp(KeyCode.I))
			Toggle();
	}

	public override void Toggle() {
		InvPanel.SetActive(!InvPanel.activeInHierarchy);
		bookButton.gameObject.SetActive(!InvPanel.activeInHierarchy);
	}

	public void Load(HighlightableObject item) {
		m_Item = item;
		//EquipementUI.UpdateEquipment(m_Data.Equipment, m_Data.Stats);

		for (int i = 0; i < m_ItemEntries.Length; ++i) {
			m_ItemEntries[i].UpdateEntry();
		}
	}

	public void ObjectDoubleClicked(InventorySystem.InventoryEntry usedItem) {
		//if(m_Data.Inventory.UseItem(usedItem))
		//    SFXManager.PlaySound(SFXManager.Use.Sound2D, new SFXManager.PlayData() {Clip = usedItem.Item is EquipmentItem ? SFXManager.ItemEquippedSound : SFXManager.ItemUsedSound} );
		InventorySystem.Instance.UseItem(usedItem);
		ObjectHoverExited(m_HoveredItem);
		Load(m_Item);
	}


	public void ObjectHoveredEnter(ItemEntryUI hovered) {
		m_HoveredItem = hovered;

		Tooltip.gameObject.SetActive(true);

		Item itemUsed = m_HoveredItem.InventoryEntry != -1 ? InventorySystem.Instance.Entries[m_HoveredItem.InventoryEntry].Item : m_HoveredItem.EquipmentItem;

		Tooltip.Name.text = itemUsed.ItemName;
		Tooltip.DescriptionText.text = itemUsed.GetDescription();
	}

	public void ObjectHoverExited(ItemEntryUI exited) {
		if (m_HoveredItem == exited) {
			m_HoveredItem = null;
			Tooltip.gameObject.SetActive(false);
		}
	}

	public void HandledDroppedEntry(Vector3 position) {
		// check for drop on ItemSlots
		for (int i = 0; i < ItemSlots.Length; ++i) {
			RectTransform t = ItemSlots[i];

			if (RectTransformUtility.RectangleContainsScreenPoint(t, position)) {
				if (m_ItemEntries[i] != CurrentlyDragged.DraggedEntry) {
					var prevItem = InventorySystem.Instance.Entries[CurrentlyDragged.DraggedEntry.InventoryEntry];
					InventorySystem.Instance.Entries[CurrentlyDragged.DraggedEntry.InventoryEntry] = InventorySystem.Instance.Entries[i];
					InventorySystem.Instance.Entries[i] = prevItem;

					CurrentlyDragged.DraggedEntry.UpdateEntry();
					m_ItemEntries[i].UpdateEntry();
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
						Debug.Log("Drop Item");
						DropItem(data, PlayerControl.Instance.m_CurrentlyDragged);
						break;
					}
				}
			}
		}
	}

	private void DropItem(Target target, InventoryUI.DragData dragData) {
		var EntryIndex = dragData.DraggedEntry.InventoryEntry;
		CreateWorldRepresentation(InventorySystem.Instance.Entries[EntryIndex].Item, target);
		InventorySystem.Instance.RemoveItem(EntryIndex);
	}

	void CreateWorldRepresentation(Item item, Target target) {
		var pos = target.gameObject.transform.position + Vector3.up * item.WorldObjectPrefab.gameObject.transform.localScale.y / 2;
		//if the item have a world object prefab set use that...
		if (item.WorldObjectPrefab != null) {
			var obj = Instantiate(item.WorldObjectPrefab, pos, new Quaternion(), target.transform);
			obj.transform.localScale = new Vector3(
				obj.transform.localScale.x / target.transform.localScale.x,
				obj.transform.localScale.y / target.transform.localScale.y,
				obj.transform.localScale.z / target.transform.localScale.z
				);
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
