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

	public bool isOn = false;               // flag allumé ?

	// pour les projecteurs
	Projector projector;
	Animator animator;
	// pour les systèmes de particules
	ParticleSystem[] particlesList;
	ParticleSystem particles;
	// pour les tores
	SelectionRing ring;

	protected virtual void Start() {
		// pour les projecteurs
		projector = GetComponentInChildren<Projector>();
		animator = GetComponentInChildren<Animator>();
		if (projector)
			projector.material = new Material(projector.material);
		// pour les systèmes de particules
		particlesList = GetComponentsInChildren<ParticleSystem>();
		foreach( ParticleSystem ps in particlesList) {
			if (ps.name == "MagicParticles") {
				particles = ps;
				break;
			}
		}
		// pour les tores
		ring = GetComponentInChildren<SelectionRing>();

		Highlight(false);
	}

	/// <summary>
	/// true  : allumer le projecteur
	/// false : éteindre le projecteur
	/// </summary>
	public virtual void Highlight(bool on) {
		// pour les projecteurs
		if (animator)
			animator.enabled = on;
		if (projector)
			projector.enabled = on;
		// pour les systèmes de particules
		if (particles) {
			if (on)
				particles.Play();
			else
				particles.Stop();
		}
		// pour les tores
		if (ring)
			ring.Highlight(on);

		isOn = on;
	}

	public void SetColor(Color color) {
		// pour les projecteurs
		if (projector)
			projector.material.color = color;
		// pour les systèmes de particules
		if (particles) {
			var psMain = particles.main;
			psMain.startColor = color;
		}
		// pour les tores
		if (ring)
			ring.SetColor(color);
	}

}
