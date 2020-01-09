using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe de base pour les boutons d'interface (Dialogues, Inventaire, Magie ou QUêtes)
/// </summary>
public class UIButton : MonoBehaviour
{

	public KeyCode key;			// raccourci clavier
	public UIBase target;		// interface à activer (Dialogues, Inventaire, Magie ou QUêtes)
	public Sprite open;			// visuel pour ouvrir
	public Sprite close;		// visuel pour fermer

	Image image;				// l'objet image contenant le visuel à afficher

	private void Start() {
		image = GetComponent<Image>();
		image.sprite = open;	// par défaut : visuel 'pour ouvrir'
	}

	void Update() {
		//Keyboard shortcut
		if (Input.GetKeyUp(key))
			Toggle();
	}

	/// <summary>
	/// bascule ouvert/fermé
	/// </summary>
	public void Toggle() {
		target.Toggle();
		image.sprite = image.sprite == target.isOn ? close : open;
	}
}
