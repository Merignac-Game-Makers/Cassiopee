using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static InventoryManager;
using TMPro;

public class OrbEntryUI : EntryUI
{
	private void Start() {
		inventoryUI = InventoryUI.Instance;
	}

	public override void Init(Entry orbEntry) {
		entry = orbEntry;
		entry.ui = this;
		lowerText.text = (orbEntry as OrbEntry).orb.constellation;
		label.text = (orbEntry as OrbEntry).orb.orbType.ToString();
	}


	/// <summary>
	/// mise à jour
	/// </summary>
	public override void UpdateEntry() {
		bool isEnabled = entry != null;
		if (isEnabled) {
		} else {
		}
	}
}
