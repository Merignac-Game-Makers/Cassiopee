using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static InventoryManager;
using TMPro;


/// <summary>
/// classe pour afficher un orbe dans la case en bas à droite de l'écran
/// entry : classe OrbEntry qui contient les caractéristiques de l'orbe (médaillon, constellation, ...)
/// 1 étiquette pour la constellation
/// 1 étiquette pour le médaillon utilisé
/// </summary>
public class OrbEntryUI : EntryUI
{
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
