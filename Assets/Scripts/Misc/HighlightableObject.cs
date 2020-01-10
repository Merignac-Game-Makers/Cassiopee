using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// All object that can be highlighted (enemies, interactable object etc.) derive from this class, which takes care
/// of setting the material parameters for it when it gets highlighted.
/// If the object use another material, it will just ignore all the changes.
/// </summary>
public class HighlightableObject : MonoBehaviour
{

	public bool isOn = false;				// flag allumé ?

	Projector projector;
	Animator animator;

	protected virtual void Start() {
		projector = GetComponentInChildren<Projector>();
		animator = GetComponentInChildren<Animator>();
		Highlight(false);
		if (projector)
			projector.material = new Material(projector.material);
	}

	/// <summary>
	/// true  : allumer le projecteur
	/// false : éteindre le projecteur
	/// </summary>
	public virtual void Highlight(bool on) {
		if (animator)
			animator.enabled = on;
		if (projector)
			projector.enabled = on;
		isOn = on;
	}
	 
	public void SetColor(Color color) {
		if (projector)
			projector.material.color = color;
	}

}
