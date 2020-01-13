using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Code relatif à l'interface utilisateur du grimoire
/// </summary>
public class MagicUI : UIBase
{
	// Définition du contenu du grimoire
	[HideInInspector]
	public int MaxPage => pages.Count;
	public GameObject magicBookContent;
	List<Page> pages;

	// objets d'interface
	[Header("Panels")]
	public GameObject bookPanel;        // panneau livre ouvert (plein écran)
	public Book book;                   // livre


	[Header("Boutons")]
	public GameObject bookButton;       // activer/désactiver le grimoire
	public GameObject artefactButton;   // bouton artefact 
	public GameObject helpButton;       // bouton aide

	// zones de contenu
	[Header("Zones de contenu")]
	public Sprite moon;                 // image médaillon lune
	public Sprite sun;                  // image médaillon soleil

	// autres
	public static MagicUI Instance;      // instance statique
	public enum SelectedArtefact { Moon, Sun }                      // artefact sélectionnable
	public SelectedArtefact selectedArtefact { get; private set; }  // artefact sélectionné
	public int currentPageIdx { get; private set; }                 // index de la page courante

	public enum State { inactive, active, open }
	private State state;

	/// <summary>
	/// initialisation
	/// </summary>
	public override void Init(UIManager uiManager) {
		Instance = this;                // instance statique

		gameObject.SetActive(true);     // Book UI actif
		panel.SetActive(false);         // panneau masqué

		book.Init();
		//ShowPage(currentPageIdx);       // afficher la page courante

		selectedArtefact = SelectedArtefact.Sun;    // artefact sélectionné par défaut = SUN

		// au démarrage du jeu
		state = State.open;
		bookButton.GetComponent<Image>().color = new Color(1, 1, 1, .6f);   // grimoire transparent
		bookButton.gameObject.SetActive(false);                             // grimoire masqué
		artefactButton.gameObject.SetActive(false);                         // artefact masqué

	}

	/// <summary>
	/// bascule actif/inactif
	/// </summary>
	public override void Toggle() { }

	/// <summary>
	/// bascule affichage plein écran
	/// </summary>
	public void ToggleFullScreen() {
		if (state == State.inactive) {
			SetState(State.active);
		} else if (state == State.active) {
			SetState(State.open);
		} else {
			SetState(State.inactive);
		}

		//bookPanel.SetActive(!bookPanel.activeInHierarchy);
		if (isOn && InventoryUI.Instance.isOn) {
			InventoryUI.Instance.Toggle();
		}
	}

	public void SetState(State state) {
		this.state = state;
		if (state == State.inactive) {
			bookButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);     // grimoire opaque
			artefactButton.gameObject.SetActive(true);                          // médaillon visible
		} else if (state == State.active) {
			bookPanel.gameObject.SetActive(true);                               // livre ouvert visible
		} else {
			artefactButton.gameObject.SetActive(false);                         // médaillon invisible
			bookPanel.gameObject.SetActive(false);                              // livre ouvert invisible
			bookButton.GetComponent<Image>().color = new Color(1, 1, 1, .6f);   // grimoire transparent
		}

	}
	public void SetState() {
		SetState(state);
	}

	public void ShowButtons(bool on) {

	}

	/// <summary>
	/// Récupérer le contenu d'une page
	/// </summary>
	/// <param name="idx">#page à récupérer</param>
	/// <returns></returns>
	public Page GetPage(int idx) {
		if (idx < MaxPage) {
			return pages[idx];
		} else {
			return null;
		}
	}

	/// <summary>
	/// Récupérer le contenu de la page courante
	/// </summary>
	/// <returns></returns>
	public Page GetPage() {
		return pages[currentPageIdx];
	}

	/// <summary>
	/// Sélectionner un artefact
	/// </summary>
	/// <param name="button">bouton de l'artefact à sélectionner</param>
	public void selectArtefact() {
		selectedArtefact = selectedArtefact == SelectedArtefact.Sun ? SelectedArtefact.Moon : SelectedArtefact.Sun;
		artefactButton.GetComponent<Image>().sprite = selectedArtefact == SelectedArtefact.Sun ? sun : moon;
		MagicManager.Instance.UpdateArtefact(selectedArtefact);
	}

}
