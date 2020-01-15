﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Base class for interactable object, inherit from this class and override InteractWith to handle what happen when
/// the player interact with the object.
/// </summary>
public abstract class InteractableObject : HighlightableObject
{

	public enum Mode { onClick, onTheFly, onTheFlyOnce }	// modes d'intéraction possibles

	public Mode mode;                                       // le mode d'intéraction de cet objet

	public abstract bool IsInteractable();					// m'objet est-il actif pour l'intéraction ?

	[HideInInspector]
	public bool Clicked;									// flag clic sur l'objet ?


	public virtual void InteractWith(HighlightableObject target) {
		Clicked = false;
		if (mode == Mode.onTheFlyOnce)
			mode = Mode.onClick;
	}

}
