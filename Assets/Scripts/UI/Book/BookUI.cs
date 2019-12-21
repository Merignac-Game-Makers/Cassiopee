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


	public GameObject bookButton;
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

	bool isFullScreen = false;
	UIManager uiManager;

	public bool IsOpen => fullScreenButton.activeInHierarchy;
	[HideInInspector]
	public int currentPageIdx = 0;

	public override void Init(UIManager uiManager) {
		Instance = this;
		this.uiManager = uiManager;

		gameObject.SetActive(true);
		panel.SetActive(false);

		bookSystem = GetComponent<BookSystem>();
		ShowPage(0);
	}

	private void Update() {
		m_NextPage.SetActive(currentPageIdx < bookSystem.MaxPage-1);
		m_PrevPage.SetActive(currentPageIdx > 0);
	}

	public override void Toggle() {
		panel.SetActive(!panel.activeInHierarchy);
		//fullScreenPanel.SetActive(isOn && isFullScreen);
		sidePanel.SetActive(isOn);
	}

	public void ToggleFullScreen() {
		isFullScreen = !isFullScreen;
		fullScreenPanel.SetActive(isFullScreen);
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
