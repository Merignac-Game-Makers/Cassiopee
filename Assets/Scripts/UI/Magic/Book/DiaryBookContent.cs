using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryBookContent : BaseBookContent
{
	public static DiaryBookContent Instance;
	public GameObject pagePrefab;
	public Book book;
	public DiaryPageMaker[] content;

	public List<List<int>> story;

	private void Awake() {
		Instance = this;
	}

	private void Start() {
		story = new List<List<int>>();									// initialiser la liste globale
		for (int p = 0; p < content.Length; p++) {                      // pour chaque page
			story.Add(content[p].chapter.State());						// ajouter les index courants des paragraphes
		}
	}

	public void AddPage(Chapter chapter) {
		var page = Instantiate(pagePrefab, transform);
		var pageMaker = page.GetComponent<DiaryPageMaker>();
		pageMaker.chapter = chapter;
		pageMaker.Make();
	}

	public void UpdateChapter(Chapter chapter) {
		foreach (DiaryPageMaker dpm in GetComponentsInChildren<DiaryPageMaker>()) {
			if (dpm.chapter == chapter) {
				book.MakeCurrentPages();
				book.UpdateCurrentPages();
				break;
			}
		}
	}

	// trouver la page suivante (
	public override PageMaker GetNextAvailablePage(int after) {
		if (after < content.Length - 1)
			return content[after + 1];
		return null;
	}

	public override PageMaker GetPreviousAvailablePage(int before) {
		if (before > 0)
			return content[before - 1];
		return null;
	}
}
