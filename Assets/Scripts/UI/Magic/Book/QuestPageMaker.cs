using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PageMaker.Side;

public class QuestPageMaker : PageMaker
{
	public TMP_Text title;

	Side side = left;
	public List<QuestBase> quests { get; private set; }

	private void Start() {
		quests = new List<QuestBase>();
	}

	public override void Make() {
		title.text = "Objectifs";

		foreach (SetQuestPanel qp in leftContent.GetComponentsInChildren<SetQuestPanel>()) {
			qp.gameObject.SetActive(qp.quest != null);
			//if (qp.quest == null)
			//	qp.gameObject.SetActive(false);
		}
		foreach (SetQuestPanel qp in rightContent.GetComponentsInChildren<SetQuestPanel>()) {
			qp.gameObject.SetActive(qp.quest != null);
			//if (qp.quest == null)
			//	qp.gameObject.SetActive(false);
		}
	}

	public bool AddQuest(QuestBase quest) {
		SetQuestPanel qp = GetFirtsFree();
		if (qp != null) {
			qp.SetQuest(quest);
			quests.Add(quest);
			return true;
		}
		return false;
	}
	public bool UpdateQuest(QuestBase quest) {
		SetQuestPanel qp = GetQuestPanel(quest);
		if (qp != null) {
			qp.SetQuest(quest);
			return true;
		}
		return false;
	}
	public SetQuestPanel GetQuestPanel(QuestBase quest) {
		foreach (SetQuestPanel qp in leftContent.GetComponentsInChildren<SetQuestPanel>(true)) {
			if (qp.quest == quest)
				return qp;
		}
		foreach (SetQuestPanel qp in rightContent.GetComponentsInChildren<SetQuestPanel>(true)) {
			if (qp.quest == quest)
				return qp;
		}
		return null;

	}

	public SetQuestPanel GetFirtsFree() {
		foreach (SetQuestPanel qp in leftContent.GetComponentsInChildren<SetQuestPanel>(true)) {
			if (qp.quest == null)
				return qp;
		}
		foreach (SetQuestPanel qp in rightContent.GetComponentsInChildren<SetQuestPanel>(true)) {
			if (qp.quest == null)
				return qp;
		}
		return null;
	}

}
