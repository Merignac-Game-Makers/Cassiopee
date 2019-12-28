using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using static QuestBase;

public abstract class DialogValidation : MonoBehaviour
{
	public QuestBase quest;
	public int NonAvailableQuestNode = -1;
	public int AvailableQuestNode = -1;
	//public int AcceptedQuestNode = -1;
	//public int RefusedQuestNode = -1;
	public int FailedQuestNode = -1;
	public int DoneQuestNode = -1;
	public int PassedQuestNode = -1;


	public abstract bool QuestConditions(VIDE_Assign dialogue);
}
