using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
	Animator anim;


	private void Start() {
		anim = gameObject.GetComponent<Animator>();
	}

	public void animEnd() {
		UIManager.Instance.ManageButtons(UIManager.State.end);
		anim.Play("End");
	}

	public void Exit() {
		Application.Quit();
	}
}
