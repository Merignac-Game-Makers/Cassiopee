using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using static DialogueEntryNode.QuestStatus;

public class DefaultDialogDispatcher : DValid
{
	//[HideInInspector]
	public QuestBase currentQuest;

	public override bool QuestConditions(VIDE_Assign dialogue) {
		if (currentQuest == null)
			return true;

		foreach (Entry entry in entries) {
			if (entry.quest == currentQuest) {
				entry.quest.TestSuccess();
				if (VD.isActive) {  //Stuff we check while the dialogue is active
									//var data = VD.nodeData;


				} else {            //Stuff we do right before the dialogue begins
									//if (quest?.status == QuestStatus.Refused) {
									//	dialogue.overrideStartNode = RefusedQuestNode;
									//} else if (quest?.status == QuestStatus.Accepted) {
									//	dialogue.overrideStartNode = AcceptedQuestNode;
									//} else 
					if (entry.quest.status == Done) {
						dialogue.overrideStartNode = entry.DoneQuestNode;
					} else if (entry.quest.status == Failed) {
						dialogue.overrideStartNode = entry.FailedQuestNode;
					} else if (entry.quest.status == Passed) {
						dialogue.overrideStartNode = entry.PassedQuestNode;
					} else if (entry.quest?.status == Available) {
						dialogue.overrideStartNode = entry.AvailableQuestNode;
					} else if (entry.quest.status == NonAvailable) {
						dialogue.overrideStartNode = entry.NonAvailableQuestNode;
					}
				}
			}
		}

		return true;
	}

}
