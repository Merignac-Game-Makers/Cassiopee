using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static MagicButtonStates;
using static MagicButtonStates.State;
using static UIManager.State;

/// <summary>
/// Code relatif à l'interface utilisateur du grimoire
/// </summary>
public class MagicUI : UIBase
{
	// Définition du contenu du grimoire
	public int MaxPage => pages.Count;
	public MagicBookContent magicBookContent;
	List<Page> pages;

	// objets d'interface
	[Header("Panels")]
	public GameObject bookPanel;        // panneau livre ouvert (plein écran)
	public Book book;                   // livre
	public InventoryUI inventory;       // inventaire
	public GameObject orbPanel;			// orbe


	[Header("Boutons")]
	public GameObject bookButton;       // activer/désactiver le grimoire
	public GameObject artefactButton;   // bouton artefact 

	// zones de contenu
	[Header("Zones de contenu")]
	public Sprite moon;                 // image médaillon lune
	public Sprite sun;                  // image médaillon soleil

	// autres
	[Header("Autres")]
	public GameObject playerBody;

	public static MagicUI Instance;      // instance statique
	public enum SelectedArtefact { Moon, Sun }                      // artefact sélectionnable
	public SelectedArtefact selectedArtefact { get; private set; }  // artefact sélectionné
	public int currentPageIdx { get; private set; }                 // index de la page courante

	private MagicTrainingManager magicTrainingManager;
	private MagicButtonStates magicButtonStates;
	private Material playerMaterial;


	/// <summary>
	/// initialisation
	/// </summary>
	public override void Init(UIManager uiManager) {
		Instance = this;                // instance statique

		gameObject.SetActive(true);     // Book UI actif
		panel.SetActive(false);         // panneau masqué
		orbPanel.SetActive(false);		// panneau orbe masqué

		book.Init();					// initialisation du grimoire

		selectedArtefact = SelectedArtefact.Sun;    // artefact sélectionné par défaut = SUN

		bookButton.gameObject.SetActive(false);                             // grimoire masqué
		artefactButton.gameObject.SetActive(false);                         // artefact masqué

		magicTrainingManager = GetComponentInChildren<MagicTrainingManager>();
		magicButtonStates = bookButton.GetComponentInChildren<MagicButtonStates>();
		playerMaterial = playerBody.GetComponent<Renderer>().material;
	}

	/// <summary>
	/// bascule actif/inactif
	/// </summary>
	public override void Toggle() { }

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

	public void SetState(State state) {
		if (state == active) {
			//artefactButton.gameObject.SetActive(true);                          // médaillon visible
			bookPanel.gameObject.SetActive(false);                              // livre ouvert invisible
			//inventory.Restore();												// remettre l'inventaire comme il était lors de l'ouverture du livre
			PlayerManager.Instance.VisualMagicMode(true);                       // mise en évidence du mode 'magie activée'
			UIManager.Instance.ManageButtons(closedBook);						// coordination des boutons de l'UI
		} else if (state == open) {
			//artefactButton.gameObject.SetActive(true);                          // médaillon visible
			bookPanel.gameObject.SetActive(true);                               // livre ouvert visible
			//inventory.SaveAndHide();											// mémoriser la position de l'inventaire puis cacher l'inventaire
			PlayerManager.Instance.VisualMagicMode(true);                       // mise en évidence du mode 'magie activée'
			UIManager.Instance.ManageButtons(openBook);                         // coordination des boutons de l'UI
		} else {
			//artefactButton.gameObject.SetActive(false);                         // médaillon invisible
			bookPanel.gameObject.SetActive(false);                              // livre ouvert invisible
			MagicManager.Instance.SetMagicOff();                                // désactiver toute magie en cours
			PlayerManager.Instance.VisualMagicMode(false);                      // annulation de la mise en évidence du mode 'magie activée'
			UIManager.Instance.ManageButtons(noMagic);							// coordination des boutons de l'UI
		}
	}


	public void SetState() {
		SetState(magicButtonStates.state);
	}

}
