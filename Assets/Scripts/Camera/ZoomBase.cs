using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZoomBase : MonoBehaviour
{
	public ZoomUI zoomUI { get; private set; }
	public List<Loot> loots;
	public SwapCamera swapCamera { get; private set; }

	void Start() {
		zoomUI = UIManager.Instance.GetComponentInChildren<ZoomUI>(true);
		swapCamera = GetComponentInChildren<SwapCamera>();
	}

	public virtual bool TestExit() {
		if (AllDone()) {									// si les conditions de sortie sont réunies
			swapCamera.Exit();								// Sortir
			swapCamera.gameObject.SetActive(false);			// désactiver le zoom
			return true;
		}
		return false;
	}

	public abstract bool AllDone();

	public virtual void LootTaken(Loot loot) {
		foreach(Loot l in loots) {
			if (l == loot) { 
				loots.Remove(l);
				TestExit();
				break;
			}
		}
	}
}
