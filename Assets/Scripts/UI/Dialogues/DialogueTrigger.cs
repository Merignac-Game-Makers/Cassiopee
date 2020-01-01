using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using VIDE_Data;

public class DialogueTrigger : MonoBehaviour
{


	DialoguesUI dialoguesUI;
	VIDE_Assign dialogue;
	DValid dialogueValidation;

	// Start is called before the first frame update
	void Start() {
		dialoguesUI = DialoguesUI.Instance;									// le gestionnaire d'interface de dialogues
		dialogue = GetComponent<VIDE_Assign>();								// le dialogue
		dialogueValidation = GetComponentInChildren<DValid>();	// le script de validation de dialogue (points d'entrée en fonction du statut de la quête [si elle existe])
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			dialoguesUI.End(null);
		}

	}

	public void Run() {
		if ((dialogueValidation != null && dialogueValidation.QuestConditions(dialogue)) || dialogueValidation == null)
			dialoguesUI.Begin(dialogue);
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(DialogueTrigger))]
public class DialogTriggerEditor : Editor
{

	public override void OnInspectorGUI() {

		GUILayout.Label(
@"Attention !
Pour personnaliser le dialogue : 
     - affecter un dialoque dans le composant VIDE_Assign
     - ajouter un DialogValidation spécifique du dialogue");

	}
}
#endif
