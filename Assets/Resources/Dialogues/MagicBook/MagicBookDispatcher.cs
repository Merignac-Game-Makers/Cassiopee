using UnityEngine;
using static QuestBase.QuestStatus;

public class MagicBookDispatcher : DialogueDispatcher
{
	QuestBase magicTraining;
	VIDE_Assign dialogue;

	public int available;
	public int pending;
	public int done;

	private void Start() {
		magicTraining = QuestManager.Instance.gameObject.GetComponentInChildren<MagicTraining>();
		dialogue = gameObject.GetComponent<VIDE_Assign>();
	}

	public override void SetStartNode() {
		magicTraining.UpdateStatus();

		if (magicTraining.status == Available) {            // quête accessible
			dialogue.overrideStartNode = available;
		}
		if (magicTraining.status == Accepted) {             // quête en cours
			dialogue.overrideStartNode = pending;
		}
		if ((int)magicTraining.status >= (int)Done) {		// quête terminée
			dialogue.overrideStartNode = done;
		} 
	}
}
