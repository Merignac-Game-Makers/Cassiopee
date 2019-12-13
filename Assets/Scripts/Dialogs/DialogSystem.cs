using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogSystem : InteractableObject
{

	public string DialogText;
	DialogUI m_DialogUI;

	public override bool IsInteractable => DialogText!="";



	private void Start() {
		base.Start();
		m_DialogUI = DialogUI.Instance;
	}

	private void Update() {

	}

	//private void OnCollisionEnter(Collision collision) {
	//	m_DialogUI.DisplayText(DialogText);
	//}

	public override void InteractWith(HighlightableObject target) {
		Clicked = false;
		m_DialogUI.DisplayText(DialogText);
		//throw new System.NotImplementedException();
	}
}
