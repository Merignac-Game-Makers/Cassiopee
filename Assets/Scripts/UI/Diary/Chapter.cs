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

	public List<int> state = new List<int>();   // liste des index courants des paragraphes

	public List<int> State() {
		state.Clear();
		foreach (Paragraph p in paragraphs) {
			state.Add(0); //p.idx
		}
		return state;
	}

	public void Reset() {
		foreach (Paragraph p in paragraphs) {
			p.Reset();
		}
	}

	public void EnableParagraph(int paragraph) {
		if (paragraph >= 0 && paragraph < paragraphs.Count) {
			paragraphs[paragraph].enabled = true;
		}
	}

	public void SetParagraphVersion(int paragraph, int version) {
		if (paragraph >= 0 && paragraph < paragraphs.Count) {
			paragraphs[paragraph].Set(version);
		}
	}
	public void SetParagraphNext(int paragraph) {
		if (paragraph >= 0 && paragraph < paragraphs.Count) {
			paragraphs[paragraph].Next();
		}
	}

}