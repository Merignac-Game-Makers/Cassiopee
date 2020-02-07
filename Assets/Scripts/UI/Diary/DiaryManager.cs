using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryManager : MonoBehaviour
{
	public static DiaryManager Instance;

	public DiaryBookContent diaryBookContent;
	public DiaryPageMaker pagePrefab;
	ChapterManager[] chapterManagers;
	List<Chapter> chapters;

	DiaryPageMaker pageMaker;

	private void Awake() {
		Instance = this;
	}

	/// <summary>
	/// Initialisation  => création des pages rattachées à 'diaryBookContent'
	/// Appelé par <see cref="DiaryBookContent.Init"/>
	/// </summary>
	public void Init() {
		chapterManagers = GetComponentsInChildren<ChapterManager>();			// récupération des chapitres (rattachés aux enfants de ce gameObject)
		chapters = new List<Chapter>();											// initialisation de la liste des chapitres
		foreach (ChapterManager cm in chapterManagers) {						// pour chaque chapitre trouvé
			chapters.Add(cm.chapter);											// ajout à la liste des chapitres 
			pageMaker = Instantiate(pagePrefab.gameObject, diaryBookContent.transform).GetComponentInChildren<DiaryPageMaker>();	// création de la page
			pageMaker.SetChapterManager(cm);									// rattachement du chapitre à la page
			//pageMaker.Make();
		}
	}
}
