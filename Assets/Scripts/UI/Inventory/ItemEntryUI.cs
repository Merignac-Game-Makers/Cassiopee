using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static InventoryManager;
using TMPro;

public class ItemEntryUI : EntryUI 
{
	public Image iconeImage;

	ItemEntryUI[] all;

	private void Start() {
		inventoryUI = InventoryUI.Instance;
	}

	public override void Init(Entry entry) {
		this.entry = entry;
		entry.ui = this;
		iconeImage.sprite = (entry as InventoryEntry).item.ItemSprite;
		lowerText.text = "";
		label.text = (entry as InventoryEntry).item.ItemName;
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
		base.Toggle();
		all = inventoryUI.GetComponentsInChildren<ItemEntryUI>();
		foreach(ItemEntryUI entry in all) {
			if (entry!=this)
				entry.Select(false);
		}
	}

}
