using UnityEngine;
using static QuestBase.QuestStatus;

public class GMDispatcher : DialogueDispatcher
{
	QuestBase cubeInWell;
	QuestBase waterInWell;
	QuestBase magicTraining;
	VIDE_Assign dialogue;

	public int cubeInWellDone;
	public int magicAvailable;
	public int waterInWellRefused;
	public int waterInWellPending;
	public int waterInWellDone;
	public int magicDone;

	bool greetings = false;

	private void Start() {
		cubeInWell = QuestManager.Instance.gameObject.GetComponentInChildren<CubeInWell>();
		waterInWell = QuestManager.Instance.gameObject.GetComponentInChildren<WaterInWell>();
		magicTraining = QuestManager.Instance.gameObject.GetComponentInChildren<MagicTraining>();
		dialogue = gameObject.GetComponent<VIDE_Assign>();
	}

	public override void SetStartNode() {
		cubeInWell.UpdateStatus();
		waterInWell.UpdateStatus();
		magicTraining.UpdateStatus();

		if (cubeInWell.IsDone() && (int)magicTraining.status<(int)Available) {      // le cube est dans le puits et le joueur n'a pas pris les objets magiques
			dialogue.overrideStartNode = cubeInWellDone;
		}
		if ((int)magicTraining.status >= (int)Available) {		// le joueur a pris les objets magiques et a consulté le grimoire
			dialogue.overrideStartNode = magicAvailable;
		}
		if (waterInWell.status == Refused) {					// le joueur a refusé la quête de l'eau
			dialogue.overrideStartNode = waterInWellRefused;
		}
		if (waterInWell.IsPending()) {							// le joueur n'a pas encore rétabli l'eau
			dialogue.overrideStartNode = waterInWellPending;
		}
		if (waterInWell.IsDone()) {								// l'eau est rétablie
			if (magicTraining.IsDone() && greetings) {
				dialogue.overrideStartNode = magicDone;         // -> aider pour l'accès à la pyramide
			} else {
				greetings = true;								// pour n'afficher ce message qu'une fois
				dialogue.overrideStartNode = waterInWellDone;
			}
		}
	}
}
