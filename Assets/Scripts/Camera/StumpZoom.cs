using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StumpZoom : ZoomBase, LootZoom
{
	public List<Loot> loots;

	public override bool AllDone() {
        return loots.Count == 0;
    }

	public virtual void LootTaken(Loot loot) {
		foreach (Loot l in loots) {
			if (l == loot) {
				loots.Remove(l);
				TestExit();
				break;
			}
		}
	}

}
