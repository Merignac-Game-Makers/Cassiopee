using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using VIDE_Data;

public class DialogueTrigger : MonoBehaviour
{


	DialoguesUI dialoguesUI;
	VIDE_Assign dialog;
	DialogValidation QC;

	// Start is called before the first frame update
	void Start() {
		dialoguesUI = DialoguesUI.Instance;
		dialog = GetComponent<VIDE_Assign>();

		QC = GetComponentInChildren<DialogValidation>();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			dialoguesUI.End(null);
		}

	}

	public void Run() {
		//if (dialogManager.PreConditions(dialog))
		if (QC != null && dialog.assignedID != 0 && QC.QuestConditions(dialog))
			dialoguesUI.Begin(dialog);
	}

	private void OnTriggerEnter(Collider other) {
		Debug.Log(other);
		PlayerControl.Instance.StopAgent();
		Run();
	}



}

#if UNITY_EDITOR
[CustomEditor(typeof(DialogueTrigger))]
public class DialogTriggerEditor : Editor
{

	public override void OnInspectorGUI() {

		GUILayout.Label("Attention !\nIl est impératif d'ajouter les scripts : \n  - VIDE_Assign\n  - DialogValidation spécifique du dialogue");

	}
}
#endif
