using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
	Animator anim;
	public GameObject magicButton;
	public GameObject inventoryButton;
	public GameObject questButton;
	public GameObject inventory;
	public GameObject Magic;
	public GameObject Quest;


	private void Start() {
		anim = gameObject.GetComponent<Animator>();
	}

	public void animEnd() {
		magicButton.SetActive(false);
		inventoryButton.SetActive(false);
		questButton.SetActive(false);
		inventory.SetActive(false);
		Magic.SetActive(false);
		Quest.SetActive(false);
		anim.Play("End");
	}

	public void Exit() {
		Application.Quit();
	}
}
