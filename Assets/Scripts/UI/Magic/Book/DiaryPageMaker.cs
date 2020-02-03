using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PageMaker.Side;


public class DiaryPageMaker : PageMaker
{
	public TMP_Text title;
	public GameObject leftContent;
	public GameObject rightContent;

	GameObject currentContent => side == left ? leftContent : rightContent;

	public TMP_Text paragraphTemplate;

	Side side = left;

	public string titleString;
	public List<Paragraph> paragraphs; // { get; private set; }

	private void Start() {
		//paragraphs = new List<string>();
		paragraphs.Add(Resources.Load("Diary/GM001") as Paragraph);
		Make(); 
	}

	public override void Make() {
		title.text = titleString;
		Clear(leftContent);
		foreach (Paragraph p in paragraphs) {
			AddParagraph(p.text);
		}
	}

	public void AddParagraph(string paragraph) {
		TMP_Text newParagraph = Instantiate(paragraphTemplate, currentContent.transform, false);
		newParagraph.text = paragraph;

	}

	void Clear(GameObject content) {
		foreach (Transform child in content.transform) {
			Destroy(child.gameObject);
		}
	}
	//public bool UpdateQuest(QuestBase quest) {
	//	SetQuestPanel qp = GetQuestPanel(quest);
	//	if (qp != null) {
	//		qp.SetQuest(quest);
	//		return true;
	//	}
	//	return false;
	//}
	//public SetQuestPanel GetQuestPanel(QuestBase quest) {
	//	foreach (SetQuestPanel qp in leftContent.GetComponentsInChildren<SetQuestPanel>(true)) {
	//		if (qp.quest == quest)
	//			return qp;
	//	}
	//	foreach (SetQuestPanel qp in rightContent.GetComponentsInChildren<SetQuestPanel>(true)) {
	//		if (qp.quest == quest)
	//			return qp;
	//	}
	//	return null;

	//}

}
