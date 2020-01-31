using UnityEngine;
using UnityEngine.UI;
using static InventoryManager;

public class ItemEntryUI : EntryUI 
{
	public Image iconeImage;
	public GameObject combine;

	ItemEntryUI[] all;
	public Item item;

	private void Start() {
		inventoryUI = InventoryUI.Instance;
	}

	public override void Init(Entry entry) {
		this.entry = entry;
		entry.ui = this;
		item = (entry as InventoryEntry).item;
		iconeImage.sprite = item.ItemSprite;
		lowerText.text = "";
		label.text = item.ItemName;
		combine.SetActive(item.combinable);
	}


	/// <summary>
	/// mise à jour
	/// </summary>
	public override void UpdateEntry() {
		bool isEnabled = entry != null && (entry as InventoryEntry)?.count > 0;

		if (isEnabled) {
			iconeImage.sprite = (entry as InventoryEntry)?.item.ItemSprite;
			if ((entry as InventoryEntry)?.count > 1) {
				lowerText.gameObject.SetActive(true);
				lowerText.text = (entry as InventoryEntry)?.count.ToString();
			} else {
				lowerText.gameObject.SetActive(false);
			}
		} else {
			inventoryUI.RemoveEntry(this);
		}
	}

	public override void Toggle() {
		all = inventoryUI.GetComponentsInChildren<ItemEntryUI>();
		foreach(ItemEntryUI entry in all) {
			if (entry!=this && entry.selected)
				entry.Select(false);
		}
		base.Toggle();
		if (selected && item.combinable) {
			inventoryUI.combinePanel.GetComponent<CombineUI>().SetObject(item);
			inventoryUI.combinePanel.SetActive(true);
		} else {
			inventoryUI.combinePanel.SetActive(false);
		}
	}

}
