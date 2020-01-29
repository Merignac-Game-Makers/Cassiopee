using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
	public TMP_Text title, text;

	Animator animator;

	private void Awake() {
		animator = GetComponent<Animator>();
	}

	public void Toggle(bool on) {
		if (on)
			animator.SetTrigger("open");
		else
			animator.SetTrigger("close");
	}
	public void Set(string title, string text) {
		this.title.text = title;
		this.text.text = text;
	}

	public void Show(float s) {
		StartCoroutine(Ishow(s));
	}
	IEnumerator Ishow(float s) {
		Toggle(true);
		yield return new WaitForSeconds(s);
		Toggle(false);
	}
}
