using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTrainingManager : MonoBehaviour
{
	public QuestBase quest;
	DialoguesUI dialoguesUI;
	VIDE_Assign dialogue;
	DialogValidation dialogueValidation;

	bool firstUse = true;

	void Start() {
		dialoguesUI = DialoguesUI.Instance;
		dialogue = GetComponent<VIDE_Assign>();                             // le dialogue
		dialogueValidation = GetComponentInChildren<DialogValidation>();    // le script de validation de dialogue (points d'entrée en fonction du statut de la quête [si elle existe])
	}

	// Update is called once per frame
	void Update() {

	}

	public void FirtsUse() {
		if (firstUse) {
			firstUse = false;
			Run();
		}
	}


	public void Run() {
		if ((dialogueValidation != null && dialogueValidation.QuestConditions(dialogue)) || dialogueValidation == null)
			dialoguesUI.Begin(dialogue);
		quest.AcceptQuest();
	}

	public void QuestPassed() {
		dialogueValidation.PassedQuestNode = 5;
		quest.QuestPassed();
		if ((dialogueValidation != null && dialogueValidation.QuestConditions(dialogue)) || dialogueValidation == null) {
			dialoguesUI.Begin(dialogue);
		}
	}
}
