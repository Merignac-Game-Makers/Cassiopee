using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gestionnaire général des interfaces (Dialogues, Inventaire, Magie ou QUêtes)
/// </summary>
public class UIManager : MonoBehaviour
{
	public static UIManager Instance;

	public DialoguesUI dialoguesUI;     // interface Dialogues
	public InventoryUI inventoryUI;     // interface Inventaire
	public MagicUI magicUI;             // interface Magie
	public QuestsUI questsUI;           // interface Quêtes

	public UIButton magicButton;        // bouton du grimoire		
	public Button artifactButton;		// bouton des artefacts		
	public UIButton inventoryButton;    // bouton due l'inventaire		
	public Button exitButton;			// bouton exit

	void Awake() {
		Instance = this;
	}

	void OnEnable() {
		dialoguesUI.Init(this);
		inventoryUI.Init(this);
		magicUI.Init(this);
		questsUI.Init(this);

	}

	/// <summary>
	/// Gérer la coordination d'affichage des boutons
	/// (masquer le bouton grimoire quand on affiche l'inventaire ou les quêtes)
	/// </summary>
	public void ManageButtons() {
		if (PlayerManager.Instance.gameObject.GetComponentInChildren<CharacterData>().isMagicEquiped) {
			magicUI.SetState();
		}
	}
}
