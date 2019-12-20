using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestBase : MonoBehaviour
{
	public enum QuestStatus { None, Accepted, Refused, Done, Failed, Passed }
	[HideInInspector]
	public QuestStatus status = QuestStatus.None;

	public static QuestsUI questsUI;

	public Sprite picture;
	public string title;
	public string shortText;
	public abstract bool TestSuccess();
	public abstract void Reset();

	private void Start() {
		questsUI = QuestsUI.Instance;
	}
}
