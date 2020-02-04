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

	VerticalLayoutGroup currentContent => side == left ? leftContent : rightContent;

	public TMP_Text paragraphTemplate;

	Side side = left;

	public Chapter chapter;


	public override void Make() {
		if (chapter == null) return;
			
		title.text = chapter.title;
		Clear(leftContent);
		Clear(rightContent);

		for (int i = 0; i < chapter.state.Count; i++) {
			if (chapter.paragraphs[i].enabled) {
				AddParagraph(chapter.paragraphs[i].Get(chapter.state[i]));
				chapter.state[i] = chapter.paragraphs[i].Next();
			}
		}
	}

	public void AddParagraph(string paragraph) {
		TMP_Text newParagraph = Instantiate(paragraphTemplate, currentContent.transform, false);
		newParagraph.text = paragraph;
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
