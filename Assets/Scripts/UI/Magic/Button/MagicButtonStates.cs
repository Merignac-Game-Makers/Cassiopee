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
	public Book book;
	public GameObject questButton;

	public State state { get; private set; }


	void Start() {
		if (!bookButtonSelector) bookButtonSelector = GetComponentInChildren<SimpleScrollSnap>(true);
		state = State.inactive;				// au démarrage du jeu le grimoire est inactif
	}

	private void OnEnable() {
		questButton.SetActive(true);		// afficher également le bouton des quêtes
	}

	/// <summary>
	/// bascule affichage plein écran
	/// </summary>
	public void ToggleFullScreen() {
		if (!bookButtonSelector) bookButtonSelector = GetComponentInChildren<SimpleScrollSnap>(true);
		if (bookButtonSelector.targetPanel == 1) {					// inactif
			if (state == State.inactive)
				halo.SetTrigger("startColorON");
			state = State.active;
		} else if (bookButtonSelector.targetPanel == 2) {			// actif fermé
			state = State.open;
		} else {													// actif ouvert
			if (state == State.active)
				halo.SetTrigger("startColorOFF");
			state = State.inactive;
		}
		MagicUI.Instance.SetState(state);
	}

	/// <summary>
	/// afficher le livre ouvert sur la section des quêtes
	/// </summary>
	public void ShowQuests() {
		book.SetQuestSection();
		bookButtonSelector.targetPanel = 2;
		ToggleFullScreen();
	}

	/// <summary>
	/// afficher le livre ouvert sur la section de la magie
	/// </summary>
	public void ShowMagic() {
		if (book.section != Book.Section.magic)
			book.SetMagicSection();
		ToggleFullScreen();
	}
	public void Close() {
		bookButtonSelector.GoToPanel(1);
	}

}
