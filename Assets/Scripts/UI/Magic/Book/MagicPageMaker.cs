using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicPageMaker : PageMaker
{
	public TMP_Text title;
	public TMP_Text text;
	public Image picture;
	public Button helpButton;

	[HideInInspector]
	public Page page => GetComponent<PageTemplate>().page;
	bool helpIsOn = false;


	public override void Make() {
		title.text = page.title;
		text.text = page.text;
		picture.sprite = page.picture;
		helpButton.gameObject.SetActive(page.hasHelp);
	}

	public void ToggleHelp() {
		helpIsOn = !helpIsOn;
		picture.sprite = helpIsOn ? page.helpPicture : page.picture;
	}

	public override void SetSide(Side side) {
		leftPage.SetActive(side == Side.left);
		rightPage.SetActive(side == Side.right);
	}
}
