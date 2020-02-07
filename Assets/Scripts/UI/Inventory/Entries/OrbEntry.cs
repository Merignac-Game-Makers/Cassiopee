using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Entrée d'inventaire spécifique pour les orbes de magie
/// contient :
///		- l'orbe
/// pas de quantité (on ne peut détenir qu'un orbe à la fois)
/// </summary>
public class OrbEntry : Entry
{
	public MagicOrb orb;

	public OrbEntry(MagicOrb orb) {
		this.orb = orb;
	}
}

