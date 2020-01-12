using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PageMaker.Side;

public class PageMaker : MonoBehaviour
{
	public enum Side { left, right }

	public Image picture;
	public TMP_Text title;
	public TMP_Text text;

	Page page;


	public void Make(Page page, Side side) {
		this.page = page;
		if (side == left) {
			title.text = page.title;
			text.text = page.text;
			picture.enabled = false;
		} else {
			title.text = "";
			text.text = "";
			picture.enabled = true;
			picture.sprite = page.picture;
		}
	}
	public void Make(PageMaker other) {
		page = other.page;
		title.text = other.title.text;
		text.text = other.text.text;
		picture.enabled = other.picture.enabled;
		picture.sprite = other.picture.sprite;
	}

	public void ToggleHelp(bool on) {
		if (picture.enabled) {
			picture.sprite = on ? page.helpPicture : page.picture; 
		}
	}

	public Sprite GetSprite() {
		RenderTexture rTex = GetComponentInChildren<Camera>().targetTexture;
		Texture2D tex = new Texture2D(rTex.width, rTex.height);
		RenderTexture.active = rTex;
		tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
		tex.Apply();
		return Sprite.Create(tex, new Rect(0, 0, rTex.width, rTex.height), new Vector2(.5f, .5f));
	}
}
