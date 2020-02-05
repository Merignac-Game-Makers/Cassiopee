using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Chapitre de journal", menuName = "Custom/Chapitre", order = -999)]
public class Chapter : ScriptableObject
{
	public bool enabled;                        // ce chapitre est-il affiché ?
	public string title;                        // titre du chapitre
	public List<Paragraph> paragraphs;          // un chapitre peut compter plusieurs paragraphes dont certians ne seront pas affichés au départ

	[HideInInspector]
	public List<int> state = new List<int>();   // liste des index courants des paragraphes

	/// <summary>
	/// Création de la liste des avancements des paragraphes (tous au début)
	/// </summary>
	/// <returns></returns>
	public List<int> State() {
		state.Clear();							// vider la liste des paragraphes
		foreach (Paragraph p in paragraphs) {	// pour chaque paragraphe
			state.Add(0); //p.idx				// initialiser l'avancement au début
		}
		return state;
	}

	/// <summary>
	/// Réinitialisation des paragraphes
	/// </summary>
	public void Reset() {
		foreach (Paragraph p in paragraphs) {
			p.Reset();
		}
	}

	/// <summary>
	/// Rendre un paragraphe affichable
	/// </summary>
	/// <param name="paragraphIndex"></param>
	public void EnableParagraph(int paragraphIndex) {
		if (paragraphIndex >= 0 && paragraphIndex < paragraphs.Count) {
			paragraphs[paragraphIndex].enabled = true;
		}
	}

	/// <summary>
	/// Modifier l'avancement d'un paragraphe
	/// </summary>
	/// <param name="paragraphIndex"></param>
	/// <param name="version"></param>
	public void SetParagraphVersion(int paragraphIndex, int version) {
		if (paragraphIndex >= 0 && paragraphIndex < paragraphs.Count) {
			paragraphs[paragraphIndex].Set(version);
		}
	}

	/// <summary>
	/// Avancer un paragraphe à la version suivante
	/// </summary>
	/// <param name="paragraphIndex"></param>
	public void SetParagraphNext(int paragraphIndex) {
		if (paragraphIndex >= 0 && paragraphIndex < paragraphs.Count) {
		state[paragraphIndex] = paragraphs[paragraphIndex].Next();
		}
	}

}