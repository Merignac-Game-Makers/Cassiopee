using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static InventoryManager;

public class ItemEntryUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
	IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Image iconeImage;
	public Text itemCount;
	public InventoryEntry inventoryEntry;

	//public EquipmentItem equipmentItem { get; private set; }

	public InventoryUI inventoryUI { get; set; }
	public int Index { get; set; }

	public void Init(InventoryUI inventoryUI, InventoryEntry entry) {
		this.inventoryUI = inventoryUI;
		inventoryEntry = entry;
		iconeImage.sprite = entry.item.ItemSprite;
		itemCount.text = "";
	}

	/// <summary>
	/// double clic pour 'consommer' un objet
	/// </summary>
	/// <param name="eventData"></param>
	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.clickCount % 2 == 0) {
			if (inventoryEntry != null)
				InventoryUI.Instance.ObjectDoubleClicked(inventoryEntry);
		}
	}

	/// <summary>
	/// début de survol
	/// </summary>
	/// <param name="eventData"></param>
	public void OnPointerEnter(PointerEventData eventData) {
		inventoryUI.ObjectHoveredEnter(this);
	}

	/// <summary>
	/// fin de survol
	/// </summary>
	/// <param name="eventData"></param>
	public void OnPointerExit(PointerEventData eventData) {
		inventoryUI.ObjectHoverExited(this);
	}

	/// <summary>
	/// mise à jour
	/// </summary>
	public void UpdateEntry() {
		bool isEnabled = inventoryEntry != null && inventoryEntry.count > 0;
		//gameObject.SetActive(isEnabled);

		if (isEnabled) {
			iconeImage.sprite = inventoryEntry.item.ItemSprite;
			if (inventoryEntry.count > 1) {
				itemCount.gameObject.SetActive(true);
				itemCount.text = inventoryEntry.count.ToString();
			} else {
				itemCount.gameObject.SetActive(false);
			}
		} else {
			inventoryUI.RemoveEntry(this);
		}
	}

	//public void SetupEquipment(EquipmentItem itm) {
	//	equipmentItem = itm;
	//	enabled = itm != null;
	//	iconeImage.enabled = enabled;
	//	if (enabled)
	//		iconeImage.sprite = itm.ItemSprite;
	//}

	/// <summary>
	/// début de glisser-déposer
	/// </summary>
	/// <param name="eventData"></param>
	public void OnBeginDrag(PointerEventData eventData) {
		inventoryUI.currentlyDragged = new InventoryUI.DragData();                                  // créer un 'dragData'
		inventoryUI.currentlyDragged.DraggedEntry = this;                                           // qui contient cette entrée
		inventoryUI.currentlyDragged.OriginalParent = (RectTransform)transform.parent;              // dont on mémorise le parent actuel
		transform.SetParent(inventoryUI.DragCanvas.transform, true);                                // puis qu'on rattaceha au canvas 'DragCanvas'
	}

	/// <summary>
	/// pendant le glisser-déposer
	/// </summary>
	/// <param name="eventData"></param>
	public void OnDrag(PointerEventData eventData) {
		transform.localPosition = transform.localPosition + UnscaleEventDelta(eventData.delta);     // tenir compte de l'échelle du DragCanvas
	}

	/// <summary>
	/// tenir compte de l'échelle du DragCanvas
	/// </summary>
	/// <param name="vec"></param>
	/// <returns></returns>
	Vector3 UnscaleEventDelta(Vector3 vec) {
		Vector2 referenceResolution = inventoryUI.DragCanvasScaler.referenceResolution;
		Vector2 currentResolution = new Vector2(Screen.width, Screen.height);
		float heightRatio = currentResolution.y / referenceResolution.y;
		return vec / heightRatio;
	}

	/// <summary>
	/// fin de glisser-déposer
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData) {
		inventoryUI.HandledDroppedEntry(eventData.position);                            // gérer le 'drop'
		RectTransform t = transform as RectTransform;
		transform.SetParent(inventoryUI.currentlyDragged.OriginalParent, true);         // rattacher au parent original
		inventoryUI.currentlyDragged = null;                                            // supprimer le 'dragData'
		t.offsetMax = -Vector2.one * 4;
		t.offsetMin = Vector2.one * 4;
	}
}
