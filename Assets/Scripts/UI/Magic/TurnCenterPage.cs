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
		title.text = newPage.Title;
		text.text = newPage.Text;
		picture.sprite = currentPage.Picture;
		nextPagePicture.sprite = newPage.Picture;
		gameObject.SetActive(true);
		animator.Play("TurnCenterPage");
	}

	public void TurnLeft() {
		var currentPage = MagicUI.Instance.GetPage();
		var newPage = MagicUI.Instance.GetPage(MagicUI.Instance.currentPageIdx - 1);
		title.text = currentPage.Title;
		text.text = currentPage.Text;
		picture.sprite = newPage.Picture;
		nextPageTitle.text = newPage.Title;
		nextPageText.text = newPage.Text;
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
