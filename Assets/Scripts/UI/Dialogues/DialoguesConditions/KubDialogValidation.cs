using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using static QuestBase;

public class KubDialogValidation : DialogValidation
{

	public override bool QuestConditions(VIDE_Assign dialogue) {
		//if (quest.TestSuccess())
		//	quest.status = QuestStatus.Done;
		quest?.TestSuccess();

		if (VD.isActive) {  //Stuff we check while the dialogue is active
			//var data = VD.nodeData;

			
		} else {            //Stuff we do right before the dialogue begins
			//if (quest?.status == QuestStatus.Refused) {
			//	dialogue.overrideStartNode = RefusedQuestNode;
			//} else if (quest?.status == QuestStatus.Accepted) {
			//	dialogue.overrideStartNode = AcceptedQuestNode;
			//} else 
			if (quest?.status == QuestStatus.Done) {
				dialogue.overrideStartNode = DoneQuestNode;
			} else if (quest?.status == QuestStatus.Failed) {
				dialogue.overrideStartNode = FailedQuestNode;
			} else if (quest?.status == QuestStatus.Passed) {
				dialogue.overrideStartNode = PassedQuestNode;
			}
		}
		return true;
	}

}
