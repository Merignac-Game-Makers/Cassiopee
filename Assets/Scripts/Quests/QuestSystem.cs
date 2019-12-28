using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuestBase.QuestStatus;

public class QuestSystem : MonoBehaviour
{

	public static QuestSystem Instance;

	[HideInInspector]
	public List<QuestBase> quests; 
	public void Init() {
		Instance = this;
	}

	private void Start() {
		quests = new List<QuestBase>(GetComponentsInChildren<QuestBase>());
	}

	public void Add(QuestBase quest) {
		quests.Add(quest);
	}

	public List<QuestBase> GetPending() {
		var list = new List<QuestBase>();
		foreach (QuestBase quest in quests){
			if (quest.status == Accepted || quest.status == Failed)
				list.Add(quest);
		}
		return list;
	}
	public List<QuestBase> GetDone() {
		var list = new List<QuestBase>();
		foreach (QuestBase quest in quests) {
			if (quest.status == Done || quest.status == Passed)
				list.Add(quest);
		}
		return list;
	}

	public void SetStatus(QuestBase quest, QuestBase.QuestStatus status) {
		int idx = quests.IndexOf(quest);
		if (idx != -1) {
			quests[idx].status = status;
		}
	}
}
