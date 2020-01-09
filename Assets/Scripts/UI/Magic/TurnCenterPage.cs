using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCenterPage : MonoBehaviour
{

	public Text title;
	public Text text;
	public Image picture;

	public Text nextPageTitle;
	public Text nextPageText;
	public Image nextPagePicture;



	Animator animator;

	void Start() {
		animator = gameObject.GetComponentInChildren<Animator>();
		gameObject.SetActive(false);

	}

	public void TurnRight() {
		var currentPage = MagicUI.Instance.GetPage();
		var newPage = MagicUI.Instance.GetPage(MagicUI.Instance.currentPageIdx + 1);
		title.text = newPage.title;
		text.text = newPage.text;
		picture.sprite = currentPage.picture;
		nextPagePicture.sprite = newPage.picture;
		MagicUI.Instance.helpButton.gameObject.SetActive(false);
		gameObject.SetActive(true);
		animator.Play("TurnCenterPage");
	}

	public void TurnLeft() {
		var currentPage = MagicUI.Instance.GetPage();
		var newPage = MagicUI.Instance.GetPage(MagicUI.Instance.currentPageIdx - 1);
		title.text = currentPage.title;
		text.text = currentPage.text;
		picture.sprite = newPage.picture;
		nextPageTitle.text = newPage.title;
		nextPageText.text = newPage.text;
		MagicUI.Instance.helpButton.gameObject.SetActive(false);
		gameObject.SetActive(true);
		animator.Play("TurnCenterPageReverse");
	}

	public void End() {
		MagicUI.Instance.NextPage();
		gameObject.SetActive(false);
	}

	public void EndReverse() {
		MagicUI.Instance.PrevPage();
		gameObject.SetActive(false);
	}
}
