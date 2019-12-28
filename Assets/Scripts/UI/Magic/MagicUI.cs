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
	public GameObject fullScreenPanel;  // livre ouvert (plein écran)
	public GameObject leftPage;         // livre ouvert - âge gauche
	public GameObject rightPage;        // livre ouvert - âge droite
	public GameObject bookButton;       // activer/désactiver le grimoire
	public GameObject fullScreenButton; // bascule d'affichage du mode 'plein écran'
	public GameObject sidePanel;        // panneau latéral contenant les artefacts
	public GameObject nextPage;			// coin 'page suivante'
	public GameObject prevPage;			// coin 'page précédente'
	public GameObject sunButton;		// bouton artefact SUN
	public GameObject moonButton;       // bouton artefact MOON

	// zones de contenu
	public Text m_Title;                // titre de la page 
	public Text m_Text;                 // texte
	public Image m_Picture;             // image

	// autres
	public static MagicUI Instance;      // instance statique
	public enum SelectedArtefact { Moon, Sun }						// artefact sélectionnable
	public SelectedArtefact selectedArtefact { get; private set; }	// artefact sélectionné
	public bool isFullScreen => fullScreenButton.activeInHierarchy; // flag 'plein écran'
	public int currentPageIdx { get; private set; }					// index de la page courante

	/// <summary>
	/// initialisation
	/// </summary>
	public override void Init(UIManager uiManager) {
		Instance = this;                // instance statique

		gameObject.SetActive(true);     // Book UI actif
		panel.SetActive(false);         // panneau latéral masqué

		pages = new List<Page>();		// récupération des pages dans 'MagicBookContent'
		foreach (PageTemplate page in magicBookContent.GetComponentsInChildren<PageTemplate>()) {
			pages.Add(page.page);
		}

		currentPageIdx = 0;             // page courante = 1ère page
		ShowPage(currentPageIdx);       // afficher la page courante

		selectedArtefact = SelectedArtefact.Sun;	// artefact sélectionné par défaut = SUN

	}

	/// <summary>
	/// bascule grimoire actif/inactif
	/// </summary>
	public override void Toggle() {
		panel.SetActive(!panel.activeInHierarchy);      // panneau principal
														//sidePanel.SetActive(isOn);						// panneau latéral
	}

	/// <summary>
	/// bascule affichage plein écran
	/// </summary>
	public void ToggleFullScreen() {
		fullScreenPanel.SetActive(!fullScreenPanel.activeInHierarchy);
	}

	/// <summary>
	/// Afficher une page
	/// </summary>
	/// <param name="idx">#page à afficher</param>
	void ShowPage(int idx) {
		m_Title.text = pages[idx].Title;
		m_Text.text = pages[idx].Text;
		m_Picture.sprite = pages[idx].Picture;
		prevPage.SetActive(idx > 0);
		nextPage.SetActive(idx < MaxPage - 1);
		currentPageIdx = idx;
	}

	/// <summary>
	/// page suivante
	/// </summary>
	public void NextPage() {
		if (currentPageIdx < MaxPage - 1) {
			ShowPage(currentPageIdx + 1);
		}
	}

	/// <summary>
	/// page précédente
	/// </summary>
	public void PrevPage() {
		if (currentPageIdx > 0) {
			ShowPage(currentPageIdx - 1);
		}
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
	public void selectArtefact(GameObject button) {
		selectedArtefact = button == moonButton ? SelectedArtefact.Moon : SelectedArtefact.Sun;
		MagicController.Instance.UpdateArtefact(selectedArtefact);
	}

}

#if UNITY_EDITOR
[CustomEditor(typeof(MagicUI))]
[CanEditMultipleObjects]
public class BookUIEditor : Editor
{
	MagicUI ui;
	public void OnEnable() {
		//serializedObject.ApplyModifiedProperties();
		ui = (MagicUI) target;
	}


	public override void OnInspectorGUI() {

		EditorStyles.textField.wordWrap = true;

		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.magicBookContent)), true);

		GUILayout.Label("\nPanels", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.panel)));
		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.fullScreenPanel)));
		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.leftPage)));
		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.rightPage)));
		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.sidePanel)));

		GUILayout.Label("\nButtons", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.bookButton)));
		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.fullScreenButton)));
		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.nextPage)));
		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.prevPage)));
		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.sunButton)));
		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.moonButton)));

		GUILayout.Label("\nContenu", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.m_Title)));
		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.m_Text)));
		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ui.m_Picture)));

		serializedObject.ApplyModifiedProperties();
	}

}
#endif
