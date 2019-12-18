using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestBase : MonoBehaviour
{
	public enum QuestStatus { None, Accepted, Refused, Done }
	[HideInInspector]
	public QuestStatus questStatus = QuestStatus.None;
	public void RefuseQuest() {
		questStatus = QuestStatus.Refused;
	}
	public void AcceptQuest() {
		questStatus = QuestStatus.Accepted;
	}
	public void QuestDone() {
		questStatus = QuestStatus.Done;
	}

}
