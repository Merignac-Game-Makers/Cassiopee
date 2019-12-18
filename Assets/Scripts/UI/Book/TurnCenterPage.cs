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

	// Start is called before the first frame update
	void Start() {
		animator = gameObject.GetComponentInChildren<Animator>();
		gameObject.SetActive(false);

	}

	// Update is called once per frame
	void Update() {

	}

	//public void TurnRight() {
	//    BookUI.Instance.NextPage();
	//}

	public void TurnRight() {
		var currentPage = BookUI.Instance.GetPage();
		var newPage = BookUI.Instance.GetPage(BookUI.Instance.currentPageIdx + 1);
		title.text = newPage.Title;
		text.text = newPage.Text;
		picture.sprite = currentPage.Picture;
		nextPagePicture.sprite = newPage.Picture;
		gameObject.SetActive(true);
		animator.Play("TurnCenterPage");
	}

	public void TurnLeft() {
		var currentPage = BookUI.Instance.GetPage();
		var newPage = BookUI.Instance.GetPage(BookUI.Instance.currentPageIdx - 1);
		title.text = currentPage.Title;
		text.text = currentPage.Text;
		picture.sprite = newPage.Picture;
		nextPageTitle.text = newPage.Title;
		nextPageText.text = newPage.Text;
		gameObject.SetActive(true);
		animator.Play("TurnCenterPageReverse");
	}


	public void End() {
		BookUI.Instance.NextPage();
		gameObject.SetActive(false);
		//animator.StopPlayback();
	}

	public void EndReverse() {
		BookUI.Instance.PrevPage();
		gameObject.SetActive(false);
		//animator.StopPlayback();
	}
}
