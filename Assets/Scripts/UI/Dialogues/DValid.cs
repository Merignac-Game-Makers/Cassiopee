using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using static QuestBase;

public abstract class DValid : MonoBehaviour
{
	public Entry[] entries;

	public abstract bool QuestConditions(VIDE_Assign dialogue);

	[System.Serializable]
	public class Entry
	{
		public QuestBase quest;
		public int NonAvailableQuestNode;
		public int AvailableQuestNode;
		public int FailedQuestNode;
		public int DoneQuestNode;
		public int PassedQuestNode;

	}

	public Entry GetCurrent(QuestBase quest) {
		foreach (Entry entry in entries) {
			if (entry.quest == quest)
				return entry;
		}
		return null;
	}
}
