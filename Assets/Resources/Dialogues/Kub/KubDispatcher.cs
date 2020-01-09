using UnityEngine;
using static QuestBase.QuestStatus;

public class KubDispatcher : DialogueDispatcher
{
	QuestBase cubeInWell;
	QuestBase waterInWell;
	VIDE_Assign dialogue;

	public int cubeInWellFail;
	public int cubeInWellDone;
	public int waterInWellDone;

	private void Start() {
		cubeInWell = QuestManager.Instance.gameObject.GetComponentInChildren<CubeInWell>();
		waterInWell = QuestManager.Instance.gameObject.GetComponentInChildren<WaterInWell>();
		dialogue = gameObject.GetComponent<VIDE_Assign>();
	}

	public override void SetStartNode() {
		cubeInWell.UpdateStatus();
		waterInWell.UpdateStatus();

		if (cubeInWell.status == Failed) {                  // l'objet dans le puits n'est pas un cube
			dialogue.overrideStartNode = cubeInWellFail;
		}
		if (cubeInWell.status == Done) {                    // le cube est dans le puits
			dialogue.overrideStartNode = cubeInWellDone;
		}
		if (waterInWell.status == Done) {					// l'eau est rétablie
			dialogue.overrideStartNode = waterInWellDone;
		} 
	}
}
