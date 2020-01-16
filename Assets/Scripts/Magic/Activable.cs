using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

/// <summary>
/// Objet magique 'activable' pour tracer une constellation
/// </summary>
public class Activable : InteractableObject
{

	public override bool IsInteractable() {
		// un objet magique n'est 'activable' que si
		//	- la magie est active
		//	- il n'y a pas d'orbe existante
		return MagicManager.Instance.isOn && MagicManager.Instance.currentOrb == null;
	}

	public bool IsActive => m_IsActive;
	[HideInInspector]
	public bool m_IsActive;

	public Color activeColor = Color.magenta;			// couleur 'actif'
	public Color inactiveColor = Color.white;			// couleur 'inactif'

	void Awake() {
		m_IsActive = false;								// par défaut l'objet est inactif
	}

	/// <summary>
	/// Ajouter/ retirer d'une constellation en cours :
	///		- ajouter => allumé
	///		- retirer => éteint
	/// </summary>
	public void Toggle() {
		m_IsActive = !m_IsActive;
		SetColor(IsActive ? activeColor : inactiveColor) ;
		MagicManager.Instance.AddOrRemove(this);
	}

	/// <summary>
	/// Désactiver et éteindre
	/// </summary>
	public void Inactivate() {
		m_IsActive = false;
		Highlight(false);                                                   
	}

	/// <summary>
	/// true  : allumer
	/// false : éteindre
	/// </summary>
	public override void Highlight(bool on) {
		base.Highlight(on);
		SetColor(IsActive ? activeColor : inactiveColor);
	}
}
