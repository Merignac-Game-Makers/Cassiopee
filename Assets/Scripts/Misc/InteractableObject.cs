﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base class for interactable object, inherit from this class and override InteractWith to handle what happen when
/// the player interact with the object.
/// </summary>
public abstract class InteractableObject : HighlightableObject
{
	public abstract bool IsInteractable { get; }
	public bool OnTheFly = true;
	[HideInInspector]
	public bool Clicked;

	public abstract void InteractWith(HighlightableObject target);
}
