using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{

	public KeyCode key;
	public UIBase target;
	public Sprite open;
	public Sprite close;

	Image image;

	private void Start() {
		image = GetComponent<Image>();
		image.sprite = open;
	}

	// Update is called once per frame
	void Update() {
		//Keyboard shortcut
		if (Input.GetKeyUp(key))
			Toggle();
	}

	public void Toggle() {
		target.Toggle();
		image.sprite = image.sprite == target.isOn ? close : open;
	}
}
