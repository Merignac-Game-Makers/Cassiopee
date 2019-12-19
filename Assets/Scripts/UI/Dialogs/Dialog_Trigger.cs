using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VIDE_Data;

public class Dialog_Trigger : MonoBehaviour
{


	DManager dialogManager;
	VIDE_Assign dialog;
	DialogValidation QC;

	// Start is called before the first frame update
	void Start() {
		dialogManager = DManager.Instance;
		dialog = GetComponent<VIDE_Assign>();

		QC = GetComponentInChildren<DialogValidation>();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			dialogManager.End(null);
		}

	}

	public void Run() {
		//if (dialogManager.PreConditions(dialog))
		if (QC!=null && dialog !=null && QC.QuestConditions(dialog))
			dialogManager.Begin(dialog);
	}

	private void OnTriggerEnter(Collider other) {
		Debug.Log(other);
		PlayerControl.Instance.StopAgent();
		Run();
	}

}
