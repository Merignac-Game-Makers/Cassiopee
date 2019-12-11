using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Handle all the UI code related to the inventory (drag'n'drop of object, using objects, equipping object etc.)
/// </summary>
public class BookUI: MonoBehaviour
{

	public GameObject BookPanel;
	public GameObject openButton;
	public GameObject closeButton;
	public GameObject fullScreenButton;

	public static BookUI Instance;
	bool isFullScreen = false;

	public bool isOpen => IsOpen();


	private void Awake() {
		Instance = this;
	}

	private void Start() {
		BookPanel.SetActive(false);
		fullScreenButton.SetActive(false);
		closeButton.SetActive(false);
		openButton.SetActive(true);
	}

	void OnEnable() {
	}

	private void Update() {
		//Keyboard shortcut
		if (Input.GetKeyUp(KeyCode.B))
			Toggle();
	}

	public void Toggle() {
		BookPanel.SetActive(closeButton.activeInHierarchy && isFullScreen);
		fullScreenButton.SetActive(!fullScreenButton.activeInHierarchy);
		closeButton.SetActive(fullScreenButton.activeInHierarchy);
		openButton.SetActive(!closeButton.activeInHierarchy);
		InventoryUI.Instance.Show(!BookPanel.activeInHierarchy);
	}

	public void Show(bool on) {
		gameObject.SetActive(on);
	}

	public void ToggleFullScreen() {
		isFullScreen = !isFullScreen;
		BookPanel.SetActive(isFullScreen);
		InventoryUI.Instance.Show(!BookPanel.activeInHierarchy);
	}

	public void TurnPage(int i) {

	}

	bool IsOpen() {
		return closeButton.activeInHierarchy;
	}

}
