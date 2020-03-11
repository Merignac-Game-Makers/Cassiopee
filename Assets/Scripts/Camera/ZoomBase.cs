using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface LootZoom
{
	void LootTaken(Loot loot);
}

public abstract class ZoomBase : SwapCamera
{

	public virtual bool TestExit() {
		if (AllDone()) {									// si les conditions de sortie sont réunies
			Exit();												// Sortir
			//enabled = false;                                    // désactiver le zoom
			GetComponent<Collider>().enabled = false;
			return true;
		}
		return false;
	}

	public abstract bool AllDone();
}
