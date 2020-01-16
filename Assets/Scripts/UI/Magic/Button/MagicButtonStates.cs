using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine.UI;

public class MagicButtonStates : MonoBehaviour
{

    SimpleScrollSnap bookButtonSelector;

	public enum State { inactive, active, open }

	public Animator halo;
	public State state { get; private set; }
	private MagicUI magicUI;
	private InventoryUI inventoryUI;

	// Start is called before the first frame update
	void Start()
    {
 		magicUI = MagicUI.Instance;
		inventoryUI = InventoryUI.Instance;
		bookButtonSelector = GetComponentInChildren<SimpleScrollSnap>();

		// au démarrage du jeu
		state = State.inactive;
	}

	/// <summary>
	/// bascule affichage plein écran
	/// </summary>
	public void ToggleFullScreen() {
		if (bookButtonSelector.targetPanel == 1) {
			//if (halo.GetCurrentAnimatorStateInfo(0).IsName("Empty") || halo.GetCurrentAnimatorStateInfo(0).IsName("colorOFF"))
			if (state == State.inactive)
				halo.SetTrigger("startColorON");
			state = State.active;
		} else if (bookButtonSelector.targetPanel == 2) {
			state = State.open;
		} else {
			//if (halo.GetCurrentAnimatorStateInfo(0).IsName("colorON"))
			if (state == State.active)
				halo.SetTrigger("startColorOFF");
			state = State.inactive;
		}
		magicUI.SetState(state);

		//bookPanel.SetActive(!bookPanel.activeInHierarchy);
		if (magicUI.isOn && inventoryUI.isOn) {
			inventoryUI.Toggle();
		}
	}

	public void Close() {
		bookButtonSelector.GoToPanel(1);
	}

}
