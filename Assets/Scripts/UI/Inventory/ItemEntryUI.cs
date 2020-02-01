using UnityEngine;
using UnityEngine.UI;

public class ItemEntryUI : EntryUI
{
	public Image iconeImage;
	public Image plus;

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
		plus.enabled = item.combinable;
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
		var combineItem = inventoryUI.combineUI.item;
		if (combineItem != null && item == combineItem.combineWith) {              // si une entrée combinable avec l'item est actuellement sélectionnée
			// Debug.Log("Combine");
			var combineEntry = inventoryUI.combineUI.entry;
			combineEntry.item = combineItem.obtain;
			combineEntry.count = 1;
			combineEntry.ui.Init(combineEntry);
			inventoryUI.combineUI.SetObject(combineEntry);
			inventoryUI.RemoveEntry(this);
		} else {                                                                    // sinon
			all = inventoryUI.GetComponentsInChildren<ItemEntryUI>();
			foreach (ItemEntryUI entry in all) {                                    // désélectionner toutes les autres entrées de l'inventaire
				if (entry != this && entry.selected)
					entry.Select(false);
			}
			base.Toggle();                                                          // sélectionner/déselectionner cette entrée
			if (selected && item.combinable) {                                      // si on sélectionne et que l'item est combinable
				inventoryUI.combineUI.SetObject(entry as InventoryEntry);			//		afficher le panneau 'combine'
			} else {                                                                // sinon
				inventoryUI.combineUI.Clear();										//		masquer le panneau combine
			}
		}
	}
}
