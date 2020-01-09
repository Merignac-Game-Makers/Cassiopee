using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestion de la quête "apprentissage de la magie"
/// </summary>
public class MagicTrainingManager : MonoBehaviour
{													// déclarations :
	public QuestBase quest;							// la quête
	DialoguesUI dialoguesUI;						// le gestionnaire de dialogues
	VIDE_Assign dialogue;                           // le dialogue du grimoire
	DialogueDispatcher dialogueValidation;			// le dispatcher pour changer le point d'entrée du dialogue en fonction de l'avancement de la quête

	bool firstUse = true;

	void Start() {																	// initialisations:
		dialoguesUI = DialoguesUI.Instance;											// le gestionnaire de dialogues
		dialogue = GetComponent<VIDE_Assign>();										// le dialogue
		dialogueValidation = GetComponentInChildren<DialogueDispatcher>();		// le dispatcher pour changer le point d'entrée du dialogue en fonction de l'avancement de la quête
	}

	// à la 1ère utilisation du grimoire
	public void FirtsUse() {
		if (firstUse) {
			firstUse = false;
			Run();					// déclencher le dialogue
		}
	}


	public void Run() {
		if (dialogueValidation!=null)
			dialogueValidation.SetStartNode();
		dialoguesUI.Begin(dialogue);			// débuter le dialogue
		quest.AcceptQuest();					// mettre la quête au statut 'acceptée'
	}

	public void QuestPassed() {
		//dialogueValidation.PassedQuestNode = 5;
		quest.QuestPassed();                    // mettre la quête au statut 'terminée'
		if (dialogueValidation!=null)
			dialogueValidation.SetStartNode();
		dialoguesUI.Begin(dialogue);			// démarrer le dialogue au point d'entrée correspondant
		
	}
}
