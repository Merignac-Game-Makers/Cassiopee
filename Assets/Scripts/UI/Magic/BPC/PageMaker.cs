using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PageMaker.Side;

public class PageMaker : MonoBehaviour
{
	public enum Side { left, right }

	public Image picture;
	public Text title;
	public Text text;


	// Start is called before the first frame update
	void Start() {

	}

	public void Make(Page page, Side side) {
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
		title.text = other.title.text;
		text.text = other.text.text;
		picture.enabled = other.picture.enabled;
		picture.sprite = other.picture.sprite;
	}

	public Sprite GetSprite() {
		Texture2D tex = new Texture2D(210, 297);
		RenderTexture rTex = GetComponentInChildren<Camera>().targetTexture;
		RenderTexture.active = rTex;
		tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
		tex.Apply();
		return Sprite.Create(tex, new Rect(0, 0, rTex.width, rTex.height), new Vector2(.5f, .5f));
	}
}
