using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ItemEntryUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
	IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Image iconeImage;
	public Text itemCount;
	public int count { get; private set; } = 0;

	public int inventoryEntry { get; set; } = -1;
	public EquipmentItem equipmentItem { get; private set; }

	public InventoryUI owner { get; set; }
	public int Index { get; set; }

	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.clickCount % 2 == 0) {
			if (InventorySystem.Instance.Entries[inventoryEntry] != null)
				InventoryUI.Instance.ObjectDoubleClicked(InventorySystem.Instance.Entries[inventoryEntry]);

		}
	}


	public void OnPointerEnter(PointerEventData eventData) {
		owner.ObjectHoveredEnter(this);
	}

	public void OnPointerExit(PointerEventData eventData) {
		owner.ObjectHoverExited(this);
	}

	public void UpdateEntry() {
		var entry = InventorySystem.Instance.Entries[inventoryEntry];
		bool isEnabled = entry != null;

		gameObject.SetActive(isEnabled);

		if (isEnabled) {
			iconeImage.sprite = entry.item.ItemSprite;
			count = entry.count;

			if (entry.count > 1) {
				itemCount.gameObject.SetActive(true);
				itemCount.text = entry.count.ToString();
			} else {
				itemCount.gameObject.SetActive(false);
			}
		}
	}

	public void SetupEquipment(EquipmentItem itm) {
		equipmentItem = itm;

		enabled = itm != null;
		iconeImage.enabled = enabled;
		if (enabled)
			iconeImage.sprite = itm.ItemSprite;
	}

	public void OnBeginDrag(PointerEventData eventData) {
		if (equipmentItem != null)
			return;

		owner.CurrentlyDragged = new InventoryUI.DragData();
		owner.CurrentlyDragged.DraggedEntry = this;
		owner.CurrentlyDragged.OriginalParent = (RectTransform)transform.parent;

		transform.SetParent(owner.DragCanvas.transform, true);
	}

	public void OnDrag(PointerEventData eventData) {
		if (equipmentItem != null)
			return;

		transform.localPosition = transform.localPosition + UnscaleEventDelta(eventData.delta);
	}


	Vector3 UnscaleEventDelta(Vector3 vec) {
		Vector2 referenceResolution = owner.DragCanvasScaler.referenceResolution;
		Vector2 currentResolution = new Vector2(Screen.width, Screen.height);

		//float widthRatio = currentResolution.x / referenceResolution.x;
		float heightRatio = currentResolution.y / referenceResolution.y;
		//float ratio = Mathf.Lerp(widthRatio, heightRatio,  Owner.DragCanvasScaler.matchWidthOrHeight);

		return vec / heightRatio;

		//return new Vector3(vec.x / heightRatio, vec.y / heightRatio, vec.z);
	}

	public void OnEndDrag(PointerEventData eventData) {
		if (equipmentItem != null)
			return;

		owner.HandledDroppedEntry(eventData.position);

		RectTransform t = transform as RectTransform;

		transform.SetParent(owner.CurrentlyDragged.OriginalParent, true);
		owner.CurrentlyDragged = null;

		t.offsetMax = -Vector2.one * 4;
		t.offsetMin = Vector2.one * 4;
	}
}
