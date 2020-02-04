using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{

	public DiaryBookContent diaryBookContent;
	public DiaryPageMaker pagePrefab;
	ChapterManager[] chapterManagers;
	List<Chapter> chapters;

	DiaryPageMaker pageMaker;
	void Start() {
		chapterManagers = GetComponentsInChildren<ChapterManager>();
		chapters = new List<Chapter>();
		foreach (ChapterManager cm in chapterManagers) {
			chapters.Add(cm.chapter);
			pageMaker = Instantiate(pagePrefab.gameObject, diaryBookContent.transform).GetComponentInChildren<DiaryPageMaker>();
			pageMaker.chapter = cm.chapter;
			pageMaker.Make();
		}

	}

	// Update is called once per frame
	void Update() {

	}
}
