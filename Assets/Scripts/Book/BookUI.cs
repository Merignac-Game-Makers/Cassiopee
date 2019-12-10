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

	public GameObject InvPanel;
	public GameObject openButton;
	public GameObject closeButton;

	public static BookUI Instance;

	public bool isOpen => IsOpen();


	private void Awake() {
		Instance = this;
	}

	private void Start() {
		InvPanel.SetActive(false);
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
		InvPanel.SetActive(!InvPanel.activeInHierarchy);
		closeButton.SetActive(InvPanel.activeInHierarchy);
		openButton.SetActive(!closeButton.activeInHierarchy);
	}


	bool IsOpen() {
		return closeButton.activeInHierarchy;
	}

}
