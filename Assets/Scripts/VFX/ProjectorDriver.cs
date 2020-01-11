﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorDriver : MonoBehaviour
{
	public Color color = Color.white;		// couleur
	public Texture cookie;					// motif

	Projector projector;
	Animator animator;
	Light light;

	void Start()
    {
		projector = GetComponentInChildren<Projector>();
		animator = GetComponentInChildren<Animator>();
		light = GetComponentInChildren<Light>();
		if (projector) {
			projector.material = new Material(projector.material);	// duplication du material pour ne pas changer tous les projecteurs simultanément
			projector.material.SetTexture("_ShadowTex", cookie);	// mise en place du motif
			projector.material.SetColor("_Color", color);			// mise en place de la couleur
		}
	}

	/// <summary>
	/// true  : allumer 
	/// false : éteindre 
	/// </summary>
	public virtual void Highlight(bool on) {
		if (animator)
			animator.enabled = on;		// animation
		if (projector)
			projector.enabled = on;		// motif
		if (light)
			light.enabled = on;			// lumière
	}

	public void SetColor(Color color) {
		if (projector)
			projector.material.color = color; // changer la couleur
	}
}
