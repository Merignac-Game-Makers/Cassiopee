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
public class BookUI : MonoBehaviour
{

	public GameObject FullScreenPanel;
	public GameObject openButton;
	public GameObject closeButton;
	public GameObject fullScreenButton;

	public GameObject m_Title;
	public GameObject m_Text;
	public GameObject m_Picture;

	public GameObject m_NextPage;
	public GameObject m_PrevPage;

	[HideInInspector]
	public BookSystem bookSystem;

	public static BookUI Instance;
	bool isFullScreen = false;

	public bool isOpen => IsOpen();
	[HideInInspector]
	public int currentPage = 0;

	private void Awake() {
		Instance = this;
	}

	private void Start() {
		FullScreenPanel.SetActive(false);
		fullScreenButton.SetActive(false);
		closeButton.SetActive(false);
		openButton.SetActive(true);

		bookSystem = GetComponent<BookSystem>();
		ShowPage();
	}

	void OnEnable() {
	}

	private void Update() {
		//Keyboard shortcut
		if (Input.GetKeyUp(KeyCode.B))
			Toggle();

		m_NextPage.SetActive(currentPage < bookSystem.MaxPage-1);
		m_PrevPage.SetActive(currentPage > 0);
	}

	public void Toggle() {
		fullScreenButton.SetActive(!fullScreenButton.activeInHierarchy);
		closeButton.SetActive(fullScreenButton.activeInHierarchy);
		FullScreenPanel.SetActive(closeButton.activeInHierarchy && isFullScreen);
		openButton.SetActive(!closeButton.activeInHierarchy);
		InventoryUI.Instance.Show(!FullScreenPanel.activeInHierarchy);
	}

	public void Show(bool on) {
		gameObject.SetActive(on);
	}

	public void ToggleFullScreen() {
		isFullScreen = !isFullScreen;
		FullScreenPanel.SetActive(isFullScreen);
		InventoryUI.Instance.Show(!FullScreenPanel.activeInHierarchy);
	}


	void ShowPage() {
		m_Title.GetComponent<Text>().text = bookSystem.pages[currentPage].Title;
		m_Text.GetComponent<Text>().text = bookSystem.pages[currentPage].Text;
		m_Picture.GetComponent<Image>().sprite = bookSystem.pages[currentPage].Picture;
	}

	public void NextPage() {
		if (bookSystem.GetNextPage(this))
			ShowPage();
	}

	public void PrevPage() {
		if (bookSystem.GetPrevPage(this))
			ShowPage();
	}

	bool IsOpen() {
		return closeButton.activeInHierarchy;
	}

}
