using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;

/// <summary>
/// Gestionnaire général des interfaces (Dialogues, Inventaire, Magie ou QUêtes)
/// </summary>
public class UIManager : MonoBehaviour
{
	public static UIManager Instance;

	public DialoguesUI dialoguesUI;				// interface Dialogues
	public InventoryUI inventoryUI;				// interface Inventaire
	public DiaryBookContent diaryBookContent;   // pages du journal
	public MagicUI magicUI;						// interface Magie

	public GameObject magicButton;				// bouton du grimoire		
	public Button artifactButton;				// bouton des artefacts		
	public Button exitButton;					// bouton exit
	public GameObject questButton;				// bouton des quêtes		

	public GameObject messageLabel;

	Coroutine coroutine;

	void Awake() {
		Instance = this;
	}

	void OnEnable() {
		dialoguesUI.Init(this);				// initialisation du gestionnaire de dialogues
		inventoryUI.Init(this);             // initialisation du gestionnaire d'inventaire
		diaryBookContent.Init();			// initialisation des pages du journal
		magicUI.Init(this);                 // intialisation du gestionnaire de magie
		questButton.SetActive(false);
	}

	/// <summary>
	/// Gérer la coordination d'affichage des boutons
	/// (masquer le bouton grimoire quand on affiche l'inventaire ou les quêtes)
	/// </summary>
	public void ManageButtons() {
		if (PlayerManager.Instance.gameObject.GetComponentInChildren<CharacterData>().isMagicEquiped) {	// si le joueur dispose du grimoire
			magicUI.SetState();																			// coordonner les affichages
		}
	}

	/// <summary>
	/// afficher un message
	/// </summary>
	/// <param name="text">le message</param>
	/// <param name="position">la position d'affichage</param>
	public void ShowLabel(string text, Vector2 position) {
		messageLabel.GetComponentInChildren<TMP_Text>().text = text;
		messageLabel.transform.position = position;
		if (coroutine!=null)
			StopCoroutine(coroutine);
		coroutine = StartCoroutine(IShow(messageLabel, 2));
	}

	IEnumerator IShow(GameObject obj, float s) {
		obj.SetActive(true);
		yield return new WaitForSeconds(s);
		obj.SetActive(false);
	}

}
