using System;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Classe générique pour les PNJ
/// => Intéraction par défaut = interrompre le déplacement + lancer le dialogue
/// </summary>
public class PNJ : InteractableObject
{
	public string PNJname;     // nom du PNJ (pour les dialogues)
	public Sprite image;    // image du PNJ (pour les dialogues)

	public override bool IsInteractable => true;

	protected override void Start() {
		base.Start();
		var dialogue = gameObject.GetComponentInChildren<VIDE_Assign>();
		if (dialogue != null) {
			if (image != null) {
				dialogue.defaultNPCSprite = image;
			}
			if (!String.IsNullOrEmpty(PNJname)) {
				dialogue.alias = PNJname;
			}
		}
	}

	public override void InteractWith(HighlightableObject target) {
		base.InteractWith(target);
		PlayerManager.Instance.StopAgent();
		GetComponentInChildren<DialogueTrigger>()?.Run();
	}

}
