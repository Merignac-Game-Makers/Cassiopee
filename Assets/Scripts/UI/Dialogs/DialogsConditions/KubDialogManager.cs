using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using static QuestBase;

public class KubDialogManager : MonoBehaviour
{
	public QuestBase quest;
	public int AcceptedQuestNode = -1;
	public int RefusedQuestNode = -1;
	public int DoneQuestNode = -1;
	public int FailedQuestNode = -1;
	public int PassedQuestNode = -1;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public bool QuestConditions(VIDE_Assign dialogue) {
		//if (quest.TestSuccess())
		//	quest.status = QuestStatus.Done;
		quest.TestSuccess();

		if (VD.isActive) {  //Stuff we check while the dialogue is active
			var data = VD.nodeData;

			
		} else {            //Stuff we do right before the dialogue begins
			if (quest.status == QuestStatus.Refused) {
				dialogue.overrideStartNode = RefusedQuestNode;
			} else if (quest.status == QuestStatus.Accepted) {
				dialogue.overrideStartNode = AcceptedQuestNode;
			} else if (quest.status == QuestStatus.Done) {
				dialogue.overrideStartNode = DoneQuestNode;
			} else if (quest.status == QuestStatus.Failed) {
				dialogue.overrideStartNode = FailedQuestNode;
			} else if (quest.status == QuestStatus.Passed) {
				dialogue.overrideStartNode = PassedQuestNode;
			}
		}
		return true;
	}
}
