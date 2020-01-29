using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBookContent : BaseBookContent
{
	public static QuestBookContent Instance;
	public QuestPageMaker[] content;
	public Book book;

	private void Awake() {
		Instance = this;
	}

	public void AddQuest(QuestBase quest) {
		foreach (QuestPageMaker qpm in GetComponentsInChildren<QuestPageMaker>()) {
			if (qpm.AddQuest(quest)) {
				book.MakeCurrentPages();
				book.UpdateCurrentPages();
				break;
			}
		}
	}

	public void UpdateQuest(QuestBase quest) {
		foreach (QuestPageMaker qpm in GetComponentsInChildren<QuestPageMaker>()) {
			if (qpm.UpdateQuest(quest)) {
				book.MakeCurrentPages();
				book.UpdateCurrentPages();
				break;
			}
		}
	}

	public void UpdateQuestList() {
		foreach (QuestBase quest in QuestManager.Instance.quests) {							// pour chaque quête
			foreach (QuestPageMaker qpm in GetComponentsInChildren<QuestPageMaker>()) {     // pour chaque page existante
				if (qpm.quests != null && qpm.quests.Contains(quest)) {						// si la page contient la quête
					qpm.GetQuestPanel(quest).SetQuest(quest);								// mettre l'affichage à jour
				}
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
