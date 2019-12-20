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
public class BookUI : UIBase
{

	public GameObject fullScreenPanel;
	public GameObject leftPage;
	public GameObject rightPage;


	//public GameObject openButton;
	//public GameObject closeButton;
	public GameObject fullScreenButton;
	public GameObject sidePanel;

	public Text m_Title;
	public Text m_Text;
	public Image m_Picture;

	public GameObject m_NextPage;
	public GameObject m_PrevPage;


	[HideInInspector]
	public BookSystem bookSystem;

	public static BookUI Instance;

	Image background;
	bool isFullScreen = false;

	public bool IsOpen => fullScreenButton.activeInHierarchy;
	[HideInInspector]
	public int currentPageIdx = 0;

	//private void Awake() {
	//	Instance = this;
	//}

	public override void Init() {
		Instance = this;
		gameObject.SetActive(true);
		fullScreenPanel.SetActive(false);
		fullScreenButton.SetActive(false);
		//closeButton.SetActive(false);
		//openButton.SetActive(true);
		sidePanel.SetActive(false);
		background = GetComponent<Image>();
		background.enabled = fullScreenPanel.activeInHierarchy;

		bookSystem = GetComponent<BookSystem>();
		ShowPage(0);
	}

	private void Update() {

		m_NextPage.SetActive(currentPageIdx < bookSystem.MaxPage-1);
		m_PrevPage.SetActive(currentPageIdx > 0);
	}

	public override void Toggle() {
		fullScreenButton.SetActive(!fullScreenButton.activeInHierarchy);
		//closeButton.SetActive(fullScreenButton.activeInHierarchy);
		fullScreenPanel.SetActive(fullScreenButton.activeInHierarchy && isFullScreen);
		sidePanel.SetActive(fullScreenButton.activeInHierarchy);
		//openButton.SetActive(!fullScreenButton.activeInHierarchy);
		InventoryUI.Instance.Show(!fullScreenPanel.activeInHierarchy);
		background.enabled = fullScreenPanel.activeInHierarchy;
	}

	public void ToggleFullScreen() {
		isFullScreen = !isFullScreen;
		fullScreenPanel.SetActive(isFullScreen);
		sidePanel.SetActive(fullScreenButton.activeInHierarchy);
		InventoryUI.Instance.Show(!fullScreenPanel.activeInHierarchy);
		background.enabled = fullScreenPanel.activeInHierarchy;
	}


	void ShowPage(int idx) {
		m_Title.text = bookSystem.pages[idx].Title;
		m_Text.text = bookSystem.pages[idx].Text;
		m_Picture.sprite = bookSystem.pages[idx].Picture;
		currentPageIdx = idx;
	}

	public void NextPage() {
		if (currentPageIdx < bookSystem.MaxPage-1) {
			ShowPage(currentPageIdx+1);
		}
	}

	public void PrevPage() {
		if (currentPageIdx>0) {
			ShowPage(currentPageIdx-1);
		}
	}

	public Page GetPage(int idx) {
		if (idx<bookSystem.MaxPage) {
			return bookSystem.pages[idx];
		} else {
			return null;
		}
	}
	public Page GetPage() {
			return bookSystem.pages[currentPageIdx];
	}

}
