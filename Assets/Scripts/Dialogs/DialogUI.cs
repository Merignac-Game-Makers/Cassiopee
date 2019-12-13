using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
	public static DialogUI Instance;
	public bool isActive => panel.activeInHierarchy;
	public GameObject panel;
	public Text dialogText;

	private void Awake() {
		Instance = this;
	}

	// Start is called before the first frame update
	void Start() {
		//panel = gameObject.GetComponentInChildren<Image>().gameObject;
		panel.SetActive(false);
		//dialogText = gameObject.GetComponentInChildren<Text>();
	}
	// Update is called once per frame
	void Update() {
		if (isActive && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			panel.SetActive(false);
		}
	}

	public void DisplayText(string textToDisplay) {
		dialogText.text = textToDisplay;
		panel.SetActive(true);
		PlayerControl.Instance.StopAgent();
	}

}


