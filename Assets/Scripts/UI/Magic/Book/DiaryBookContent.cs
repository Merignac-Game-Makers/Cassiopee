using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryBookContent : BaseBookContent
{
	public static DiaryBookContent Instance;
	public GameObject pagePrefab;
	public Book book;
	public DiaryPageMaker[] content { get; private set; } = new DiaryPageMaker[0];

	public List<List<int>> diary;

	private void Awake() {
		Instance = this;
	}
	
	/// <summary>
	/// Initialisation
	/// Appelé par <see cref="UIManager.OnEnable"/>
	/// </summary>
	public void Init() {
		DiaryManager.Instance.Init();
		content = GetComponentsInChildren<DiaryPageMaker>();
		book.diaryPages = new List<PageMaker>(content);
		diary = new List<List<int>>();                                  // initialiser la liste globale des chapitres
		for (int p = 0; p < content.Length; p++) {                      // pour chaque page
			diary.Add(content[p].chapter.State());                      // ajouter les index courants des paragraphes
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
