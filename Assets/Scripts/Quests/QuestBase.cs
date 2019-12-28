using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuestBase.QuestStatus;

public abstract class QuestBase : MonoBehaviour
{
	public enum QuestStatus { NonAvailable, Available, Accepted, Refused, Done, Failed, Passed }
	[HideInInspector]
	public QuestStatus status = QuestStatus.Available;

	public static QuestsUI questsUI;
	public static QuestSystem questSystem;

	public Sprite picture;
	public string title;
	public string shortText;

	private void Start() {
		questsUI = QuestsUI.Instance;
		questSystem = QuestSystem.Instance;
	}



	public virtual void QuestAvailable() {
		status = Available;
		questSystem.SetStatus(this, Available);
	}
	public virtual void RefuseQuest() {
		status = Refused;
		questSystem.SetStatus(this, Refused);
	}
	public virtual void AcceptQuest() {
		status = Accepted;
		questSystem.SetStatus(this, Accepted);
	}
	public virtual void QuestDone() {
		status = Done;
		questSystem.SetStatus(this, Done);
	}
	public virtual void QuestFailed() {
		status = Failed;
		questSystem.SetStatus(this, Failed);
	}
	public virtual void QuestPassed() {
		status = Passed;
		questSystem.SetStatus(this, Passed);
	}
	public abstract bool TestSuccess();
	public abstract void Reset();



}
