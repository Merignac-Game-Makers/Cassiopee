using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestBase : MonoBehaviour
{
	public enum QuestStatus { None, Accepted, Refused, Done, Failed }
	[HideInInspector]
	public QuestStatus status = QuestStatus.None;

	public abstract bool TestSuccess();
	public abstract void Reset();
}
