using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VIDE_Data;

public class VD_Trigger : MonoBehaviour
{

	DManager dialogManager;
	VIDE_Assign dialog;

	// Start is called before the first frame update
	void Start() {
		dialogManager = DManager.Instance;
		dialog = GetComponent<VIDE_Assign>();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			dialogManager.End(null);
		}

	}

	public void Run() {
		if (dialogManager.PreConditions(dialog))
			dialogManager.Begin(GetComponent<VIDE_Assign>());
	}

	private void OnTriggerEnter(Collider other) {
		Debug.Log(other);
		GetComponentInChildren<VD_Trigger>().Run();
	}
}
