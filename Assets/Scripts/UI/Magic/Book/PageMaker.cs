using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PageMaker.Side;

public class PageMaker : MonoBehaviour
{
	public enum Side { left, right }

	public GameObject leftPage;
	public TMP_Text title;
	public TMP_Text text;

	public GameObject rightPage;
	public Image picture;

	Page page;
	RenderTexture rt;
	Camera cam;
	Texture2D tex;

	public void Awake() {
		cam = GetComponentInChildren<Camera>();
		rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32) {
			useMipMap = false,
			antiAliasing = 1,
			height = 297,
			width = 210
		};
		cam.targetTexture = rt;
		tex = new Texture2D(rt.width, rt.height);
	}


	//public void Make(PageMaker other) {
	//	page = other.page;
	//	title.text = other.title.text;
	//	text.text = other.text.text;
	//	picture.enabled = other.picture.enabled;
	//	picture.sprite = other.picture.sprite;
	//}

	public void ToggleHelp(bool on) {
		picture.sprite = on ? page.helpPicture : page.picture;
	}

	public IEnumerator IGetSprite(Page page, Side side) {
		// make page
		this.page = page;
		if (side == left) {
			leftPage.SetActive(true);
			rightPage.SetActive(false);
			title.text = page.title;
			text.text = page.text;
		} else {
			leftPage.SetActive(false);
			rightPage.SetActive(true);
			picture.sprite = page.picture;
		}
		yield return new WaitForEndOfFrame();
		// get sprite from renderTexture
		RenderTexture.active = rt;
		tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
		tex.Apply();
		Sprite s = Sprite.Create(tex, new Rect(0, 0, rt.width, rt.height), new Vector2(.5f, .5f));
		RenderTexture.active = null;
		yield return s;
	}



}
